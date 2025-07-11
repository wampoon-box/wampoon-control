
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Wampoon.ControlPanel.Helpers;

namespace Wampoon.ControlPanel.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            CenterToScreen();
            LoadApplicationInfo();
        }

        private void LoadApplicationInfo()
        {
            Text = $"About {AppConstants.APP_NAME}";
            appNameLabel.Text = AppConstants.APP_NAME;
            
            // Display the current installer version
            var version = SystemHelper.GetInstallerVersion();
            appVersionLabel.Text = $"Version {version}";
            
            copyrightLabel.Text = "Copyright © 2025 - frostybee";
            descriptionLabel.Text = "A lightweight control panel for managing Apache and MariaDB servers in the WAMPoon (Portable Windows Apache MySQL PHP) environment.";

            // Load license and credits information
            LoadCreditsAndLicense();
        }

        private void LoadCreditsAndLicense()
        {
            // License information
            licenseLabel.Text = "Licensed under the MIT License";
            
            // Dependencies and credits
            dependenciesLabel.Text = "Special thanks to the open-source community";
        }

        private void GitHubRepoButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(AppConstants.GITHUB_REPO_URI);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open GitHub repository: {ex.Message}\n\nYou can visit the repository manually at: {AppConstants.GITHUB_REPO_URI}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GitHubIssuesButton_Click(object sender, EventArgs e)
        {
            try
            {
                var issuesUrl = $"{AppConstants.GITHUB_REPO_URI}/issues";
                Process.Start(issuesUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open GitHub Issues: {ex.Message}\n\nYou can visit Issues manually at: {AppConstants.GITHUB_REPO_URI}/issues", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ViewLicenseButton_Click(object sender, EventArgs e)
        {
            try
            {
                var licenseUrl = $"{AppConstants.GITHUB_REPO_URI}/blob/main/LICENSE";
                Process.Start(licenseUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open license: {ex.Message}\n\nYou can view the license manually at: {AppConstants.GITHUB_REPO_URI}/blob/main/LICENSE", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}