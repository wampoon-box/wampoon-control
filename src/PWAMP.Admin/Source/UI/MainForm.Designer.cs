using System.Drawing;
using System.Windows.Forms;

namespace Frostybee.PwampAdmin.UI
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
            this.logsPanel = new System.Windows.Forms.Panel();
            this.logsHeaderLabel = new System.Windows.Forms.Label();
            this.logTabControl = new System.Windows.Forms.TabControl();
            this.outputTab = new System.Windows.Forms.TabPage();
            this._logTextBox = new System.Windows.Forms.RichTextBox();
            this.errorTab = new System.Windows.Forms.TabPage();
            this._errorLogTextBox = new System.Windows.Forms.RichTextBox();
            this.accessTab = new System.Windows.Forms.TabPage();
            this.btnExportLogs = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._mySqlModule = new Frostybee.PwampAdmin.Controls.MySqlControl();
            this._apacheModule = new Frostybee.PwampAdmin.Controls.ApacheControl();
            this.grpHelpers = new System.Windows.Forms.GroupBox();
            this.btnStopAllServers = new System.Windows.Forms.Button();
            this.btnOpenExplorer = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.logsPanel.SuspendLayout();
            this.logTabControl.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.errorTab.SuspendLayout();
            this.panel1.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpHelpers.SuspendLayout();
            this.SuspendLayout();
            // 
            // logsPanel
            // 
            this.logsPanel.BackColor = System.Drawing.Color.White;
            this.logsPanel.Controls.Add(this.logsHeaderLabel);
            this.logsPanel.Controls.Add(this.logTabControl);
            this.logsPanel.Controls.Add(this.btnExportLogs);
            this.logsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsPanel.Location = new System.Drawing.Point(0, 0);
            this.logsPanel.Name = "logsPanel";
            this.logsPanel.Size = new System.Drawing.Size(814, 395);
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
            this.logTabControl.Size = new System.Drawing.Size(791, 272);
            this.logTabControl.TabIndex = 1;
            // 
            // outputTab
            // 
            this.outputTab.BackColor = System.Drawing.Color.White;
            this.outputTab.Controls.Add(this._logTextBox);
            this.outputTab.Location = new System.Drawing.Point(4, 24);
            this.outputTab.Name = "outputTab";
            this.outputTab.Padding = new System.Windows.Forms.Padding(3);
            this.outputTab.Size = new System.Drawing.Size(783, 244);
            this.outputTab.TabIndex = 0;
            this.outputTab.Text = "Output Logs";
            // 
            // _logTextBox
            // 
            this._logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._logTextBox.BackColor = System.Drawing.Color.White;
            this._logTextBox.Font = new System.Drawing.Font("Consolas", 10F);
            this._logTextBox.ForeColor = System.Drawing.Color.Black;
            this._logTextBox.Location = new System.Drawing.Point(13, 3);
            this._logTextBox.Name = "_logTextBox";
            this._logTextBox.ReadOnly = true;
            this._logTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._logTextBox.Size = new System.Drawing.Size(777, 238);
            this._logTextBox.TabIndex = 0;
            this._logTextBox.Text = "";
            // 
            // errorTab
            // 
            this.errorTab.BackColor = System.Drawing.Color.White;
            this.errorTab.Controls.Add(this._errorLogTextBox);
            this.errorTab.Location = new System.Drawing.Point(4, 24);
            this.errorTab.Name = "errorTab";
            this.errorTab.Padding = new System.Windows.Forms.Padding(3);
            this.errorTab.Size = new System.Drawing.Size(783, 244);
            this.errorTab.TabIndex = 1;
            this.errorTab.Text = "Error Logs";
            this.errorTab.UseVisualStyleBackColor = true;
            // 
            // _errorLogTextBox
            // 
            this._errorLogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._errorLogTextBox.BackColor = System.Drawing.Color.White;
            this._errorLogTextBox.Font = new System.Drawing.Font("Consolas", 10F);
            this._errorLogTextBox.ForeColor = System.Drawing.Color.Red;
            this._errorLogTextBox.Location = new System.Drawing.Point(16, 6);
            this._errorLogTextBox.Name = "_errorLogTextBox";
            this._errorLogTextBox.ReadOnly = true;
            this._errorLogTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._errorLogTextBox.Size = new System.Drawing.Size(1354, 376);
            this._errorLogTextBox.TabIndex = 0;
            this._errorLogTextBox.Text = "";
            // 
            // accessTab
            // 
            this.accessTab.Location = new System.Drawing.Point(4, 24);
            this.accessTab.Name = "accessTab";
            this.accessTab.Padding = new System.Windows.Forms.Padding(3);
            this.accessTab.Size = new System.Drawing.Size(783, 244);
            this.accessTab.TabIndex = 2;
            this.accessTab.Text = "Access Logs";
            this.accessTab.UseVisualStyleBackColor = true;
            // 
            // btnExportLogs
            // 
            this.btnExportLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExportLogs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportLogs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportLogs.ForeColor = System.Drawing.Color.White;
            this.btnExportLogs.Location = new System.Drawing.Point(20, 328);
            this.btnExportLogs.Name = "btnExportLogs";
            this.btnExportLogs.Size = new System.Drawing.Size(80, 30);
            this.btnExportLogs.TabIndex = 4;
            this.btnExportLogs.Text = "💾 Export";
            this.btnExportLogs.UseVisualStyleBackColor = false;
            this.btnExportLogs.Click += new System.EventHandler(this.BtnExportLogs_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.logsPanel);
            this.panel1.Location = new System.Drawing.Point(12, 430);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(814, 395);
            this.panel1.TabIndex = 6;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.subtitleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(826, 82);
            this.headerPanel.TabIndex = 8;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.titleLabel.Location = new System.Drawing.Point(20, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(437, 32);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "🚀 PWAMP Management Dashboard";
            // 
            // subtitleLabel
            // 
            this.subtitleLabel.AutoSize = true;
            this.subtitleLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.subtitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.subtitleLabel.Location = new System.Drawing.Point(67, 47);
            this.subtitleLabel.Name = "subtitleLabel";
            this.subtitleLabel.Size = new System.Drawing.Size(337, 19);
            this.subtitleLabel.TabIndex = 1;
            this.subtitleLabel.Text = "Monitor and control your Apache and MySQL servers";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.headerPanel);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(826, 82);
            this.panel2.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._mySqlModule);
            this.groupBox1.Controls.Add(this._apacheModule);
            this.groupBox1.Location = new System.Drawing.Point(12, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(826, 238);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // _mySqlModule
            // 
            this._mySqlModule.BackColor = System.Drawing.SystemColors.Control;
            this._mySqlModule.Location = new System.Drawing.Point(422, 27);
            this._mySqlModule.Name = "_mySqlModule";
            this._mySqlModule.Size = new System.Drawing.Size(392, 169);
            this._mySqlModule.TabIndex = 7;
            // 
            // _apacheModule
            // 
            this._apacheModule.BackColor = System.Drawing.SystemColors.Control;
            this._apacheModule.Location = new System.Drawing.Point(6, 27);
            this._apacheModule.Name = "_apacheModule";
            this._apacheModule.Size = new System.Drawing.Size(398, 169);
            this._apacheModule.TabIndex = 4;
            // 
            // grpHelpers
            // 
            this.grpHelpers.Controls.Add(this.btnStopAllServers);
            this.grpHelpers.Controls.Add(this.btnOpenExplorer);
            this.grpHelpers.Controls.Add(this.btnAbout);
            this.grpHelpers.Location = new System.Drawing.Point(12, 362);
            this.grpHelpers.Name = "grpHelpers";
            this.grpHelpers.Size = new System.Drawing.Size(814, 62);
            this.grpHelpers.TabIndex = 11;
            this.grpHelpers.TabStop = false;
            this.grpHelpers.Text = "Helpers";
            // 
            // btnStopAllServers
            // 
            this.btnStopAllServers.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnStopAllServers.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnStopAllServers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStopAllServers.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnStopAllServers.Location = new System.Drawing.Point(120, 19);
            this.btnStopAllServers.Name = "btnStopAllServers";
            this.btnStopAllServers.Size = new System.Drawing.Size(94, 37);
            this.btnStopAllServers.TabIndex = 1;
            this.btnStopAllServers.Text = "Stop All";
            this.btnStopAllServers.UseVisualStyleBackColor = false;
            this.btnStopAllServers.Click += new System.EventHandler(this.BtnStopAllServers_Click);
            // 
            // btnOpenExplorer
            // 
            this.btnOpenExplorer.BackColor = System.Drawing.Color.LightGreen;
            this.btnOpenExplorer.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnOpenExplorer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOpenExplorer.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnOpenExplorer.Location = new System.Drawing.Point(6, 19);
            this.btnOpenExplorer.Name = "btnOpenExplorer";
            this.btnOpenExplorer.Size = new System.Drawing.Size(94, 37);
            this.btnOpenExplorer.TabIndex = 0;
            this.btnOpenExplorer.Text = "Explorer";
            this.btnOpenExplorer.UseVisualStyleBackColor = false;
            this.btnOpenExplorer.Click += new System.EventHandler(this.BtnOpenExplorer_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.LightBlue;
            this.btnAbout.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAbout.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnAbout.Location = new System.Drawing.Point(234, 19);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(94, 37);
            this.btnAbout.TabIndex = 2;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.BtnAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(843, 833);
            this.Controls.Add(this.grpHelpers);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PWAMP Control Panel";
            this.logsPanel.ResumeLayout(false);
            this.logsPanel.PerformLayout();
            this.logTabControl.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.errorTab.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grpHelpers.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.ApacheControl _apacheModule;
        private Panel logsPanel;
        private Label logsHeaderLabel;
        private TabControl logTabControl;
        private TabPage outputTab;
        private RichTextBox _logTextBox;
        private TabPage errorTab;
        private RichTextBox _errorLogTextBox;
        private TabPage accessTab;
        private Button btnExportLogs;
        private Panel panel1;
        private Controls.MySqlControl _mySqlModule;
        private Panel headerPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private Panel panel2;
        private GroupBox groupBox1;
        private GroupBox grpHelpers;
        private Button btnOpenExplorer;
        private Button btnStopAllServers;
        private Button btnAbout;
    }
}
