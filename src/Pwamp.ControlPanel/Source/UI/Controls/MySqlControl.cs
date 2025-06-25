using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frostybee.Pwamp.UI;
using Frostybee.Pwamp.Models;
using Frostybee.Pwamp.Controllers;
using Frostybee.Pwamp.Enums;
using Frostybee.Pwamp.Helpers;
using static Frostybee.Pwamp.Helpers.ErrorLogHelper;

namespace Frostybee.Pwamp.Controls
{
    internal partial class MySqlControl : ServerControlBase, IDisposable
    {
        private MySQLServerManager _mysqlManager;

        public MySqlControl()
        {
            ServiceName = PackageType.MariaDB.ToServerName();
            DisplayName = "MariaDB Server";
            // Default MySQL port, change if needed.
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
                if (!CheckPort(PortNumber, false))
                {
                    return;
                }

                if (!EnsureServerManagerInitialized())
                {
                    return;
                }

                base.BtnStart_Click(sender, e);
            }
            catch (Exception ex)
            {
                //ExceptionHandlerUtils.HandleUIException(ex, "starting", ServiceName, this);
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
                    _mysqlManager.OnLogServerMessage -= HandleServerLog;
                    _mysqlManager.Dispose();
                    _mysqlManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
