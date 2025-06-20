namespace Frostybee.PwampAdmin.Controls
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
            this.pnlControls.BackColor = System.Drawing.Color.White;
            this.pnlControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.pnlControls.Size = new System.Drawing.Size(278, 146);
            this.pnlControls.TabIndex = 2;
            // 
            // lblServerIcon
            // 
            this.lblServerIcon.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblServerIcon.Location = new System.Drawing.Point(15, 3);
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
            this.lblServerTitle.Location = new System.Drawing.Point(51, 7);
            this.lblServerTitle.Name = "lblServerTitle";
            this.lblServerTitle.Size = new System.Drawing.Size(109, 21);
            this.lblServerTitle.TabIndex = 1;
            this.lblServerTitle.Text = "Server Name";
            // 
            // pcbServerStatus
            // 
            this.pcbServerStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.pcbServerStatus.Location = new System.Drawing.Point(195, 10);
            this.pcbServerStatus.Name = "pcbServerStatus";
            this.pcbServerStatus.Size = new System.Drawing.Size(15, 15);
            this.pcbServerStatus.TabIndex = 2;
            this.pcbServerStatus.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblStatus.Location = new System.Drawing.Point(215, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(59, 15);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "STOPPED";
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.AutoSize = true;
            this.lblServerInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblServerInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblServerInfo.Location = new System.Drawing.Point(15, 35);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Size = new System.Drawing.Size(165, 15);
            this.lblServerInfo.TabIndex = 7;
            this.lblServerInfo.Text = "Status: Stopped | Port: Not Set";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(15, 57);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(121, 35);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "▶️ Start";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(142, 57);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(121, 35);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "⏹️ Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            // 
            // btnServerAdmin
            // 
            this.btnServerAdmin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnServerAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnServerAdmin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnServerAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServerAdmin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnServerAdmin.ForeColor = System.Drawing.Color.White;
            this.btnServerAdmin.Location = new System.Drawing.Point(15, 102);
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
            this.btnTools.Location = new System.Drawing.Point(142, 102);
            this.btnTools.Name = "btnTools";
            this.btnTools.Size = new System.Drawing.Size(121, 35);
            this.btnTools.TabIndex = 8;
            this.btnTools.Text = "⚙️ Tools ▼";
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlControls);
            this.Name = "ServerControlBase";
            this.Size = new System.Drawing.Size(278, 146);
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
