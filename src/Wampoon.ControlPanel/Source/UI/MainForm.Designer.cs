﻿using System.Drawing;
using System.Windows.Forms;

namespace Wampoon.ControlPanel.UI
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
            this.logsGroupBox = new System.Windows.Forms.GroupBox();
            this.logTabControl = new System.Windows.Forms.TabControl();
            this.outputTab = new System.Windows.Forms.TabPage();
            this._rtxtActionsLog = new System.Windows.Forms.RichTextBox();
            this.errorTab = new System.Windows.Forms.TabPage();
            this._rtxtErrorLog = new System.Windows.Forms.RichTextBox();
            this.accessTab = new System.Windows.Forms.TabPage();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.bannerIcon = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpHelpers = new System.Windows.Forms.GroupBox();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnStartAllServers = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnStopAllServers = new System.Windows.Forms.Button();
            this.btnOpenExplorer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._apacheServerModule = new Wampoon.ControlPanel.Controls.ApacheControl();
            this._mySqlServerModule = new Wampoon.ControlPanel.Controls.MySqlControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logsGroupBox.SuspendLayout();
            this.logTabControl.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.errorTab.SuspendLayout();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bannerIcon)).BeginInit();
            this.panel2.SuspendLayout();
            this.grpHelpers.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // logsGroupBox
            // 
            this.logsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logsGroupBox.Controls.Add(this.logTabControl);
            this.logsGroupBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.logsGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.logsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.logsGroupBox.Name = "logsGroupBox";
            this.logsGroupBox.Size = new System.Drawing.Size(736, 287);
            this.logsGroupBox.TabIndex = 0;
            this.logsGroupBox.TabStop = false;
            this.logsGroupBox.Text = "📋 Server Logs";
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
            this.logTabControl.Location = new System.Drawing.Point(6, 24);
            this.logTabControl.Name = "logTabControl";
            this.logTabControl.SelectedIndex = 0;
            this.logTabControl.Size = new System.Drawing.Size(724, 257);
            this.logTabControl.TabIndex = 1;
            // 
            // outputTab
            // 
            this.outputTab.BackColor = System.Drawing.Color.White;
            this.outputTab.Controls.Add(this._rtxtActionsLog);
            this.outputTab.Location = new System.Drawing.Point(4, 24);
            this.outputTab.Name = "outputTab";
            this.outputTab.Padding = new System.Windows.Forms.Padding(3);
            this.outputTab.Size = new System.Drawing.Size(716, 229);
            this.outputTab.TabIndex = 0;
            this.outputTab.Text = "Output Logs";
            // 
            // _rtxtActionsLog
            // 
            this._rtxtActionsLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this._rtxtActionsLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtxtActionsLog.Font = new System.Drawing.Font("Cascadia Code", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._rtxtActionsLog.ForeColor = System.Drawing.Color.White;
            this._rtxtActionsLog.Location = new System.Drawing.Point(3, 3);
            this._rtxtActionsLog.Name = "_rtxtActionsLog";
            this._rtxtActionsLog.ReadOnly = true;
            this._rtxtActionsLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._rtxtActionsLog.Size = new System.Drawing.Size(710, 223);
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
            this.errorTab.Size = new System.Drawing.Size(707, 232);
            this.errorTab.TabIndex = 1;
            this.errorTab.Text = "MariaDB Logs";
            this.errorTab.UseVisualStyleBackColor = true;
            // 
            // _rtxtErrorLog
            // 
            this._rtxtErrorLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this._rtxtErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtxtErrorLog.Font = new System.Drawing.Font("Cascadia Code", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._rtxtErrorLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this._rtxtErrorLog.Location = new System.Drawing.Point(3, 3);
            this._rtxtErrorLog.Name = "_rtxtErrorLog";
            this._rtxtErrorLog.ReadOnly = true;
            this._rtxtErrorLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._rtxtErrorLog.Size = new System.Drawing.Size(701, 226);
            this._rtxtErrorLog.TabIndex = 0;
            this._rtxtErrorLog.Text = "";
            // 
            // accessTab
            // 
            this.accessTab.Location = new System.Drawing.Point(4, 24);
            this.accessTab.Name = "accessTab";
            this.accessTab.Padding = new System.Windows.Forms.Padding(3);
            this.accessTab.Size = new System.Drawing.Size(707, 232);
            this.accessTab.TabIndex = 2;
            this.accessTab.Text = "Access Logs";
            this.accessTab.UseVisualStyleBackColor = true;
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.subtitleLabel);
            this.headerPanel.Controls.Add(this.bannerIcon);
            this.headerPanel.Location = new System.Drawing.Point(0, 1);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1019, 71);
            this.headerPanel.TabIndex = 8;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(95, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(298, 32);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "WAMPoon Control Panel";
            // 
            // subtitleLabel
            // 
            this.subtitleLabel.AutoSize = true;
            this.subtitleLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.subtitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.subtitleLabel.Location = new System.Drawing.Point(97, 42);
            this.subtitleLabel.Name = "subtitleLabel";
            this.subtitleLabel.Size = new System.Drawing.Size(345, 19);
            this.subtitleLabel.TabIndex = 1;
            this.subtitleLabel.Text = "Monitor and control your Apache and MariaDB servers";
            // 
            // bannerIcon
            // 
            this.bannerIcon.BackColor = System.Drawing.Color.Transparent;
            this.bannerIcon.Location = new System.Drawing.Point(37, 11);
            this.bannerIcon.Name = "bannerIcon";
            this.bannerIcon.Size = new System.Drawing.Size(40, 40);
            this.bannerIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.bannerIcon.TabIndex = 2;
            this.bannerIcon.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.grpHelpers);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(12, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(740, 326);
            this.panel2.TabIndex = 9;
            // 
            // grpHelpers
            // 
            this.grpHelpers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpHelpers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(234)))), ((int)(((byte)(238)))));
            this.grpHelpers.Controls.Add(this.btnQuit);
            this.grpHelpers.Controls.Add(this.btnStartAllServers);
            this.grpHelpers.Controls.Add(this.btnAbout);
            this.grpHelpers.Controls.Add(this.btnStopAllServers);
            this.grpHelpers.Controls.Add(this.btnOpenExplorer);
            this.grpHelpers.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpHelpers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.grpHelpers.Location = new System.Drawing.Point(10, 241);
            this.grpHelpers.Name = "grpHelpers";
            this.grpHelpers.Padding = new System.Windows.Forms.Padding(15);
            this.grpHelpers.Size = new System.Drawing.Size(730, 76);
            this.grpHelpers.TabIndex = 11;
            this.grpHelpers.TabStop = false;
            this.grpHelpers.Text = "⚡ Quick Actions";
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(73)))), ((int)(((byte)(73)))));
            this.btnQuit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuit.ForeColor = System.Drawing.Color.White;
            this.btnQuit.Location = new System.Drawing.Point(581, 27);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(94, 37);
            this.btnQuit.TabIndex = 4;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // btnStartAllServers
            // 
            this.btnStartAllServers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(150)))), ((int)(((byte)(85)))));
            this.btnStartAllServers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartAllServers.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStartAllServers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartAllServers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartAllServers.ForeColor = System.Drawing.Color.White;
            this.btnStartAllServers.Location = new System.Drawing.Point(213, 27);
            this.btnStartAllServers.Name = "btnStartAllServers";
            this.btnStartAllServers.Size = new System.Drawing.Size(94, 37);
            this.btnStartAllServers.TabIndex = 1;
            this.btnStartAllServers.Text = "Start All";
            this.btnStartAllServers.UseVisualStyleBackColor = false;
            this.btnStartAllServers.Click += new System.EventHandler(this.BtnStartAllServers_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAbout.ForeColor = System.Drawing.Color.White;
            this.btnAbout.Location = new System.Drawing.Point(481, 27);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(94, 37);
            this.btnAbout.TabIndex = 3;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.BtnAbout_Click);
            // 
            // btnStopAllServers
            // 
            this.btnStopAllServers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(73)))), ((int)(((byte)(73)))));
            this.btnStopAllServers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopAllServers.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStopAllServers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopAllServers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStopAllServers.ForeColor = System.Drawing.Color.White;
            this.btnStopAllServers.Location = new System.Drawing.Point(317, 27);
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
            this.btnOpenExplorer.Location = new System.Drawing.Point(31, 27);
            this.btnOpenExplorer.Name = "btnOpenExplorer";
            this.btnOpenExplorer.Size = new System.Drawing.Size(116, 37);
            this.btnOpenExplorer.TabIndex = 0;
            this.btnOpenExplorer.Text = "Open Web Root";
            this.btnOpenExplorer.UseVisualStyleBackColor = false;
            this.btnOpenExplorer.Click += new System.EventHandler(this.BtnOpenExplorer_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(234)))), ((int)(((byte)(238)))));
            this.groupBox1.Controls.Add(this._apacheServerModule);
            this.groupBox1.Controls.Add(this._mySqlServerModule);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.groupBox1.Location = new System.Drawing.Point(10, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(15);
            this.groupBox1.Size = new System.Drawing.Size(730, 219);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "🖥️ Server Management";
            // 
            // _apacheServerModule
            // 
            this._apacheServerModule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._apacheServerModule.Location = new System.Drawing.Point(10, 27);
            this._apacheServerModule.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._apacheServerModule.Name = "_apacheServerModule";
            this._apacheServerModule.Size = new System.Drawing.Size(335, 178);
            this._apacheServerModule.TabIndex = 4;
            // 
            // _mySqlServerModule
            // 
            this._mySqlServerModule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._mySqlServerModule.Location = new System.Drawing.Point(374, 27);
            this._mySqlServerModule.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._mySqlServerModule.Name = "_mySqlServerModule";
            this._mySqlServerModule.Size = new System.Drawing.Size(335, 178);
            this._mySqlServerModule.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.logsGroupBox);
            this.panel1.Location = new System.Drawing.Point(13, 417);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(985, 416);
            this.panel1.TabIndex = 10;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(755, 719);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.panel2);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WAMPoon Control Panel";
            this.logsGroupBox.ResumeLayout(false);
            this.logTabControl.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.errorTab.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bannerIcon)).EndInit();
            this.panel2.ResumeLayout(false);
            this.grpHelpers.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.ApacheControl _apacheServerModule;
        private GroupBox logsGroupBox;
        private TabControl logTabControl;
        private TabPage outputTab;
        private RichTextBox _rtxtActionsLog;
        private TabPage errorTab;
        private RichTextBox _rtxtErrorLog;
        private TabPage accessTab;
        private Controls.MySqlControl _mySqlServerModule;
        private Panel headerPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private PictureBox bannerIcon;
        private Panel panel2;
        private GroupBox groupBox1;
        private GroupBox grpHelpers;
        private Button btnOpenExplorer;
        private Button btnStartAllServers;
        private Button btnStopAllServers;
        private Button btnAbout;
        private Button btnQuit;
        private Panel panel1;
    }
}
