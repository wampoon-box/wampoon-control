using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wampoon.ControlPanel.Helpers;
using Wampoon.ControlPanel.Enums;

namespace Wampoon.ControlPanel.Controllers
{
    internal class MySQLServerManager : ServerManagerBase
    {
        public override string ServerName { get; set; } = PackageType.MariaDB.ToServerName();
        protected override bool CanMonitorOutput { get; set; } = true;

        public MySQLServerManager(string executablePath, string configPath = null) : base(executablePath, configPath)
        {
        }

        protected override ProcessStartInfo GetProcessStartInfo()
        {
            CanMonitorOutput = false;
            return new ProcessStartInfo()
            {
                FileName = _executablePath,
                Arguments = GetStartArguments(),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                WindowStyle = ProcessWindowStyle.Normal,
            };
        }

        protected override string GetStartArguments()
        {
            if (!string.IsNullOrEmpty(_configPath))
            {
                string wampoonRootDir = ServerPathManager.AppBaseDirectory.Replace('\\', '/');          
                string maraiaDbLogsDir = $"{wampoonRootDir}/apps/temp/mariadb_logs";
                //--console
                return string.Format(
                    "--defaults-file={0} --log-error={1} --slow_query_log_file={2} --general_log_file={3} ", 
                    _configPath,
                    $"{maraiaDbLogsDir}/mariadb_error.log",
                    $"{maraiaDbLogsDir}/mysql_slow.log",
                    $"{maraiaDbLogsDir}/mariadb-general.log"
                    );
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
            return AppConstants.Timeouts.MYSQL_STARTUP_DELAY_MS;
        }

        protected override async Task<bool> PerformGracefulShutdown()
        {
            // Use the MySQL/MariaDB shutdown command.
            string mariaDbBinPath = Path.GetDirectoryName(_executablePath);
            string mariaDbAdminExe = Path.Combine(mariaDbBinPath, "mariadb-admin.exe");

            LogError($"Attempting to stop { ServerName}", LogType.Warning);

            try
            {
                if (!ValidateExecutableExists())
                {
                    return false;
                }

                LogMessage($"Stopping...");

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
