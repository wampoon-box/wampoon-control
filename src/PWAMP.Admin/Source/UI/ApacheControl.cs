using Pwamp.Admin.Controllers;
using Pwamp.Admin.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pwamp.Admin.MainForm;

namespace Pwamp.Admin.Controls
{
    internal partial class ApacheControl: ServerControlBase, IDisposable
    {
        ApacheManager _apacheManager;

        //FIXME: Make these path dynamic or configurable based on the startup location of the current assembly.
        private string apacheHttpdPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\bin\httpd.exe"; // CHANGE THIS

        private string configPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\conf\httpd.conf"; // CHANGE THIS

        public ApacheControl()
        {
            ServiceName = "Apache";
            DisplayName = "Apache HTTP Server";
            lblServerIcon.Text = "🌐"; // FontAwesome icon for server            
        }
        public void InitializeModule()
        {
            lblServerTitle.Text = DisplayName;
            _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
            _apacheManager.ErrorOccurred += LogError;
            _apacheManager.StatusChanged += LogMessage;

            AddLog($"Initializing {ServiceName}", LogType.Info);
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
            try
            {
                btnStart.Enabled = false;                
                UpdateStatus(STATUS_STARTING);
                bool success = await _apacheManager.StartAsync();
                if (success)
                {
                    UpdateStatus(STATUS_RUNNING);
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                }
                else
                {
                    btnStart.Enabled = true;
                    UpdateStatus(STATUS_STOPPED);
                    // Only dispose on failure - manager is reusable
                    //if (_apacheManager != null)
                    //{
                    //    _apacheManager.Dispose();
                    //    _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                    //    //FIXME: reinitialize event handlers?
                    //    //_apacheManager.ErrorOccurred += LogError;
                    //    //_apacheManager.StatusChanged += LogMessage;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                UpdateStatus(STATUS_STOPPED);
                // Only dispose on unrecoverable error
                //if (_apacheManager != null)
                //{
                //    _apacheManager.Dispose();
                //    _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                //    //FIXME: reinitialize event handlers?
                //    //_apacheManager.ErrorOccurred += LogError;
                //    //_apacheManager.StatusChanged += LogMessage;
                //}
            }
        }

        protected async override void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;                
                UpdateStatus(STATUS_STOPPING);

                bool success = await _apacheManager.StopAsync();
                if (success)
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    UpdateStatus(STATUS_STOPPED);
                    // Don't dispose manager - keep it for future use
                }
                else
                {
                    btnStop.Enabled = true;
                    // Only dispose on failure
                    //if (_apacheManager != null)
                    //{
                    //    _apacheManager.Dispose();
                    //    _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                    //    //_apacheManager.ErrorOccurred += LogError;
                    //    //_apacheManager.StatusChanged += LogMessage;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStop.Enabled = true;
                UpdateStatus(STATUS_STOPPED);
                // Only dispose on unrecoverable error
                //if (_apacheManager != null)
                //{
                //    _apacheManager.Dispose();
                //    _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                //    //_apacheManager.ErrorOccurred += LogError;
                //    //_apacheManager.StatusChanged += LogMessage;
                //}
            }
        }               

        public virtual void Dispose()
        {
            if (_apacheManager != null)
            {
                _apacheManager.Dispose();
                _apacheManager = null;
            }
        }

        internal Task StopServer()
        {
            Dispose();
            return Task.CompletedTask;
        }

        internal bool IsRunning()
        {
            return _apacheManager != null && _apacheManager.IsRunning;
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.pcbServerStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // ApacheControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "ApacheControl";
            this.Size = new System.Drawing.Size(354, 143);
            ((System.ComponentModel.ISupportInitialize)(this.pcbServerStatus)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
