namespace Wampoon.ControlPanel.Controls
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
            ((System.ComponentModel.ISupportInitialize)(this.pcbServerStatus)).BeginInit();
            this.SuspendLayout();
            //
            // lblServerIcon
            //
            this.lblServerIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblServerIcon.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblServerIcon.Location = new System.Drawing.Point(14, 9);
            this.lblServerIcon.Name = "lblServerIcon";
            this.lblServerIcon.Size = new System.Drawing.Size(30, 30);
            this.lblServerIcon.TabIndex = 0;
            this.lblServerIcon.Text = "🌐";
            this.lblServerIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblServerTitle
            //
            this.lblServerTitle.AutoSize = true;
            this.lblServerTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblServerTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblServerTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblServerTitle.Location = new System.Drawing.Point(46, 14);
            this.lblServerTitle.Name = "lblServerTitle";
            this.lblServerTitle.Size = new System.Drawing.Size(109, 20);
            this.lblServerTitle.TabIndex = 1;
            this.lblServerTitle.Text = "Server Name";
            //
            // pcbServerStatus
            //
            this.pcbServerStatus.BackColor = System.Drawing.Color.Transparent;
            this.pcbServerStatus.Location = new System.Drawing.Point(207, 11);
            this.pcbServerStatus.Name = "pcbServerStatus";
            this.pcbServerStatus.Size = new System.Drawing.Size(0, 0);
            this.pcbServerStatus.TabIndex = 2;
            this.pcbServerStatus.TabStop = false;
            this.pcbServerStatus.Visible = false;
            //
            // lblStatus
            // Note: This label is used for positioning only. The actual badge is drawn in Control_Paint.
            //
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Transparent;
            this.lblStatus.Location = new System.Drawing.Point(210, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(85, 22);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblServerInfo
            //
            this.lblServerInfo.AutoSize = true;
            this.lblServerInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblServerInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblServerInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblServerInfo.Location = new System.Drawing.Point(46, 38);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Size = new System.Drawing.Size(133, 15);
            this.lblServerInfo.TabIndex = 7;
            this.lblServerInfo.Text = "Port: Not Set | PID: ... ";
            //
            // btnStart
            //
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(140)))), ((int)(((byte)(75)))));
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(120)))), ((int)(((byte)(65)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(18, 72);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(130, 34);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "▶️ Start";
            this.btnStart.UseVisualStyleBackColor = false;
            //
            // btnStop
            //
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(160, 72);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(130, 34);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "⏹️ Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            //
            // btnServerAdmin
            //
            this.btnServerAdmin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(120)))), ((int)(((byte)(180)))));
            this.btnServerAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnServerAdmin.FlatAppearance.BorderSize = 0;
            this.btnServerAdmin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(100)))), ((int)(((byte)(160)))));
            this.btnServerAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServerAdmin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnServerAdmin.ForeColor = System.Drawing.Color.White;
            this.btnServerAdmin.Location = new System.Drawing.Point(18, 114);
            this.btnServerAdmin.Name = "btnServerAdmin";
            this.btnServerAdmin.Size = new System.Drawing.Size(130, 34);
            this.btnServerAdmin.TabIndex = 6;
            this.btnServerAdmin.Text = "🔧 Admin";
            this.btnServerAdmin.UseVisualStyleBackColor = false;
            //
            // btnTools
            //
            this.btnTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnTools.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTools.FlatAppearance.BorderSize = 0;
            this.btnTools.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnTools.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTools.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTools.ForeColor = System.Drawing.Color.White;
            this.btnTools.Location = new System.Drawing.Point(160, 114);
            this.btnTools.Name = "btnTools";
            this.btnTools.Size = new System.Drawing.Size(130, 34);
            this.btnTools.TabIndex = 8;
            this.btnTools.Text = "⚙️ Configs/Logs ▼";
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
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(234)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.lblServerIcon);
            this.Controls.Add(this.lblServerTitle);
            this.Controls.Add(this.pcbServerStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblServerInfo);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnServerAdmin);
            this.Controls.Add(this.btnTools);
            this.DoubleBuffered = true;
            this.Name = "ServerControlBase";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.Size = new System.Drawing.Size(311, 160);
            ((System.ComponentModel.ISupportInitialize)(this.pcbServerStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
