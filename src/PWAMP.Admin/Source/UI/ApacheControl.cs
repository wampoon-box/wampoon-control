using Pwamp.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pwamp.Admin.MainForm;

namespace Pwamp.Admin.UI
{
    internal partial class ApacheControl: ServerBaseControl
    {
        ApacheManager _apacheManager;
        private string apacheHttpdPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\bin\httpd.exe"; // CHANGE THIS

        private string configPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\conf\httpd.conf"; // CHANGE THIS

        public ApacheControl()
        {
            ServiceName = "Apache";
            DisplayName = "Apache HTTP Server";
            lblServerIcon.Text = "\uf0e8"; // FontAwesome icon for server
            lblServerTitle.Text = "Apache HTTP Server";
        }
        public void InitializeModule()
        {
            lblServerTitle.Text = "Apache HTTP Server";
            //lblServerTitle.ForeColor = Color.DarkRed;
            this.BackColor = Color.FromArgb(255, 248, 248);
            _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
            AddLog("Initializing module...", LogType.Error);
            _apacheManager.ErrorOccurred += LogError;
            _apacheManager.StatusChanged += LogMessage;

            // Apache-specific styling
            //this.btnStart.BackColor = Color.FromArgb(220, 255, 220);
            //this.btnStop.BackColor = Color.FromArgb(255, 220, 220);
            //this.btnRestart.BackColor = Color.FromArgb(255, 255, 220);

            //LogOutput("Apache HTTP Server control initialized");
            //UpdateStatus("Ready");
        }

        private void LogMessage(object sender, string message)
        {
            //AddLog(string.Format(LanguageManager._("{0} Service is disabled."), ModuleName), LogType.Debug);
            AddLog(message, LogType.Info);
        }

        private void LogError(object sender, string message)
        {
            AddLog(message, LogType.Error);
        }

        protected override void SetupEventHandlers()
        {
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            btnRestart.Click += BtnStop_Click;
        }

        protected async override void BtnStart_Click(object sender, EventArgs e)
        {
            //LogOutput("Executing: httpd.exe -k start");
            //LogOutput("Apache HTTP Server starting...");
            //UpdateStatus("Running");
            //LogOutput("Apache started successfully on port 80 and 443");
            try
            {
                btnStart.Enabled = false;
                btnStart.Text = "Starting...";

                bool success = await _apacheManager.StartAsync();
                if (success)
                {
                    btnStart.Text = "Apache: Running";
                    btnStart.Enabled = true;
                }
                else
                {
                    btnStart.Text = "Start Apache";
                    btnStart.Enabled = true;
                    // Only dispose on failure - manager is reusable
                    if (_apacheManager != null)
                    {
                        _apacheManager.Dispose();
                        _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                        //_apacheManager.ErrorOccurred += LogError;
                        //_apacheManager.StatusChanged += LogMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Text = "Start Apache";
                btnStart.Enabled = true;
                // Only dispose on unrecoverable error
                if (_apacheManager != null)
                {
                    _apacheManager.Dispose();
                    _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                    //_apacheManager.ErrorOccurred += LogError;
                    //_apacheManager.StatusChanged += LogMessage;
                }
            }
        }

        protected async override void BtnStop_Click(object sender, EventArgs e)
        {
            //LogOutput("Executing: httpd.exe -k stop");
            //LogOutput("Apache HTTP Server stopping...");
            //UpdateStatus("Stopped");
            //LogOutput("Apache stopped successfully");

            try
            {
                btnStart.Enabled = false;
                btnStart.Text = "Stopping...";

                bool success = await _apacheManager.StopAsync();
                if (success)
                {
                    btnStart.Text = "Start Apache";
                    btnStart.Text = "Stop Apache";
                    btnStart.Enabled = true;
                    btnStart.Enabled = true;
                    // Don't dispose manager - keep it for future use
                }
                else
                {
                    btnStop.Text = "Stop Apache";
                    btnStop.Enabled = true;
                    // Only dispose on failure
                    if (_apacheManager != null)
                    {
                        _apacheManager.Dispose();
                        _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                        //_apacheManager.ErrorOccurred += LogError;
                        //_apacheManager.StatusChanged += LogMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStop.Text = "Stop Apache";
                btnStop.Enabled = true;
                // Only dispose on unrecoverable error
                if (_apacheManager != null)
                {
                    _apacheManager.Dispose();
                    _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                    //_apacheManager.ErrorOccurred += LogError;
                    //_apacheManager.StatusChanged += LogMessage;
                }
            }
        }

        protected async override void BtnRestart_Click(object sender, EventArgs e)
        {
            //LogOutput("Executing: httpd.exe -k restart");
            //LogOutput("Apache HTTP Server restarting...");
            //UpdateStatus("Restarting");
            //LogOutput("Apache configuration reloaded");
            //UpdateStatus("Running");
            //LogOutput("Apache restarted successfully");
        }
    }
}
