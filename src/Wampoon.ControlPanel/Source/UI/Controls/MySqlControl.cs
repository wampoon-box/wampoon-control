using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wampoon.ControlPanel.UI;
using Wampoon.ControlPanel.Models;
using Wampoon.ControlPanel.Controllers;
using Wampoon.ControlPanel.Enums;
using Wampoon.ControlPanel.Helpers;
using static Wampoon.ControlPanel.Helpers.ErrorLogHelper;

namespace Wampoon.ControlPanel.Controls
{
    internal partial class MySqlControl : ServerControlBase, IDisposable
    {
        private MySQLServerManager _mysqlManager;

        public MySqlControl()
        {
            ServiceName = PackageType.MariaDB.ToServerName();
            DisplayName = "MariaDB Server";
            // Default MySQL port, will be updated in InitializeModule()
            PortNumber = AppConstants.Ports.MYSQL_DEFAULT; 
            lblServerIcon.Text = "🗄️"; 
            btnServerAdmin.Text = "phpMyAdmin";

        }
        public void InitializeModule()
        {
            try
            {
                LogMessage($"Initializing server settings... ", LogType.Info);
                lblServerTitle.Text = DisplayName;
                
                // Read port from MySQL config file
                UpdatePortFromConfig();
                
                // Default admin URL, might need to adjust it to make it use the actual port number.
                ServerAdminUri = AppConstants.Urls.PHPMYADMIN_URL;

                EnsureServerManagerInitialized();

                //_mysqlManager = new MySQLServerManager(
                //    ServerPathManager.GetExecutablePath(PackageType.MariaDB.ToServerName()),
                //    ServerPathManager.GetConfigPath(PackageType.MariaDB.ToServerName()));
                //ServerManager = _mysqlManager;

                //_mysqlManager.ErrorOccurred += HandleServerLogError;
                //_mysqlManager.StatusChanged += HandleServerLogMessage;

                // Set the config file path for the "Open Config File" menu item.
                ConfigFilePath = ServerPathManager.GetConfigPath(PackageType.MariaDB.ToServerName());
                UpdateStatus(CurrentStatus);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogMessage(ex.Message, LogType.Error);
            }
        }

        protected override void InitializeServerManager()
        {
            _mysqlManager = new MySQLServerManager(
                ServerPathManager.GetExecutablePath(PackageType.MariaDB.ToServerName()),
                ServerPathManager.GetConfigPath(PackageType.MariaDB.ToServerName()));
            ServerManager = _mysqlManager;
        }


        protected override async void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckPort(PortNumber, true))
                {
                    return;
                }

                if (!EnsureServerManagerInitialized())
                {
                    return;
                }

                await Task.Run(() => base.BtnStart_Click(sender, e));
            }
            catch (Exception)
            {
                //ExceptionHandlerUtils.HandleUIException(ex, "starting", ServiceName, this);
                btnStart.Enabled = true;
                UpdateStatus(ServerStatus.Stopped);
            }
        }

        /// <summary>
        /// Updates the port number by reading from MySQL configuration file.
        /// </summary>
        private void UpdatePortFromConfig()
        {
            try
            {
                var configPath = ServerPathManager.GetConfigPath(PackageType.MariaDB.ToServerName());
                if (!string.IsNullOrEmpty(configPath))
                {
                    var configuredPort = MySqlConfigParser.ParsePort(configPath, LogMessage);
                    if (configuredPort != PortNumber)
                    {
                        LogMessage($"Port updated from MySQL config: {PortNumber} -> {configuredPort}", LogType.Info);
                        PortNumber = configuredPort;
                    }
                    else
                    {
                        LogMessage($"Using configured MySQL port: {PortNumber}", LogType.Info);
                    }
                }
                else
                {
                    LogMessage($"MySQL config file not found, using default port: {PortNumber}", LogType.Warning);
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogMessage($"Error reading MySQL config, using default port {PortNumber}: {ex.Message}", LogType.Warning);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_mysqlManager != null)
                {
                    _mysqlManager.OnLogServerMessage -= HandleServerLog;
                    _mysqlManager.Dispose();
                    _mysqlManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
