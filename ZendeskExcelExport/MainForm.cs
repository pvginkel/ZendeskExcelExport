using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace ZendeskExcelExport
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            using (var key = Program.BaseKey)
            {
                _url.Text = key.GetValue("URL") as string;
                _userName.Text = key.GetValue("User name") as string;
                _includeClosed.Checked = key.GetBoolean("Include closed").GetValueOrDefault();
            }

            UpdateEnabled();
        }

        private void _url_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _userName_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _password_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _export.Enabled =
                _url.Text.Length > 0 &&
                _userName.Text.Length > 0 &&
                _password.Text.Length > 0;
        }

        private void _export_Click(object sender, EventArgs e)
        {
            using (var key = Program.BaseKey)
            {
                key.SetValue("URL", _url.Text);
                key.SetValue("User name", _userName.Text);
                key.SetValue("Include closed", _includeClosed.Checked ? 1 : 0);
            }

            Tickets tickets;

            try
            {
                tickets = GetTickets(_url.Text, _userName.Text, _password.Text, _includeClosed.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "Could not retrieve tickets" + Environment.NewLine + Environment.NewLine + ex.Message,
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            ExportTickets(tickets);
        }

        private void ExportTickets(Tickets tickets)
        {
            string fileName;

            using (var form = new SaveFileDialog())
            {
                form.Filter = "Excel (*.xls)|*.xls|All Files (*.*)|*.*";
                form.RestoreDirectory = true;

                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                fileName = form.FileName;
            }

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Zendesk requests");

            var headerStyle = CreateHeaderStyle(workbook);
            var dateStyle = CreateDateStyle(workbook);
            var wrapStyle = workbook.CreateCellStyle();
            wrapStyle.WrapText = true;
            var urlFont = workbook.CreateFont();
            urlFont.Underline = FontUnderlineType.Single;
            urlFont.Color = workbook.GetCustomPalette().FindColor(0, 0, 255).Indexed;
            var urlStyle = workbook.CreateCellStyle();
            urlStyle.SetFont(urlFont);

            // Create the headers.

            int offset = 0;
            var row = sheet.CreateRow(0);
            AddHeader(row, ref offset, "ID", headerStyle);
            AddHeader(row, ref offset, "Status", headerStyle);
            AddHeader(row, ref offset, "Priority", headerStyle);
            AddHeader(row, ref offset, "Type", headerStyle);
            AddHeader(row, ref offset, "Subject", headerStyle);
            AddHeader(row, ref offset, "Description", headerStyle);
            AddHeader(row, ref offset, "Requester", headerStyle);
            AddHeader(row, ref offset, "Assignee", headerStyle);
            AddHeader(row, ref offset, "Created", headerStyle);
            AddHeader(row, ref offset, "Updated", headerStyle);
            AddHeader(row, ref offset, "Due", headerStyle);

            for (int i = 0; i < tickets.Items.Count; i++)
            {
                row = sheet.CreateRow(i + 1);
                var ticket = tickets.Items[i];
                offset = 0;

                AddUrl(row, ref offset, (long)ticket["id"], (string)ticket["url"], urlStyle);
                AddCell(row, ref offset, Prettify((string)ticket["status"]));
                AddCell(row, ref offset, Prettify((string)ticket["priority"]));
                AddCell(row, ref offset, Prettify((string)ticket["type"]));
                AddCell(row, ref offset, (string)ticket["subject"]);
                AddCell(row, ref offset, (string)ticket["description"], wrapStyle);
                AddCell(row, ref offset, GetUser(tickets.Users, (long?)ticket["requester_id"]));
                AddCell(row, ref offset, GetUser(tickets.Users, (long?)ticket["assignee_id"]));
                AddCell(row, ref offset, (DateTime?)ticket["created_at"], dateStyle);
                AddCell(row, ref offset, (DateTime?)ticket["updated_at"], dateStyle);
                AddCell(row, ref offset, (DateTime?)ticket["due"], dateStyle);
            }

            using (var stream = File.Create(fileName))
            {
                workbook.Write(stream);
            }

            try
            {
                Process.Start(fileName);
            }
            catch
            {
                // Ignore exceptions.
            }
        }

        private object GetUser(Dictionary<long, string> users, long? id)
        {
            if (id == null)
                return null;

            return users[id.Value];
        }

        private object Prettify(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            return value.Substring(0, 1).ToUpper() + value.Substring(1);
        }

        private void AddCell(IRow row, ref int offset, object value)
        {
            AddCell(row, ref offset, value, null);
        }

        private void AddCell(IRow row, ref int offset, object value, ICellStyle style)
        {
            var cell = row.CreateCell(offset++);
            SetValue(cell, value);
            if (style != null)
                cell.CellStyle = style;
        }

        private void AddUrl(IRow row, ref int offset, object value, string url, ICellStyle urlStyle)
        {
            var cell = row.CreateCell(offset++);
            SetValue(cell, value);
            cell.CellStyle = urlStyle;

            cell.Hyperlink = new HSSFHyperlink(HyperlinkType.Url)
            {
                Address = url
            };
        }

        private void AddHeader(IRow row, ref int offset, object value, ICellStyle headerStyle)
        {
            var cell = row.CreateCell(offset++);
            SetValue(cell, value);
            cell.CellStyle = headerStyle;
        }

        private void SetValue(ICell cell, object value)
        {
            if (value == null || value is string)
            {
                string stringValue = (string)value;
                // NPOI supports a maximum string contents of 32767 characters.
                if (stringValue != null && stringValue.Length > 32767)
                    stringValue = stringValue.Substring(0, 32767);
                cell.SetCellValue(stringValue);
            }
            else if (value is int)
            {
                cell.SetCellValue((int)value);
            }
            else if (value is long)
            {
                cell.SetCellValue((long)value);
            }
            else if (value is double)
            {
                cell.SetCellValue((double)value);
            }
            else if (value is DateTime)
            {
                cell.SetCellValue((DateTime)value);
            }
            else
            {
                throw new ArgumentException("Invalid type");
            }
        }

        private static ICellStyle CreateDateStyle(HSSFWorkbook workbook)
        {
            var dateStyle = workbook.CreateCellStyle();

            var dataFormat = workbook.CreateDataFormat();

            var dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;

            string format = dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.ShortTimePattern;

            if (format.EndsWith(" tt") || format.EndsWith(" TT"))
                format = format.Substring(0, format.Length - 2) + "AM/PM";

            dateStyle.DataFormat = dataFormat.GetFormat(format);

            return dateStyle;
        }

        private static ICellStyle CreateHeaderStyle(HSSFWorkbook workbook)
        {
            var palette = workbook.GetCustomPalette();

            var style = workbook.CreateCellStyle();

            style.FillForegroundColor = palette.FindColor(192, 192, 192).Indexed;
            style.FillPattern = FillPattern.SolidForeground;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            var font = workbook.CreateFont();

            font.Boldweight = (short)FontBoldWeight.Bold;

            style.SetFont(font);

            return style;
        }

        private Tickets GetTickets(string url, string userName, string password, bool includeClosed)
        {
            _toolStripLabel.Text = "Retrieving user information";
            Update();

            var user = LoadJson(url, userName, password, "/api/v2/users/me.json");

            long organizationId = (long)((JObject)user["user"])["organization_id"];

            var response = new List<JObject>();
            var users = new Dictionary<long, string>();

            _toolStripLabel.Text = "Retrieving tickets";
            Update();

            int page = 1;

            string status = "new,open,pending,hold,solved";
            if (includeClosed)
                status += ",closed";

            var requests = LoadJson(url, userName, password, $"/api/v2/organizations/{organizationId}/requests.json?include=users&status={status}");

            AddTickets(requests, response);
            AddUsers(requests, users);

            string nextPage = (string)requests["next_page"];

            while (nextPage != null)
            {
                page++;
                _toolStripLabel.Text = "Retrieving tickets";

                requests = LoadJson(url, userName, password, nextPage);

                AddTickets(requests, response);
                AddUsers(requests, users);

                nextPage = (string)requests["next_page"];
            }

            _toolStripLabel.Text = null;

            return new Tickets(response, users);
        }

        private void AddUsers(JObject requests, Dictionary<long, string> users)
        {
            foreach (var user in (JArray)requests["users"])
            {
                users[(long)user["id"]] = (string)user["name"];
            }
        }

        private static void AddTickets(JObject requests, List<JObject> response)
        {
            if (requests["requests"] is JArray)
                response.AddRange(((JArray)requests["requests"]).Select(p => (JObject)p));
        }

        private JObject LoadJson(string url, string userName, string password, string api)
        {
            if (url.EndsWith("/"))
                url = url.Substring(0, url.Length - 1);

            if (!api.Contains("://"))
                api = url + api;

            var request = (HttpWebRequest)WebRequest.Create(api);

            string authorization = userName + ":" + password;

            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authorization)));

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return JObject.Load(jsonReader);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (_url.Text.Length == 0)
                _url.Focus();
            else if (_userName.Text.Length == 0)
                _userName.Focus();
            else
                _password.Focus();
        }

        private class Tickets
        {
            public List<JObject> Items { get; private set; }
            public Dictionary<long, string> Users { get; private set; }

            public Tickets(List<JObject> items, Dictionary<long, string> users)
            {
                Items = items;
                Users = users;
            }
        }
    }
}
