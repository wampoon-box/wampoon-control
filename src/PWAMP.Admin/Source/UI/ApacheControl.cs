using Frostybee.Pwamp.Controllers;
using Frostybee.Pwamp.Enums;
using Frostybee.Pwamp.Helpers;
using Frostybee.Pwamp.Source.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Frostybee.Pwamp.MainForm;

namespace Frostybee.Pwamp.Controls
{
    internal partial class ApacheControl : ServerControlBase, IDisposable
    {
        ApacheManager _apacheManager;
                
        //FIXME: Make these path dynamic or configurable based on the startup location of the current assembly.
        private string apacheHttpdPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\bin\httpd.exe"; // CHANGE THIS

        private string configPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\conf\httpd.conf"; // CHANGE THIS

        public ApacheControl()
        {
            ServiceName = "Apache";
            DisplayName = "Apache HTTP Server";
            PortNumber = 80; // Default HTTP port, change if needed.
            lblServerIcon.Text = "🌐";
        }

        public void InitializeModule()
        {
            lblServerTitle.Text = DisplayName;
            _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
            _apacheManager.ErrorOccurred += LogError;
            _apacheManager.StatusChanged += LogMessage;
            ServerManager = _apacheManager;
            
            ValidateServerConfig();
            
        }

        private void ValidateServerConfig()
        {
            LogMessage($"Initializing server settings... ", LogType.Info);

            //var serverApp = Path.Combine(ConfigManager.BaseDirectory, "apache", "bin", ConfigManager.Config.BinaryNames.Apache);

            //AddLog(LanguageManager._("Checking for module existence..."), LogType.Debug);

            //if (!File.Exists(serverApp))
            //{
            //    StatusPanel.BackColor = ConfigManager.ErrorColor;
            //    AddLog(string.Format(LanguageManager._("Problem detected: {0} Not Found!"), ModuleName), LogType.Error);
            //    AddLog(string.Format(LanguageManager._("Disabling {0} buttons"), ModuleName), LogType.Error);
            //    AddLog(LanguageManager._("Run this program from your XAMPP root directory!"), LogType.Error);

            //    AdminButton.Enabled = false;
            //    ServiceButton.Enabled = false;
            //    StartStopButton.Enabled = false;
            //}

            //if (!ConfigManager.Config.EnableServices.Apache)
            //{
            //    AddLog(string.Format(LanguageManager._("{0} Service is disabled."), ModuleName), LogType.Debug);
            //    ServiceButton.Enabled = false;
            //}

            //AddLog(LanguageManager._("Checking for required tools..."), LogType.Debug);
        }

        private void CheckPort(int port)
        {
            if (NetworkPortHelper.IsPortInUse(port))
            {
                MessageBox.Show($"Port {port} is in use.", "Port Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        //public virtual void Dispose()
        //{
        //    if (_apacheManager != null)
        //    {
        //        _apacheManager.Dispose();
        //        _apacheManager = null;
        //    }
        //}

        internal Task Dispose()
        {
            if (_apacheManager != null)
            {
                _apacheManager.Dispose();
                _apacheManager = null;
            }
            return Task.CompletedTask;
        }

        internal bool IsRunning()
        {
            return _apacheManager != null && _apacheManager.IsRunning;
        }
    }
}
