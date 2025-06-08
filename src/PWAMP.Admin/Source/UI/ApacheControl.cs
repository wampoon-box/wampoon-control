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
            ServerManager = _apacheManager;

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
    }
}
