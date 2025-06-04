using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pwamp.Admin.Controllers
{
    internal class MySQLManager : ServerManagerBase
    {
        public override string ServerName { get; set; } = "MariaDB";
        protected override bool CanMonitorOutput { get; set; } = true;

        public MySQLManager(string executablePath, string configPath = null) : base(executablePath, configPath)
        {
        }

        protected override ProcessStartInfo GetProcessStartInfo()
        {
            return new ProcessStartInfo()
            {
                FileName = _executablePath,
                UseShellExecute = false, 
                CreateNoWindow = true, 
                RedirectStandardError = true, 
                RedirectStandardOutput = true, 
                WindowStyle = ProcessWindowStyle.Normal,         
            };
        }

        protected override string GetStartArguments()
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

            try
            {
                if (IsRunning)
                {
                    LogMessage($"is already running!");
                    return true;
                }

                if (!File.Exists(_executablePath))
                {
                    LogError($"executable not found: {_executablePath}");
                    return false;
                }

                LogMessage($"is being stopped..");

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = mariaDbAdminExe,
                    Arguments = GetStartArguments(),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                //_serverProcess = await Task.Run(() => StartProcessInNewGroup(_executablePath, arguments));
                Process shutdownProcess = Process.Start(processStartInfo);

                await Task.Delay(GetStartupDelay());

                if (!IsRunning)
                {
                    LogError($"failed to start, please try again! Exit code: {shutdownProcess.ExitCode}");
                    return false;
                }

                shutdownProcess.Exited += (sender, e) =>
                {
                    LogMessage($"has exited with code: {shutdownProcess.ExitCode}");
                };

                //TODO: Check if the shutdown was successful by checking the exit code or output.
                // Or check if the process is no longer running?

                //TODO: Pass the process ID to the main form.
                LogMessage($"started successfully (PID: {shutdownProcess.Id})");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"failed to start: {ex.Message}");
                return false;
            }
        }
    }
}
