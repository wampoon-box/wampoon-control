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
            this._rtxtActionsLog = new System.Windows.Forms.RichTextBox();
            this.errorTab = new System.Windows.Forms.TabPage();
            this._rtxtErrorLog = new System.Windows.Forms.RichTextBox();
            this.accessTab = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._mySqlModule = new Frostybee.PwampAdmin.Controls.MySqlControl();
            this._apacheModule = new Frostybee.PwampAdmin.Controls.ApacheControl();
            this.grpHelpers = new System.Windows.Forms.GroupBox();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnStartAllServers = new System.Windows.Forms.Button();
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
            this.logsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsPanel.Location = new System.Drawing.Point(0, 0);
            this.logsPanel.Name = "logsPanel";
            this.logsPanel.Size = new System.Drawing.Size(696, 395);
            this.logsPanel.TabIndex = 5;
            // 
            // logsHeaderLabel
            // 
            this.logsHeaderLabel.AutoSize = true;
            this.logsHeaderLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.logsHeaderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.logsHeaderLabel.Location = new System.Drawing.Point(20, 15);
            this.logsHeaderLabel.Name = "logsHeaderLabel";
            this.logsHeaderLabel.Size = new System.Drawing.Size(140, 25);
            this.logsHeaderLabel.TabIndex = 0;
            this.logsHeaderLabel.Text = "📋 Server Logs";
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
            this.logTabControl.Size = new System.Drawing.Size(673, 272);
            this.logTabControl.TabIndex = 1;
            // 
            // outputTab
            // 
            this.outputTab.BackColor = System.Drawing.Color.White;
            this.outputTab.Controls.Add(this._rtxtActionsLog);
            this.outputTab.Location = new System.Drawing.Point(4, 24);
            this.outputTab.Name = "outputTab";
            this.outputTab.Padding = new System.Windows.Forms.Padding(3);
            this.outputTab.Size = new System.Drawing.Size(665, 244);
            this.outputTab.TabIndex = 0;
            this.outputTab.Text = "Output Logs";
            // 
            // _rtxtActionsLog
            // 
            this._rtxtActionsLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._rtxtActionsLog.BackColor = System.Drawing.Color.White;
            this._rtxtActionsLog.Font = new System.Drawing.Font("Consolas", 10F);
            this._rtxtActionsLog.ForeColor = System.Drawing.Color.Black;
            this._rtxtActionsLog.Location = new System.Drawing.Point(13, 0);
            this._rtxtActionsLog.Name = "_rtxtActionsLog";
            this._rtxtActionsLog.ReadOnly = true;
            this._rtxtActionsLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._rtxtActionsLog.Size = new System.Drawing.Size(659, 238);
            this._rtxtActionsLog.TabIndex = 0;
            this._rtxtActionsLog.Text = "";
            // 
            // errorTab
            // 
            this.errorTab.BackColor = System.Drawing.Color.White;
            this.errorTab.Controls.Add(this._rtxtErrorLog);
            this.errorTab.Location = new System.Drawing.Point(4, 24);
            this.errorTab.Name = "errorTab";
            this.errorTab.Padding = new System.Windows.Forms.Padding(3);
            this.errorTab.Size = new System.Drawing.Size(783, 244);
            this.errorTab.TabIndex = 1;
            this.errorTab.Text = "MySQL Logs";
            this.errorTab.UseVisualStyleBackColor = true;
            // 
            // _rtxtErrorLog
            // 
            this._rtxtErrorLog.BackColor = System.Drawing.Color.White;
            this._rtxtErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtxtErrorLog.Font = new System.Drawing.Font("Consolas", 10F);
            this._rtxtErrorLog.ForeColor = System.Drawing.Color.Red;
            this._rtxtErrorLog.Location = new System.Drawing.Point(3, 3);
            this._rtxtErrorLog.Name = "_rtxtErrorLog";
            this._rtxtErrorLog.ReadOnly = true;
            this._rtxtErrorLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._rtxtErrorLog.Size = new System.Drawing.Size(777, 238);
            this._rtxtErrorLog.TabIndex = 0;
            this._rtxtErrorLog.Text = "";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.logsPanel);
            this.panel1.Location = new System.Drawing.Point(12, 430);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(696, 395);
            this.panel1.TabIndex = 6;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.subtitleLabel);
            this.headerPanel.Location = new System.Drawing.Point(12, 12);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(620, 71);
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
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(722, 833);
            this.panel2.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._mySqlModule);
            this.groupBox1.Controls.Add(this._apacheModule);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.groupBox1.Location = new System.Drawing.Point(12, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(15);
            this.groupBox1.Size = new System.Drawing.Size(696, 238);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "🖥️ Server Management";
            // 
            // _mySqlModule
            // 
            this._mySqlModule.BackColor = System.Drawing.Color.White;
            this._mySqlModule.Location = new System.Drawing.Point(352, 28);
            this._mySqlModule.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._mySqlModule.Name = "_mySqlModule";
            this._mySqlModule.Size = new System.Drawing.Size(326, 168);
            this._mySqlModule.TabIndex = 7;
            // 
            // _apacheModule
            // 
            this._apacheModule.BackColor = System.Drawing.Color.White;
            this._apacheModule.Location = new System.Drawing.Point(14, 27);
            this._apacheModule.Name = "_apacheModule";
            this._apacheModule.Size = new System.Drawing.Size(322, 169);
            this._apacheModule.TabIndex = 4;
            // 
            // grpHelpers
            // 
            this.grpHelpers.Controls.Add(this.btnQuit);
            this.grpHelpers.Controls.Add(this.btnStartAllServers);
            this.grpHelpers.Controls.Add(this.btnAbout);
            this.grpHelpers.Controls.Add(this.btnStopAllServers);
            this.grpHelpers.Controls.Add(this.btnOpenExplorer);
            this.grpHelpers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpHelpers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.grpHelpers.Location = new System.Drawing.Point(12, 362);
            this.grpHelpers.Name = "grpHelpers";
            this.grpHelpers.Padding = new System.Windows.Forms.Padding(15);
            this.grpHelpers.Size = new System.Drawing.Size(696, 62);
            this.grpHelpers.TabIndex = 11;
            this.grpHelpers.TabStop = false;
            this.grpHelpers.Text = "⚡ Quick Actions";
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnQuit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuit.ForeColor = System.Drawing.Color.White;
            this.btnQuit.Location = new System.Drawing.Point(584, 19);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(94, 37);
            this.btnQuit.TabIndex = 4;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // btnStartAllServers
            // 
            this.btnStartAllServers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStartAllServers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartAllServers.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStartAllServers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartAllServers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartAllServers.ForeColor = System.Drawing.Color.White;
            this.btnStartAllServers.Location = new System.Drawing.Point(149, 19);
            this.btnStartAllServers.Name = "btnStartAllServers";
            this.btnStartAllServers.Size = new System.Drawing.Size(94, 37);
            this.btnStartAllServers.TabIndex = 1;
            this.btnStartAllServers.Text = "Start All";
            this.btnStartAllServers.UseVisualStyleBackColor = false;
            this.btnStartAllServers.Click += new System.EventHandler(this.BtnStartAllServers_Click);
            // 
            // btnStopAllServers
            // 
            this.btnStopAllServers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStopAllServers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopAllServers.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStopAllServers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopAllServers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStopAllServers.ForeColor = System.Drawing.Color.White;
            this.btnStopAllServers.Location = new System.Drawing.Point(249, 19);
            this.btnStopAllServers.Name = "btnStopAllServers";
            this.btnStopAllServers.Size = new System.Drawing.Size(94, 37);
            this.btnStopAllServers.TabIndex = 2;
            this.btnStopAllServers.Text = "Stop All";
            this.btnStopAllServers.UseVisualStyleBackColor = false;
            this.btnStopAllServers.Click += new System.EventHandler(this.BtnStopAllServers_Click);
            // 
            // btnOpenExplorer
            // 
            this.btnOpenExplorer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(41)))), ((int)(((byte)(60)))));
            this.btnOpenExplorer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenExplorer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(41)))), ((int)(((byte)(60)))));
            this.btnOpenExplorer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenExplorer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOpenExplorer.ForeColor = System.Drawing.Color.White;
            this.btnOpenExplorer.Location = new System.Drawing.Point(18, 19);
            this.btnOpenExplorer.Name = "btnOpenExplorer";
            this.btnOpenExplorer.Size = new System.Drawing.Size(94, 37);
            this.btnOpenExplorer.TabIndex = 0;
            this.btnOpenExplorer.Text = "Web Root";
            this.btnOpenExplorer.UseVisualStyleBackColor = false;
            this.btnOpenExplorer.Click += new System.EventHandler(this.BtnOpenExplorer_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAbout.ForeColor = System.Drawing.Color.White;
            this.btnAbout.Location = new System.Drawing.Point(473, 19);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(94, 37);
            this.btnAbout.TabIndex = 3;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.BtnAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(722, 833);
            this.Controls.Add(this.grpHelpers);
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
        private RichTextBox _rtxtActionsLog;
        private TabPage errorTab;
        private RichTextBox _rtxtErrorLog;
        private TabPage accessTab;
        private Panel panel1;
        private Controls.MySqlControl _mySqlModule;
        private Panel headerPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private Panel panel2;
        private GroupBox groupBox1;
        private GroupBox grpHelpers;
        private Button btnOpenExplorer;
        private Button btnStartAllServers;
        private Button btnStopAllServers;
        private Button btnAbout;
        private Button btnQuit;
    }
}
