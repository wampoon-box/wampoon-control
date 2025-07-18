﻿namespace Wampoon.ControlPanel.Controls
{
    partial class ServerControlBase
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.lblServerIcon = new System.Windows.Forms.Label();
            this.lblServerTitle = new System.Windows.Forms.Label();
            this.pcbServerStatus = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblServerInfo = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnServerAdmin = new System.Windows.Forms.Button();
            this.btnTools = new System.Windows.Forms.Button();
            this.contextMenuTools = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbServerStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.pnlControls.Controls.Add(this.lblServerIcon);
            this.pnlControls.Controls.Add(this.lblServerTitle);
            this.pnlControls.Controls.Add(this.pcbServerStatus);
            this.pnlControls.Controls.Add(this.lblStatus);
            this.pnlControls.Controls.Add(this.lblServerInfo);
            this.pnlControls.Controls.Add(this.btnStart);
            this.pnlControls.Controls.Add(this.btnStop);
            this.pnlControls.Controls.Add(this.btnServerAdmin);
            this.pnlControls.Controls.Add(this.btnTools);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControls.Location = new System.Drawing.Point(0, 0);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Padding = new System.Windows.Forms.Padding(15);
            this.pnlControls.Size = new System.Drawing.Size(311, 168);
            this.pnlControls.TabIndex = 2;
            // 
            // lblServerIcon
            // 
            this.lblServerIcon.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblServerIcon.Location = new System.Drawing.Point(12, 7);
            this.lblServerIcon.Name = "lblServerIcon";
            this.lblServerIcon.Size = new System.Drawing.Size(30, 30);
            this.lblServerIcon.TabIndex = 0;
            this.lblServerIcon.Text = "🌐";
            this.lblServerIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblServerTitle
            // 
            this.lblServerTitle.AutoSize = true;
            this.lblServerTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblServerTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblServerTitle.Location = new System.Drawing.Point(38, 11);
            this.lblServerTitle.Name = "lblServerTitle";
            this.lblServerTitle.Size = new System.Drawing.Size(109, 21);
            this.lblServerTitle.TabIndex = 1;
            this.lblServerTitle.Text = "Server Name";
            // 
            // pcbServerStatus
            // 
            this.pcbServerStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.pcbServerStatus.Location = new System.Drawing.Point(207, 11);
            this.pcbServerStatus.Name = "pcbServerStatus";
            this.pcbServerStatus.Size = new System.Drawing.Size(27, 22);
            this.pcbServerStatus.TabIndex = 2;
            this.pcbServerStatus.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblStatus.Location = new System.Drawing.Point(241, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 24);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "STOPPED";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.AutoSize = true;
            this.lblServerInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblServerInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblServerInfo.Location = new System.Drawing.Point(26, 44);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Size = new System.Drawing.Size(133, 17);
            this.lblServerInfo.TabIndex = 7;
            this.lblServerInfo.Text = "Port: Not Set | PID: ... ";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(150)))), ((int)(((byte)(85)))));
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(26, 70);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(121, 35);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "▶️ Start";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(73)))), ((int)(((byte)(73)))));
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(168, 70);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(121, 35);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "⏹️ Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            // 
            // btnServerAdmin
            // 
            this.btnServerAdmin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(120)))), ((int)(((byte)(249)))));
            this.btnServerAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnServerAdmin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnServerAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServerAdmin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnServerAdmin.ForeColor = System.Drawing.Color.White;
            this.btnServerAdmin.Location = new System.Drawing.Point(26, 115);
            this.btnServerAdmin.Name = "btnServerAdmin";
            this.btnServerAdmin.Size = new System.Drawing.Size(121, 35);
            this.btnServerAdmin.TabIndex = 6;
            this.btnServerAdmin.Text = "🔧 Admin";
            this.btnServerAdmin.UseVisualStyleBackColor = false;
            // 
            // btnTools
            // 
            this.btnTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnTools.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTools.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnTools.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTools.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTools.ForeColor = System.Drawing.Color.White;
            this.btnTools.Location = new System.Drawing.Point(168, 115);
            this.btnTools.Name = "btnTools";
            this.btnTools.Size = new System.Drawing.Size(121, 35);
            this.btnTools.TabIndex = 8;
            this.btnTools.Text = "⚙️ Configs ▼";
            this.btnTools.UseVisualStyleBackColor = false;
            // 
            // contextMenuTools
            // 
            this.contextMenuTools.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuTools.Name = "contextMenuTools";
            this.contextMenuTools.Size = new System.Drawing.Size(61, 4);
            // 
            // ServerControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlControls);
            this.Name = "ServerControlBase";
            this.Size = new System.Drawing.Size(311, 168);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbServerStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControls;
        protected System.Windows.Forms.Label lblServerIcon;
        protected System.Windows.Forms.Label lblServerTitle;
        protected System.Windows.Forms.PictureBox pcbServerStatus;
        protected System.Windows.Forms.Label lblStatus;
        protected System.Windows.Forms.Label lblServerInfo;
        protected System.Windows.Forms.Button btnStart;
        protected System.Windows.Forms.Button btnStop;
        protected System.Windows.Forms.Button btnServerAdmin;
        protected System.Windows.Forms.Button btnTools;
        protected System.Windows.Forms.ContextMenuStrip contextMenuTools;
    }
}
