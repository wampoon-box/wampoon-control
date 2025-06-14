using System;
using System.IO;
using System.Windows.Forms;
using Frostybee.PwampAdmin.Helpers;
using Frostybee.PwampAdmin.Models;

namespace Frostybee.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly Settings _settings;

        public SettingsForm(Settings settings)
        {
            InitializeComponent();
            _settings = settings ?? new Settings();
            LoadSettingsToForm();
        }

        private void LoadSettingsToForm()
        {
            // Apache settings
            apacheExePathTextBox.Text = _settings.ApacheExePath;
            apacheWorkingDirTextBox.Text = _settings.ApacheWorkingDir;
            
            // MySQL settings
            mysqlExePathTextBox.Text = _settings.MySqlExePath;
            mysqlWorkingDirTextBox.Text = _settings.MySqlWorkingDir;
            
            // phpMyAdmin settings
            phpMyAdminUrlTextBox.Text = _settings.PhpMyAdminUrl;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Validate paths
            if (!ValidateSettings())
            {
                return;
            }

            // Update settings
            _settings.ApacheExePath = apacheExePathTextBox.Text.Trim();
            _settings.ApacheWorkingDir = apacheWorkingDirTextBox.Text.Trim();
            _settings.MySqlExePath = mysqlExePathTextBox.Text.Trim();
            _settings.MySqlWorkingDir = mysqlWorkingDirTextBox.Text.Trim();
            _settings.PhpMyAdminUrl = phpMyAdminUrlTextBox.Text.Trim();

            // Save settings
            //if (SettingsManager.SaveSettings(_settings))
            //{
            //    MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    DialogResult = DialogResult.OK;
            //    Close();
            //}
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateSettings()
        {
            // Validate Apache executable
            if (!File.Exists(apacheExePathTextBox.Text.Trim()))
            {
                MessageBox.Show("Apache executable file not found. Please check the path.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                apacheExePathTextBox.Focus();
                return false;
            }

            // Validate Apache working directory
            if (!Directory.Exists(apacheWorkingDirTextBox.Text.Trim()))
            {
                MessageBox.Show("Apache working directory not found. Please check the path.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                apacheWorkingDirTextBox.Focus();
                return false;
            }

            // Validate MySQL executable
            if (!File.Exists(mysqlExePathTextBox.Text.Trim()))
            {
                MessageBox.Show("MySQL executable file not found. Please check the path.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mysqlExePathTextBox.Focus();
                return false;
            }

            // Validate MySQL working directory
            if (!Directory.Exists(mysqlWorkingDirTextBox.Text.Trim()))
            {
                MessageBox.Show("MySQL working directory not found. Please check the path.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mysqlWorkingDirTextBox.Focus();
                return false;
            }

            // Validate phpMyAdmin URL (basic validation)
            string url = phpMyAdminUrlTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(url) || 
                (!url.StartsWith("http://") && !url.StartsWith("https://")))
            {
                MessageBox.Show("phpMyAdmin URL must start with http:// or https://", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                phpMyAdminUrlTextBox.Focus();
                return false;
            }

            return true;
        }

        private void BrowseApacheExeButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
                dialog.Title = "Select Apache Executable";
                
                if (File.Exists(apacheExePathTextBox.Text))
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(apacheExePathTextBox.Text);
                    dialog.FileName = Path.GetFileName(apacheExePathTextBox.Text);
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    apacheExePathTextBox.Text = dialog.FileName;
                    
                    // Auto-set working directory if it's empty
                    if (string.IsNullOrWhiteSpace(apacheWorkingDirTextBox.Text))
                    {
                        apacheWorkingDirTextBox.Text = Path.GetDirectoryName(dialog.FileName);
                    }
                }
            }
        }

        private void BrowseApacheWorkingDirButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select Apache Working Directory";
                
                if (Directory.Exists(apacheWorkingDirTextBox.Text))
                {
                    dialog.SelectedPath = apacheWorkingDirTextBox.Text;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    apacheWorkingDirTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void BrowseMySqlExeButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
                dialog.Title = "Select MySQL Executable";
                
                if (File.Exists(mysqlExePathTextBox.Text))
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(mysqlExePathTextBox.Text);
                    dialog.FileName = Path.GetFileName(mysqlExePathTextBox.Text);
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    mysqlExePathTextBox.Text = dialog.FileName;
                    
                    // Auto-set working directory if it's empty
                    if (string.IsNullOrWhiteSpace(mysqlWorkingDirTextBox.Text))
                    {
                        mysqlWorkingDirTextBox.Text = Path.GetDirectoryName(dialog.FileName);
                    }
                }
            }
        }

        private void BrowseMySqlWorkingDirButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select MySQL Working Directory";
                
                if (Directory.Exists(mysqlWorkingDirTextBox.Text))
                {
                    dialog.SelectedPath = mysqlWorkingDirTextBox.Text;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    mysqlWorkingDirTextBox.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
