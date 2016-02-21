namespace ZendeskExcelExport
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._url = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._userName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._password = new System.Windows.Forms.TextBox();
            this._includeClosed = new System.Windows.Forms.CheckBox();
            this._export = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._toolStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zendesk URL:";
            // 
            // _url
            // 
            this._url.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._url.Location = new System.Drawing.Point(96, 12);
            this._url.Name = "_url";
            this._url.Size = new System.Drawing.Size(326, 20);
            this._url.TabIndex = 1;
            this._url.TextChanged += new System.EventHandler(this._url_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "E-mail address:";
            // 
            // _userName
            // 
            this._userName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._userName.Location = new System.Drawing.Point(96, 38);
            this._userName.Name = "_userName";
            this._userName.Size = new System.Drawing.Size(326, 20);
            this._userName.TabIndex = 3;
            this._userName.TextChanged += new System.EventHandler(this._userName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // _password
            // 
            this._password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._password.Location = new System.Drawing.Point(96, 64);
            this._password.Name = "_password";
            this._password.Size = new System.Drawing.Size(326, 20);
            this._password.TabIndex = 5;
            this._password.UseSystemPasswordChar = true;
            this._password.TextChanged += new System.EventHandler(this._password_TextChanged);
            // 
            // _includeClosed
            // 
            this._includeClosed.AutoSize = true;
            this._includeClosed.Location = new System.Drawing.Point(95, 90);
            this._includeClosed.Name = "_includeClosed";
            this._includeClosed.Size = new System.Drawing.Size(129, 17);
            this._includeClosed.TabIndex = 6;
            this._includeClosed.Text = "Include closed tickets";
            this._includeClosed.UseVisualStyleBackColor = true;
            // 
            // _export
            // 
            this._export.Location = new System.Drawing.Point(347, 113);
            this._export.Name = "_export";
            this._export.Size = new System.Drawing.Size(75, 23);
            this._export.TabIndex = 7;
            this._export.Text = "Export";
            this._export.UseVisualStyleBackColor = true;
            this._export.Click += new System.EventHandler(this._export_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 149);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(434, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // _toolStripLabel
            // 
            this._toolStripLabel.Name = "_toolStripLabel";
            this._toolStripLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainForm
            // 
            this.AcceptButton = this._export;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 171);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._export);
            this.Controls.Add(this._includeClosed);
            this.Controls.Add(this._password);
            this.Controls.Add(this._userName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._url);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Zendesk Excel Export";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _url;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _userName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _password;
        private System.Windows.Forms.CheckBox _includeClosed;
        private System.Windows.Forms.Button _export;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripLabel;
    }
}

