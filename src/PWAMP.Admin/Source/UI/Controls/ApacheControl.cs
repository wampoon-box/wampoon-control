using Frostybee.Pwamp.Models;
using Frostybee.PwampAdmin.Controllers;
using Frostybee.PwampAdmin.Enums;
using Frostybee.PwampAdmin.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Frostybee.PwampAdmin.MainForm;

namespace Frostybee.PwampAdmin.Controls
{
    internal partial class ApacheControl : ServerControlBase, IDisposable
    {
        private ApacheManager _apacheManager;
        private readonly AppBootstrap _appBootstrap;
        public ApacheControl()
        {
            ServiceName = "Apache";
            DisplayName = "Apache HTTP Server";
            PortNumber = 80; // Default HTTP port, change if needed.
            lblServerIcon.Text = "🌐";
            _appBootstrap = new AppBootstrap();
        }

        public void InitializeModule()
        {
            try
            {
                lblServerTitle.Text = DisplayName;
                // We need to update the Apache and phpMyAdmin paths to ensure they are properly set up in case
                // the application has been moved to a different directory/drive.
                _appBootstrap.UpdateApacheConfig();

                _apacheManager = ServerManagerFactory.CreateServerManager<ApacheManager>(ServerDefinitions.Apache.Name);
                //_apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                _apacheManager.ErrorOccurred += LogError;
                _apacheManager.StatusChanged += LogMessage;
                ServerManager = _apacheManager;
            }
            catch (Exception ex)
            {
                LogMessage($"An error occurred while initializing the Apache module: {ex.Message}", LogType.Error);
            }

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
