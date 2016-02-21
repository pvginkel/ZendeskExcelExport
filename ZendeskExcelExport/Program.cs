using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ZendeskExcelExport
{
    internal static class Program
    {
        public static RegistryKey BaseKey => Registry.CurrentUser.CreateSubKey("Software\\Zendesk Excel Export");

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
