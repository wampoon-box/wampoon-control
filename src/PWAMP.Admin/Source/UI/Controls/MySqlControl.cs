using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frostybee.PwampAdmin.UI;
using Frostybee.Pwamp.Models;
using Frostybee.PwampAdmin.Controllers;
using Frostybee.PwampAdmin.Enums;
using Frostybee.PwampAdmin.Helpers;
using static Frostybee.PwampAdmin.Helpers.ErrorLogHelper;

namespace Frostybee.PwampAdmin.Controls
{
    internal partial class MySqlControl : ServerControlBase, IDisposable
    {
        private MySQLServerManager _mysqlManager;

        public MySqlControl()
        {
            ServiceName = "MySQL";
            DisplayName = "MySQL DB Server";
            // Default MySQL port, change if needed.
            PortNumber = 3306; 
            lblServerIcon.Text = "🗄️"; 
            btnServerAdmin.Text = "phpMyAdmin";

        }
        public void InitializeModule()
        {
            try
            {
                lblServerTitle.Text = DisplayName;
                // Default admin URL, might need to adjust it to make it use the actual port number.
                ServerAdminUri = $"http://localhost/phpmyadmin"; 
                _mysqlManager = ServerManagerFactory.CreateServerManager<MySQLServerManager>(ServerDefinitions.MariaDB.Name);            
                ServerManager = _mysqlManager;

                _mysqlManager.ErrorOccurred += LogError;
                _mysqlManager.StatusChanged += LogMessage;

                LogMessage($"Initializing server settings... ", LogType.Info);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogMessage(ex.Message, LogType.Error);
            }
        }

        private void LogMessage(object sender, string message)
        {
            //AddLog(string.Format(LanguageManager._("{0} Service is disabled."), ModuleName), LogType.Debug);
            LogMessage(message, LogType.Info);
        }

        private void LogError(object sender, string message)
        {
            LogMessage(message, LogType.Error);
            MainForm.Instance?.AddErrorLog("MySQL", message);
        }
         
        internal bool IsRunning()
        {
            return _mysqlManager != null && _mysqlManager.IsRunning;
        }

        private bool CheckPort(int port, bool showDialog = true)
        {
            if (NetworkPortHelper.IsPortInUse(port))
            {
                if (showDialog)
                {
                    MessageBox.Show($"Port {port} is in use. MySQL server cannot be started.", "Port In Use",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }
            return true;
        }

        protected override async void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckPort(PortNumber, false))
                {
                    LogMessage($"Port {PortNumber} is in use. Cannot start {ServiceName}.", LogType.Warning);
                    MessageBox.Show($"Port {PortNumber} is in use. Please close the application using this port and try again.", 
                                  "Port In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ServerManager == null)
                {
                    LogMessage("Server manager is not initialized. Attempting to reinitialize...", LogType.Warning);
                    
                    try
                    {
                        _mysqlManager = ServerManagerFactory.CreateServerManager<MySQLServerManager>(ServerDefinitions.MariaDB.Name);
                        _mysqlManager.ErrorOccurred += LogError;
                        _mysqlManager.StatusChanged += LogMessage;
                        ServerManager = _mysqlManager;
                        LogMessage("Server manager reinitialized successfully.", LogType.Info);
                    }
                    catch (Exception initEx)
                    {
                        ErrorLogHelper.LogExceptionInfo(initEx);
                        LogMessage($"Failed to reinitialize server manager: {initEx.Message}", LogType.Error);
                        MessageBox.Show($"Cannot start {ServiceName}: Server manager initialization failed.\n\n{initEx.Message}", 
                                      "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                base.BtnStart_Click(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogMessage($"Error starting {ServiceName}: {ex.Message}", LogType.Error);
                MessageBox.Show($"Error starting {ServiceName}: {ex.Message}", "Startup Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                UpdateStatus(ServerStatus.Stopped);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_mysqlManager != null)
                {
                    _mysqlManager.ErrorOccurred -= LogError;
                    _mysqlManager.StatusChanged -= LogMessage;
                    _mysqlManager.Dispose();
                    _mysqlManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
