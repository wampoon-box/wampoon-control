namespace Pwamp.Admin.Controls
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
            this.pnlControls = new System.Windows.Forms.Panel();
            this.lblServerIcon = new System.Windows.Forms.Label();
            this.lblServerTitle = new System.Windows.Forms.Label();
            this.pcbServerStatus = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbServerStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.White;
            this.pnlControls.Controls.Add(this.lblServerIcon);
            this.pnlControls.Controls.Add(this.lblServerTitle);
            this.pnlControls.Controls.Add(this.pcbServerStatus);
            this.pnlControls.Controls.Add(this.lblStatus);
            this.pnlControls.Controls.Add(this.btnStart);
            this.pnlControls.Controls.Add(this.btnStop);
            this.pnlControls.Controls.Add(this.btnRestart);
            this.pnlControls.Location = new System.Drawing.Point(3, 3);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(356, 139);
            this.pnlControls.TabIndex = 2;
            // 
            // lblServerIcon
            // 
            this.lblServerIcon.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.lblServerIcon.Location = new System.Drawing.Point(12, 20);
            this.lblServerIcon.Name = "lblServerIcon";
            this.lblServerIcon.Size = new System.Drawing.Size(50, 50);
            this.lblServerIcon.TabIndex = 0;
            this.lblServerIcon.Text = "🌐";
            this.lblServerIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblServerTitle
            // 
            this.lblServerTitle.AutoSize = true;
            this.lblServerTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblServerTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblServerTitle.Location = new System.Drawing.Point(68, 10);
            this.lblServerTitle.Name = "lblServerTitle";
            this.lblServerTitle.Size = new System.Drawing.Size(140, 25);
            this.lblServerTitle.TabIndex = 1;
            this.lblServerTitle.Text = "Apache Server";
            // 
            // pcbServerStatus
            // 
            this.pcbServerStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.pcbServerStatus.Location = new System.Drawing.Point(70, 50);
            this.pcbServerStatus.Name = "pcbServerStatus";
            this.pcbServerStatus.Size = new System.Drawing.Size(20, 20);
            this.pcbServerStatus.TabIndex = 2;
            this.pcbServerStatus.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblStatus.Location = new System.Drawing.Point(97, 50);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(66, 19);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Stopped";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(20, 90);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 35);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "▶️ Start";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(130, 90);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 35);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "⏹️ Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.btnRestart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestart.ForeColor = System.Drawing.Color.White;
            this.btnRestart.Location = new System.Drawing.Point(240, 90);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(100, 35);
            this.btnRestart.TabIndex = 6;
            this.btnRestart.Text = "🔄 Restart";
            this.btnRestart.UseVisualStyleBackColor = false;
            // 
            // ServerControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlControls);
            this.Name = "ServerControlBase";
            this.Size = new System.Drawing.Size(368, 159);
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
        protected System.Windows.Forms.Button btnStart;
        protected System.Windows.Forms.Button btnStop;
        protected System.Windows.Forms.Button btnRestart;
    }
}
