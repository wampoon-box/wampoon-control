using System.Drawing;
using System.Windows.Forms;

namespace PwampControl
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "PWAMP Control Panel";
            this.FormClosing += MainForm_FormClosing;

            // Apache GroupBox
            apacheGroupBox = new GroupBox { Location = new Point(15, 15), Size = new Size(370, 100), Text = "Apache" };
            apacheTitleLabel = new Label { Location = new Point(15, 30), Size = new Size(60, 20), Text = "Status:", TextAlign = ContentAlignment.MiddleLeft };
            apacheStatusLabel = new Label { Location = new Point(85, 30), Size = new Size(100, 20), Text = "Stopped", Font = new Font(this.Font, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleLeft };
            startApacheButton = new Button { Location = new Point(195, 25), Size = new Size(80, 30), Text = "Start" };
            stopApacheButton = new Button { Location = new Point(280, 25), Size = new Size(80, 30), Text = "Stop", Enabled = false };

            startApacheButton.Click += StartApacheButton_Click;
            stopApacheButton.Click += StopApacheButton_Click;

            apacheGroupBox.Controls.AddRange(new Control[] { apacheTitleLabel, apacheStatusLabel, startApacheButton, stopApacheButton });
            this.Controls.Add(apacheGroupBox);

            // MySQL GroupBox
            mysqlGroupBox = new GroupBox { Location = new Point(15, 125), Size = new Size(370, 100), Text = "MySQL / MariaDB" };
            mysqlTitleLabel = new Label { Location = new Point(15, 30), Size = new Size(60, 20), Text = "Status:", TextAlign = ContentAlignment.MiddleLeft };
            mysqlStatusLabel = new Label { Location = new Point(85, 30), Size = new Size(100, 20), Text = "Stopped", Font = new Font(this.Font, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleLeft };
            startMysqlButton = new Button { Location = new Point(195, 25), Size = new Size(80, 30), Text = "Start" };
            stopMysqlButton = new Button { Location = new Point(280, 25), Size = new Size(80, 30), Text = "Stop", Enabled = false };

            startMysqlButton.Click += StartMysqlButton_Click;
            stopMysqlButton.Click += StopMysqlButton_Click;

            mysqlGroupBox.Controls.AddRange(new Control[] { mysqlTitleLabel, mysqlStatusLabel, startMysqlButton, stopMysqlButton });
            this.Controls.Add(mysqlGroupBox);

            // phpMyAdmin Button
            phpMyAdminButton = new Button { Location = new Point(15, 240), Size = new Size(170, 35), Text = "Open phpMyAdmin" };
            phpMyAdminButton.Click += PhpMyAdminButton_Click;
            this.Controls.Add(phpMyAdminButton);

            // Refresh Button
            refreshStatusButton = new Button { Location = new Point(195, 240), Size = new Size(170, 35), Text = "Refresh Status" };
            refreshStatusButton.Click += RefreshStatusButton_Click;
            this.Controls.Add(refreshStatusButton);

            // Settings Button
            settingsButton = new Button { Location = new Point(15, 285), Size = new Size(370, 35), Text = "Settings" };
            settingsButton.Click += SettingsButton_Click;
            this.Controls.Add(settingsButton);

            // Warning Label
            Label warningLabel = new Label
            {
                Location = new Point(15, 330),
                Size = new Size(370, 40),
                ForeColor = Color.DarkRed,
                Text = "Warning: Stopping MySQL via 'Stop' button forcefully terminates the process, risking data corruption. Use with caution."
            };
            this.Controls.Add(warningLabel);
        }

        #endregion

        // UI Controls (declare them here)
        private GroupBox apacheGroupBox;
        private Label apacheTitleLabel;
        private Label apacheStatusLabel;
        private Button startApacheButton;
        private Button stopApacheButton;

        private GroupBox mysqlGroupBox;
        private Label mysqlTitleLabel;
        private Label mysqlStatusLabel;
        private Button startMysqlButton;
        private Button stopMysqlButton;

        private Button phpMyAdminButton;
        private Button refreshStatusButton;
        private Button settingsButton;
    }
}
