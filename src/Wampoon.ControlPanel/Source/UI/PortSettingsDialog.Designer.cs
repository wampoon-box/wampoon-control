using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wampoon.ControlPanel.UI
{
    partial class PortSettingsDialog
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.portConfigGroupBox = new System.Windows.Forms.GroupBox();
            this.lblApachePort = new System.Windows.Forms.Label();
            this.nudApachePort = new System.Windows.Forms.NumericUpDown();
            this.lblApacheStatus = new System.Windows.Forms.Label();
            this.lblMySqlPort = new System.Windows.Forms.Label();
            this.nudMySqlPort = new System.Windows.Forms.NumericUpDown();
            this.lblMySqlStatus = new System.Windows.Forms.Label();
            this.chkRestartServers = new System.Windows.Forms.CheckBox();
            this.actionsGroupBox = new System.Windows.Forms.GroupBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.logGroupBox = new System.Windows.Forms.GroupBox();
            this.rtxtLog = new System.Windows.Forms.RichTextBox();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.portConfigGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudApachePort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMySqlPort)).BeginInit();
            this.actionsGroupBox.SuspendLayout();
            this.logGroupBox.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.Controls.Add(this.portConfigGroupBox);
            this.mainPanel.Controls.Add(this.actionsGroupBox);
            this.mainPanel.Controls.Add(this.logGroupBox);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(20);
            this.mainPanel.Size = new System.Drawing.Size(514, 591);
            this.mainPanel.TabIndex = 0;
            // 
            // portConfigGroupBox
            // 
            this.portConfigGroupBox.Controls.Add(this.lblApachePort);
            this.portConfigGroupBox.Controls.Add(this.nudApachePort);
            this.portConfigGroupBox.Controls.Add(this.lblApacheStatus);
            this.portConfigGroupBox.Controls.Add(this.lblMySqlPort);
            this.portConfigGroupBox.Controls.Add(this.nudMySqlPort);
            this.portConfigGroupBox.Controls.Add(this.lblMySqlStatus);
            this.portConfigGroupBox.Controls.Add(this.chkRestartServers);
            this.portConfigGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.portConfigGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.portConfigGroupBox.Location = new System.Drawing.Point(20, 20);
            this.portConfigGroupBox.Name = "portConfigGroupBox";
            this.portConfigGroupBox.Padding = new System.Windows.Forms.Padding(15);
            this.portConfigGroupBox.Size = new System.Drawing.Size(457, 160);
            this.portConfigGroupBox.TabIndex = 0;
            this.portConfigGroupBox.TabStop = false;
            this.portConfigGroupBox.Text = "⚙️ Port Configuration";
            // 
            // lblApachePort
            // 
            this.lblApachePort.AutoSize = true;
            this.lblApachePort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblApachePort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblApachePort.Location = new System.Drawing.Point(15, 30);
            this.lblApachePort.Name = "lblApachePort";
            this.lblApachePort.Size = new System.Drawing.Size(106, 15);
            this.lblApachePort.TabIndex = 0;
            this.lblApachePort.Text = "Apache HTTP Port:";
            // 
            // nudApachePort
            // 
            this.nudApachePort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudApachePort.Location = new System.Drawing.Point(150, 27);
            this.nudApachePort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudApachePort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudApachePort.Name = "nudApachePort";
            this.nudApachePort.Size = new System.Drawing.Size(80, 23);
            this.nudApachePort.TabIndex = 1;
            this.nudApachePort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.nudApachePort.ValueChanged += new System.EventHandler(this.NudApachePort_ValueChanged);
            // 
            // lblApacheStatus
            // 
            this.lblApacheStatus.AutoSize = true;
            this.lblApacheStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblApacheStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblApacheStatus.Location = new System.Drawing.Point(240, 30);
            this.lblApacheStatus.Name = "lblApacheStatus";
            this.lblApacheStatus.Size = new System.Drawing.Size(66, 15);
            this.lblApacheStatus.TabIndex = 2;
            this.lblApacheStatus.Text = "Checking...";
            // 
            // lblMySqlPort
            // 
            this.lblMySqlPort.AutoSize = true;
            this.lblMySqlPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMySqlPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblMySqlPort.Location = new System.Drawing.Point(15, 65);
            this.lblMySqlPort.Name = "lblMySqlPort";
            this.lblMySqlPort.Size = new System.Drawing.Size(123, 15);
            this.lblMySqlPort.TabIndex = 3;
            this.lblMySqlPort.Text = "MySQL/MariaDB Port:";
            // 
            // nudMySqlPort
            // 
            this.nudMySqlPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudMySqlPort.Location = new System.Drawing.Point(150, 62);
            this.nudMySqlPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudMySqlPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMySqlPort.Name = "nudMySqlPort";
            this.nudMySqlPort.Size = new System.Drawing.Size(80, 23);
            this.nudMySqlPort.TabIndex = 4;
            this.nudMySqlPort.Value = new decimal(new int[] {
            3306,
            0,
            0,
            0});
            this.nudMySqlPort.ValueChanged += new System.EventHandler(this.NudMySqlPort_ValueChanged);
            // 
            // lblMySqlStatus
            // 
            this.lblMySqlStatus.AutoSize = true;
            this.lblMySqlStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMySqlStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblMySqlStatus.Location = new System.Drawing.Point(240, 65);
            this.lblMySqlStatus.Name = "lblMySqlStatus";
            this.lblMySqlStatus.Size = new System.Drawing.Size(66, 15);
            this.lblMySqlStatus.TabIndex = 5;
            this.lblMySqlStatus.Text = "Checking...";
            // 
            // chkRestartServers
            // 
            this.chkRestartServers.AutoSize = true;
            this.chkRestartServers.Checked = true;
            this.chkRestartServers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRestartServers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkRestartServers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chkRestartServers.Location = new System.Drawing.Point(15, 100);
            this.chkRestartServers.Name = "chkRestartServers";
            this.chkRestartServers.Size = new System.Drawing.Size(318, 19);
            this.chkRestartServers.TabIndex = 6;
            this.chkRestartServers.Text = "Show reminder to restart servers after applying changes";
            this.chkRestartServers.UseVisualStyleBackColor = true;
            // 
            // actionsGroupBox
            // 
            this.actionsGroupBox.Controls.Add(this.btnValidate);
            this.actionsGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.actionsGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.actionsGroupBox.Location = new System.Drawing.Point(20, 190);
            this.actionsGroupBox.Name = "actionsGroupBox";
            this.actionsGroupBox.Padding = new System.Windows.Forms.Padding(15);
            this.actionsGroupBox.Size = new System.Drawing.Size(457, 70);
            this.actionsGroupBox.TabIndex = 1;
            this.actionsGroupBox.TabStop = false;
            this.actionsGroupBox.Text = "🔍 Actions";
            // 
            // btnValidate
            // 
            this.btnValidate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnValidate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnValidate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnValidate.ForeColor = System.Drawing.Color.White;
            this.btnValidate.Location = new System.Drawing.Point(15, 25);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(120, 30);
            this.btnValidate.TabIndex = 0;
            this.btnValidate.Text = "🔍 Validate Ports";
            this.btnValidate.UseVisualStyleBackColor = false;
            this.btnValidate.Click += new System.EventHandler(this.BtnValidate_Click);
            // 
            // logGroupBox
            // 
            this.logGroupBox.Controls.Add(this.rtxtLog);
            this.logGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.logGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.logGroupBox.Location = new System.Drawing.Point(20, 270);
            this.logGroupBox.Name = "logGroupBox";
            this.logGroupBox.Padding = new System.Windows.Forms.Padding(15);
            this.logGroupBox.Size = new System.Drawing.Size(457, 260);
            this.logGroupBox.TabIndex = 2;
            this.logGroupBox.TabStop = false;
            this.logGroupBox.Text = "📋 Validation Log";
            // 
            // rtxtLog
            // 
            this.rtxtLog.BackColor = System.Drawing.Color.Black;
            this.rtxtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.rtxtLog.ForeColor = System.Drawing.Color.White;
            this.rtxtLog.Location = new System.Drawing.Point(15, 25);
            this.rtxtLog.Name = "rtxtLog";
            this.rtxtLog.ReadOnly = true;
            this.rtxtLog.Size = new System.Drawing.Size(427, 220);
            this.rtxtLog.TabIndex = 0;
            this.rtxtLog.Text = "";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.btnOK);
            this.buttonPanel.Controls.Add(this.btnCancel);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 540);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(514, 51);
            this.buttonPanel.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(340, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Apply Changes";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(73)))), ((int)(((byte)(83)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(450, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // PortSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(514, 591);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PortSettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Port Configuration";
            this.mainPanel.ResumeLayout(false);
            this.portConfigGroupBox.ResumeLayout(false);
            this.portConfigGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudApachePort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMySqlPort)).EndInit();
            this.actionsGroupBox.ResumeLayout(false);
            this.logGroupBox.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.GroupBox portConfigGroupBox;
        private System.Windows.Forms.Label lblApachePort;
        private System.Windows.Forms.NumericUpDown nudApachePort;
        private System.Windows.Forms.Label lblApacheStatus;
        private System.Windows.Forms.Label lblMySqlPort;
        private System.Windows.Forms.NumericUpDown nudMySqlPort;
        private System.Windows.Forms.Label lblMySqlStatus;
        private System.Windows.Forms.CheckBox chkRestartServers;
        private System.Windows.Forms.GroupBox actionsGroupBox;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.GroupBox logGroupBox;
        private System.Windows.Forms.RichTextBox rtxtLog;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}