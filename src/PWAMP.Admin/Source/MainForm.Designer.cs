using System.Drawing;
using System.Windows.Forms;

namespace Pwamp.Admin
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
            this.btnStartApache = new System.Windows.Forms.Button();
            this.btnStopApache = new System.Windows.Forms.Button();
            this.btnStartMySql = new System.Windows.Forms.Button();
            this.btnStopMysql = new System.Windows.Forms.Button();
            this.logsPanel = new System.Windows.Forms.Panel();
            this.logsHeaderLabel = new System.Windows.Forms.Label();
            this.logTabControl = new System.Windows.Forms.TabControl();
            this.outputTab = new System.Windows.Forms.TabPage();
            this._logTextBox = new System.Windows.Forms.RichTextBox();
            this.errorTab = new System.Windows.Forms.TabPage();
            this.accessTab = new System.Windows.Forms.TabPage();
            this.clearLogsBtn = new System.Windows.Forms.Button();
            this.refreshLogsBtn = new System.Windows.Forms.Button();
            this.exportLogsBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this._apacheModule = new Pwamp.Admin.UI.ApacheControl();
            this.logsPanel.SuspendLayout();
            this.logTabControl.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartApache
            // 
            this.btnStartApache.Location = new System.Drawing.Point(450, 72);
            this.btnStartApache.Name = "btnStartApache";
            this.btnStartApache.Size = new System.Drawing.Size(138, 23);
            this.btnStartApache.TabIndex = 0;
            this.btnStartApache.Text = "Start Apache";
            this.btnStartApache.UseVisualStyleBackColor = true;
            this.btnStartApache.Click += new System.EventHandler(this.BtnStartApache_Click);
            // 
            // btnStopApache
            // 
            this.btnStopApache.Location = new System.Drawing.Point(594, 72);
            this.btnStopApache.Name = "btnStopApache";
            this.btnStopApache.Size = new System.Drawing.Size(163, 23);
            this.btnStopApache.TabIndex = 1;
            this.btnStopApache.Text = "Stop";
            this.btnStopApache.UseVisualStyleBackColor = true;
            this.btnStopApache.Click += new System.EventHandler(this.BtnStopApache_Click);
            // 
            // btnStartMySql
            // 
            this.btnStartMySql.Location = new System.Drawing.Point(450, 134);
            this.btnStartMySql.Name = "btnStartMySql";
            this.btnStartMySql.Size = new System.Drawing.Size(138, 32);
            this.btnStartMySql.TabIndex = 2;
            this.btnStartMySql.Text = "Start MySQL";
            this.btnStartMySql.UseVisualStyleBackColor = true;
            this.btnStartMySql.Click += new System.EventHandler(this.BtnStartMySql_Click);
            // 
            // btnStopMysql
            // 
            this.btnStopMysql.Location = new System.Drawing.Point(619, 134);
            this.btnStopMysql.Name = "btnStopMysql";
            this.btnStopMysql.Size = new System.Drawing.Size(138, 32);
            this.btnStopMysql.TabIndex = 3;
            this.btnStopMysql.Text = "Stop MySQL";
            this.btnStopMysql.UseVisualStyleBackColor = true;
            this.btnStopMysql.Click += new System.EventHandler(this.BtnStopMysql_Click);
            // 
            // logsPanel
            // 
            this.logsPanel.BackColor = System.Drawing.Color.White;
            this.logsPanel.Controls.Add(this.logsHeaderLabel);
            this.logsPanel.Controls.Add(this.logTabControl);
            this.logsPanel.Controls.Add(this.clearLogsBtn);
            this.logsPanel.Controls.Add(this.refreshLogsBtn);
            this.logsPanel.Controls.Add(this.exportLogsBtn);
            this.logsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsPanel.Location = new System.Drawing.Point(0, 0);
            this.logsPanel.Name = "logsPanel";
            this.logsPanel.Size = new System.Drawing.Size(803, 395);
            this.logsPanel.TabIndex = 5;
            // 
            // logsHeaderLabel
            // 
            this.logsHeaderLabel.AutoSize = true;
            this.logsHeaderLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.logsHeaderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.logsHeaderLabel.Location = new System.Drawing.Point(20, 15);
            this.logsHeaderLabel.Name = "logsHeaderLabel";
            this.logsHeaderLabel.Size = new System.Drawing.Size(145, 25);
            this.logsHeaderLabel.TabIndex = 0;
            this.logsHeaderLabel.Text = "📋 System Logs";
            // 
            // logTabControl
            // 
            this.logTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTabControl.Controls.Add(this.outputTab);
            this.logTabControl.Controls.Add(this.errorTab);
            this.logTabControl.Controls.Add(this.accessTab);
            this.logTabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.logTabControl.Location = new System.Drawing.Point(20, 50);
            this.logTabControl.Name = "logTabControl";
            this.logTabControl.SelectedIndex = 0;
            this.logTabControl.Size = new System.Drawing.Size(521, 272);
            this.logTabControl.TabIndex = 1;
            // 
            // outputTab
            // 
            this.outputTab.BackColor = System.Drawing.Color.White;
            this.outputTab.Controls.Add(this._logTextBox);
            this.outputTab.Location = new System.Drawing.Point(4, 24);
            this.outputTab.Name = "outputTab";
            this.outputTab.Padding = new System.Windows.Forms.Padding(3);
            this.outputTab.Size = new System.Drawing.Size(513, 244);
            this.outputTab.TabIndex = 0;
            this.outputTab.Text = "Output Logs";
            // 
            // _logTextBox
            // 
            this._logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._logTextBox.BackColor = System.Drawing.Color.Black;
            this._logTextBox.Font = new System.Drawing.Font("Consolas", 9F);
            this._logTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this._logTextBox.Location = new System.Drawing.Point(3, 3);
            this._logTextBox.Name = "_logTextBox";
            this._logTextBox.ReadOnly = true;
            this._logTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._logTextBox.Size = new System.Drawing.Size(0, 0);
            this._logTextBox.TabIndex = 0;
            this._logTextBox.Text = "";
            // 
            // errorTab
            // 
            this.errorTab.Location = new System.Drawing.Point(4, 24);
            this.errorTab.Name = "errorTab";
            this.errorTab.Padding = new System.Windows.Forms.Padding(3);
            this.errorTab.Size = new System.Drawing.Size(513, 244);
            this.errorTab.TabIndex = 1;
            this.errorTab.Text = "Error Logs";
            this.errorTab.UseVisualStyleBackColor = true;
            // 
            // accessTab
            // 
            this.accessTab.Location = new System.Drawing.Point(4, 24);
            this.accessTab.Name = "accessTab";
            this.accessTab.Padding = new System.Windows.Forms.Padding(3);
            this.accessTab.Size = new System.Drawing.Size(513, 244);
            this.accessTab.TabIndex = 2;
            this.accessTab.Text = "Access Logs";
            this.accessTab.UseVisualStyleBackColor = true;
            // 
            // clearLogsBtn
            // 
            this.clearLogsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearLogsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.clearLogsBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearLogsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearLogsBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.clearLogsBtn.ForeColor = System.Drawing.Color.White;
            this.clearLogsBtn.Location = new System.Drawing.Point(21, 328);
            this.clearLogsBtn.Name = "clearLogsBtn";
            this.clearLogsBtn.Size = new System.Drawing.Size(80, 30);
            this.clearLogsBtn.TabIndex = 2;
            this.clearLogsBtn.Text = "🗑️ Clear";
            this.clearLogsBtn.UseVisualStyleBackColor = false;
            // 
            // refreshLogsBtn
            // 
            this.refreshLogsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshLogsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.refreshLogsBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.refreshLogsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshLogsBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.refreshLogsBtn.ForeColor = System.Drawing.Color.White;
            this.refreshLogsBtn.Location = new System.Drawing.Point(111, 328);
            this.refreshLogsBtn.Name = "refreshLogsBtn";
            this.refreshLogsBtn.Size = new System.Drawing.Size(80, 30);
            this.refreshLogsBtn.TabIndex = 3;
            this.refreshLogsBtn.Text = "🔄 Refresh";
            this.refreshLogsBtn.UseVisualStyleBackColor = false;
            // 
            // exportLogsBtn
            // 
            this.exportLogsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportLogsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.exportLogsBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exportLogsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportLogsBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.exportLogsBtn.ForeColor = System.Drawing.Color.White;
            this.exportLogsBtn.Location = new System.Drawing.Point(201, 328);
            this.exportLogsBtn.Name = "exportLogsBtn";
            this.exportLogsBtn.Size = new System.Drawing.Size(80, 30);
            this.exportLogsBtn.TabIndex = 4;
            this.exportLogsBtn.Text = "💾 Export";
            this.exportLogsBtn.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.logsPanel);
            this.panel1.Location = new System.Drawing.Point(13, 289);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 395);
            this.panel1.TabIndex = 6;
            // 
            // _apacheModule
            // 
            this._apacheModule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._apacheModule.Location = new System.Drawing.Point(12, 12);
            this._apacheModule.Name = "_apacheModule";
            this._apacheModule.Size = new System.Drawing.Size(407, 177);
            this._apacheModule.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 794);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._apacheModule);
            this.Controls.Add(this.btnStopMysql);
            this.Controls.Add(this.btnStartMySql);
            this.Controls.Add(this.btnStopApache);
            this.Controls.Add(this.btnStartApache);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PWAMP Control Panel";
            this.logsPanel.ResumeLayout(false);
            this.logsPanel.PerformLayout();
            this.logTabControl.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Button btnStartApache;
        private Button btnStopApache;
        private Button btnStartMySql;
        private Button btnStopMysql;
        private UI.ApacheControl _apacheModule;
        private Panel logsPanel;
        private Label logsHeaderLabel;
        private TabControl logTabControl;
        private TabPage outputTab;
        private RichTextBox _logTextBox;
        private TabPage errorTab;
        private TabPage accessTab;
        private Button clearLogsBtn;
        private Button refreshLogsBtn;
        private Button exportLogsBtn;
        private Panel panel1;
    }
}
