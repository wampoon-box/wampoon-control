using System.Drawing;
using System.Windows.Forms;

namespace Frostybee.Forms.UI
{
    partial class SettingsForm
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
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "WAMPoon Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;

            // Apache Group Box
            GroupBox apacheGroupBox = new GroupBox
            {
                Text = "Apache Settings",
                Location = new Point(15, 15),
                Size = new Size(570, 100)
            };

            // Apache Exe Path
            Label apacheExePathLabel = new Label
            {
                Text = "Executable Path:",
                Location = new Point(10, 25),
                Size = new Size(100, 20)
            };

            apacheExePathTextBox = new TextBox
            {
                Location = new Point(110, 25),
                Size = new Size(370, 20)
            };

            Button browseApacheExeButton = new Button
            {
                Text = "Browse...",
                Location = new Point(490, 23),
                Size = new Size(70, 25)
            };
            browseApacheExeButton.Click += BrowseApacheExeButton_Click;

            // Apache Working Directory
            Label apacheWorkingDirLabel = new Label
            {
                Text = "Working Dir:",
                Location = new Point(10, 55),
                Size = new Size(100, 20)
            };

            apacheWorkingDirTextBox = new TextBox
            {
                Location = new Point(110, 55),
                Size = new Size(370, 20)
            };

            Button browseApacheWorkingDirButton = new Button
            {
                Text = "Browse...",
                Location = new Point(490, 53),
                Size = new Size(70, 25)
            };
            browseApacheWorkingDirButton.Click += BrowseApacheWorkingDirButton_Click;

            apacheGroupBox.Controls.AddRange(new Control[]
            {
                apacheExePathLabel, apacheExePathTextBox, browseApacheExeButton,
                apacheWorkingDirLabel, apacheWorkingDirTextBox, browseApacheWorkingDirButton
            });

            // MySQL Group Box
            GroupBox mysqlGroupBox = new GroupBox
            {
                Text = "MySQL Settings",
                Location = new Point(15, 125),
                Size = new Size(570, 100)
            };

            // MySQL Exe Path
            Label mysqlExePathLabel = new Label
            {
                Text = "Executable Path:",
                Location = new Point(10, 25),
                Size = new Size(100, 20)
            };

            mysqlExePathTextBox = new TextBox
            {
                Location = new Point(110, 25),
                Size = new Size(370, 20)
            };

            Button browseMySqlExeButton = new Button
            {
                Text = "Browse...",
                Location = new Point(490, 23),
                Size = new Size(70, 25)
            };
            browseMySqlExeButton.Click += BrowseMySqlExeButton_Click;

            // MySQL Working Directory
            Label mysqlWorkingDirLabel = new Label
            {
                Text = "Working Dir:",
                Location = new Point(10, 55),
                Size = new Size(100, 20)
            };

            mysqlWorkingDirTextBox = new TextBox
            {
                Location = new Point(110, 55),
                Size = new Size(370, 20)
            };

            Button browseMySqlWorkingDirButton = new Button
            {
                Text = "Browse...",
                Location = new Point(490, 53),
                Size = new Size(70, 25)
            };
            browseMySqlWorkingDirButton.Click += BrowseMySqlWorkingDirButton_Click;

            mysqlGroupBox.Controls.AddRange(new Control[]
            {
                mysqlExePathLabel, mysqlExePathTextBox, browseMySqlExeButton,
                mysqlWorkingDirLabel, mysqlWorkingDirTextBox, browseMySqlWorkingDirButton
            });

            // phpMyAdmin Group Box
            GroupBox phpMyAdminGroupBox = new GroupBox
            {
                Text = "phpMyAdmin Settings",
                Location = new Point(15, 235),
                Size = new Size(570, 60)
            };

            // phpMyAdmin URL
            Label phpMyAdminUrlLabel = new Label
            {
                Text = "URL:",
                Location = new Point(10, 25),
                Size = new Size(100, 20)
            };

            phpMyAdminUrlTextBox = new TextBox
            {
                Location = new Point(110, 25),
                Size = new Size(450, 20)
            };

            phpMyAdminGroupBox.Controls.AddRange(new Control[]
            {
                phpMyAdminUrlLabel, phpMyAdminUrlTextBox
            });

            // Buttons
            Button saveButton = new Button
            {
                Text = "Save",
                Location = new Point(410, 310),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };
            saveButton.Click += SaveButton_Click;

            Button cancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(500, 310),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };
            cancelButton.Click += CancelButton_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[]
            {
                apacheGroupBox,
                mysqlGroupBox,
                phpMyAdminGroupBox,
                saveButton,
                cancelButton
            });

            this.AcceptButton = saveButton;
            this.CancelButton = cancelButton;
        }

        #endregion

        // Form controls
        private TextBox apacheExePathTextBox;
        private TextBox apacheWorkingDirTextBox;
        private TextBox mysqlExePathTextBox;
        private TextBox mysqlWorkingDirTextBox;
        private TextBox phpMyAdminUrlTextBox;
    }
}
