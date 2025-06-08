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
    internal partial class MySqlControl : ServerControlBase, IDisposable
    {
        MySQLManager _mysqlManager;

        private string mysqlExecutablePath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\mariadb\bin\mariadbd.exe"; // CHANGE THIS
        private string mysqlConfigPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\mariadb\my.ini"; // CHANGE THIS

        public MySqlControl()
        {
            ServiceName = "MySQL";
            DisplayName = "MySQL DB Server";
            lblServerIcon.Text = "🗄️"; // FontAwesome icon for server
            
        }
        public void InitializeModule()
        {
            lblServerTitle.Text = DisplayName;
            _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
            _mysqlManager.ErrorOccurred += LogError;
            _mysqlManager.StatusChanged += LogMessage;

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
                bool success = await _mysqlManager.StartAsync();
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
                    if (_mysqlManager != null)
                    {
                        _mysqlManager.Dispose();
                        _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
                        //FIXME: reinitialize event handlers?
                        //_apacheManager.ErrorOccurred += LogError;
                        //_apacheManager.StatusChanged += LogMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                UpdateStatus(STATUS_STOPPED);
                // Only dispose on unrecoverable error
                if (_mysqlManager != null)
                {
                    _mysqlManager.Dispose();
                    _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
                    //FIXME: reinitialize event handlers?
                    //_apacheManager.ErrorOccurred += LogError;
                    //_apacheManager.StatusChanged += LogMessage;
                }
            }
        }

        protected async override void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;                
                UpdateStatus(STATUS_STOPPING);

                bool success = await _mysqlManager.StopAsync();
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
                    if (_mysqlManager != null)
                    {
                        _mysqlManager.Dispose();
                        _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
                        //_apacheManager.ErrorOccurred += LogError;
                        //_apacheManager.StatusChanged += LogMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStop.Enabled = true;
                UpdateStatus(STATUS_STOPPED);
                // Only dispose on unrecoverable error
                if (_mysqlManager != null)
                {
                    _mysqlManager.Dispose();
                    _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
                    //_apacheManager.ErrorOccurred += LogError;
                    //_apacheManager.StatusChanged += LogMessage;
                }
            }
        }               

        public virtual void Dispose()
        {
            if (_mysqlManager != null)
            {
                _mysqlManager.Dispose();
                _mysqlManager = null;
            }
        }

        internal Task StopServer()
        {
            Dispose();
            return Task.CompletedTask;
        }

        internal bool IsRunning()
        {
            return _mysqlManager != null && _mysqlManager.IsRunning;
        }
    }
}
