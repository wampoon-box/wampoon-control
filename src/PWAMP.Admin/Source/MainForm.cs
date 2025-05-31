using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pwamp.Admin.Controllers;
using Pwamp.Forms;
using Pwamp.Helpers;
using Pwamp.Models;

namespace Pwamp.Admin
{
    public partial class MainForm : Form
    {
        ApacheManager _apacheManager;
        private string apacheHttpdPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\bin\httpd.exe"; // CHANGE THIS
        private string configPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\conf\httpd.conf"; // CHANGE THIS
        public MainForm()
        {
            InitializeComponent();
            _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
            _apacheManager.ErrorOccurred += LogError;
            _apacheManager.StatusChanged += LogMessage;
            InitializeApplication();
        }

        private void LogError(object sender, string message)
        {
            if (logTextBox.InvokeRequired)
            {
                logTextBox.Invoke(new Action<string, string>(LogMessage), message);
                return;
            }

            //// Simple prefix based on service name (no emojis for .NET 4.8 compatibility)
            //string prefix;
            //if (serviceName == "Apache")
            //    prefix = "[A]";
            //else if (serviceName == "MySQL")
            //    prefix = "[M]";
            //else
            //    prefix = "[S]";

            logTextBox.AppendText(message + Environment.NewLine);
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }

        private void LogMessage(object sender, string message)
        {
            if (logTextBox.InvokeRequired)
            {
                logTextBox.Invoke(new Action<string, string>(LogMessage), message);
                return;
            }

            //// Simple prefix based on service name (no emojis for .NET 4.8 compatibility)
            //string prefix;
            //if (serviceName == "Apache")
            //    prefix = "[A]";
            //else if (serviceName == "MySQL")
            //    prefix = "[M]";
            //else
            //    prefix = "[S]";

            logTextBox.AppendText(message + Environment.NewLine);
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }

        private void InitializeApplication()
        {
            this.Text = "PWAMP Control Panel";
        }
        
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO: Check whether processes are running and ask the user if they want to stop them.
            // Ask the user if they want to stop the processes on exit.
            //var result = MessageBox.Show(
            //    "Do you want to stop the running Apache and MySQL processes started by this application before closing?",
            //    "Confirm Exit",
            //    MessageBoxButtons.YesNoCancel,
            //    MessageBoxIcon.Question);

            //if (result == DialogResult.Cancel)
            //{
            //    e.Cancel = true; // Prevent the form from closing
            //}
            //else if (result == DialogResult.Yes)
            //{
            //    // Attempt to stop processes
            //    //Task.Run(() => _processManager.StopAllProcessesAsync()).Wait(5000);
            //}
        }

        private async void btnStartApache_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = await _apacheManager.StartAsync();
                if (success)
                {
                    btnStartApache.Enabled = true;
                    btnStartApache.Text = "Apache: Running";
                    //apacheStatusLabel.ForeColor = Color.Green;
                }
                else
                {
                    btnStartApache.Enabled = true;
                    if (_apacheManager != null)
                    {
                        _apacheManager.Dispose();
                        _apacheManager = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartApache.Enabled = true;
                if (_apacheManager != null)
                {
                    _apacheManager.Dispose();
                    _apacheManager = null;
                }
            }

        }
    }
}
