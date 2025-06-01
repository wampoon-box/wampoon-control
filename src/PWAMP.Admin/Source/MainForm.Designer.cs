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
            this.txtOutputLog = new System.Windows.Forms.TextBox();
            this.btnStopApache = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartApache
            // 
            this.btnStartApache.Location = new System.Drawing.Point(46, 65);
            this.btnStartApache.Name = "btnStartApache";
            this.btnStartApache.Size = new System.Drawing.Size(138, 23);
            this.btnStartApache.TabIndex = 0;
            this.btnStartApache.Text = "Start Apache";
            this.btnStartApache.UseVisualStyleBackColor = true;
            this.btnStartApache.Click += new System.EventHandler(this.btnStartApache_Click);
            // 
            // txtOutputLog
            // 
            this.txtOutputLog.BackColor = System.Drawing.Color.Black;
            this.txtOutputLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtOutputLog.ForeColor = System.Drawing.Color.White;
            this.txtOutputLog.Location = new System.Drawing.Point(12, 275);
            this.txtOutputLog.Multiline = true;
            this.txtOutputLog.Name = "txtOutputLog";
            this.txtOutputLog.ReadOnly = true;
            this.txtOutputLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutputLog.Size = new System.Drawing.Size(860, 380);
            this.txtOutputLog.TabIndex = 0;
            // 
            // btnStopApache
            // 
            this.btnStopApache.Location = new System.Drawing.Point(190, 65);
            this.btnStopApache.Name = "btnStopApache";
            this.btnStopApache.Size = new System.Drawing.Size(163, 23);
            this.btnStopApache.TabIndex = 1;
            this.btnStopApache.Text = "Stop";
            this.btnStopApache.UseVisualStyleBackColor = true;
            this.btnStopApache.Click += new System.EventHandler(this.btnStopApache_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 694);
            this.Controls.Add(this.btnStopApache);
            this.Controls.Add(this.txtOutputLog);
            this.Controls.Add(this.btnStartApache);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PWAMP Control Panel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Button btnStartApache;
        private TextBox txtOutputLog;
        private Button btnStopApache;
    }
}
