using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frostybee.Pwamp.Helpers;

namespace Frostybee.Pwamp.Controllers
{
    internal class MySQLServerManager : ServerManagerBase
    {
        public override string ServerName { get; set; } = "MariaDB";
        protected override bool CanMonitorOutput { get; set; } = true;

        public MySQLServerManager(string executablePath, string configPath = null) : base(executablePath, configPath)
        {
        }

        protected override ProcessStartInfo GetProcessStartInfo()
        {
            return new ProcessStartInfo()
            {
                FileName = _executablePath,
                Arguments = GetStartArguments(),
                UseShellExecute = false, 
                CreateNoWindow = true, 
                RedirectStandardError = true, 
                RedirectStandardOutput = true, 
                WindowStyle = ProcessWindowStyle.Normal,         
            };
        }

        protected override string GetStartArguments()
        {

            if (!string.IsNullOrEmpty(_configPath))
            {
                return $"--defaults-file={_configPath} --console";
            }
            return string.Empty;
        }
        /// <summary>
        /// MySQL/MariaDB shutdown command.
        /// </summary>
        /// <returns>The command to shutdown MySQL/MariaDB server. </returns>
        private string GetStopArguments()
        {
            return "shutdown -u root";
        }

        protected override int GetStartupDelay()
        {
            // Allocate 3 seconds delay needed for MySQL to start up.
            return 3000;
        }

        protected override async Task<bool> PerformGracefulShutdown()
        {
            // Use the MySQL/MariaDB shutdown command.
            string mariaDbBinPath = Path.GetDirectoryName(_executablePath);
            string mariaDbAdminExe = Path.Combine(mariaDbBinPath, "mariadb-admin.exe");

            LogError($"Attempting to stop { ServerName}");

            try
            {
                if (!File.Exists(_executablePath))
                {
                    LogError($"Executable not found: {_executablePath}");
                    return false;
                }

                LogMessage($"Stopping..");

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = mariaDbAdminExe,
                    Arguments = GetStopArguments(),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                //_serverProcess = await Task.Run(() => StartProcessInNewGroup(_executablePath, arguments));
                Process shutdownProcess = Process.Start(processStartInfo);

                await Task.Delay(GetStartupDelay());

                shutdownProcess.Exited += (sender, e) =>
                {
                    LogMessage($"Has exited with code: {shutdownProcess.ExitCode}");
                };

                //TODO: Check if the shutdown was successful by checking the exit code or output.
                // Or check if the process is no longer running?                
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogError($"Failed to stop: {ex.Message}");
                return false;
            }
        }
        
    }
}
