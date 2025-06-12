using Frostybee.PwampAdmin.Controllers;
using Frostybee.PwampAdmin.Enums;
using Frostybee.PwampAdmin.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Frostybee.PwampAdmin.MainForm;

namespace Frostybee.PwampAdmin.Controls
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
            // Default MySQL port, change if needed.
            PortNumber = 3306; 
            lblServerIcon.Text = "🗄️"; 
            
        }
        public void InitializeModule()
        {
            lblServerTitle.Text = DisplayName;

            _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);            
            ServerManager = _mysqlManager;

            _mysqlManager.ErrorOccurred += LogError;
            _mysqlManager.StatusChanged += LogMessage;

            LogMessage($"Initializing server settings... ", LogType.Info);
        }

        private void LogMessage(object sender, string message)
        {
            //AddLog(string.Format(LanguageManager._("{0} Service is disabled."), ModuleName), LogType.Debug);
            LogMessage(message, LogType.Info);
        }

        private void LogError(object sender, string message)
        {
            LogMessage(message, LogType.Error);
        }
         
        //public virtual void Dispose()
        //{
        //    if (_mysqlManager != null)
        //    {
        //        _mysqlManager.Dispose();
        //        _mysqlManager = null;
        //    }
        //}

        internal Task Dispose()
        {
            if (_mysqlManager != null)
            {
                _mysqlManager.Dispose();
                _mysqlManager = null;
            }
            return Task.CompletedTask;
        }

        internal bool IsRunning()
        {
            return _mysqlManager != null && _mysqlManager.IsRunning;
        }        
    }
}
