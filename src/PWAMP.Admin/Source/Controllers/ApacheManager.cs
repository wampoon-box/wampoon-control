using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pwamp.Admin.Controllers
{
    internal class ApacheManager : ServerManagerBase
    {
        public override string ServerName { get; set; } = "Apache";

        public ApacheManager(string executablePath, string configPath = null)
            : base(executablePath, configPath)
        {
        }
        protected override string GetStartArguments()
        {
            if (!string.IsNullOrEmpty(_configPath))
            {
                return $"-f \"{_configPath}\"";
            }
            // No additional arguments if config path is not provided.
            return string.Empty;
        }
        protected override int GetStartupDelay()
        {
            // Return 2 seconds delay needed for Apache to start up.
            return 2000;
        }
        public override async Task<bool> StartAsync()
        {
            try
            {
                if (IsRunning)
                {
                    OnStatusChanged($"{ServerName} is already running");
                    return true;
                }


                if (!File.Exists(_executablePath))
                {
                    OnErrorOccurred($"{ServerName} executable not found: {_executablePath}");
                    return false;
                }

                OnStatusChanged($"Starting {ServerName}...");

                ProcessStartInfo startInfo = new ProcessStartInfo(_executablePath)
                {
                    // UseShellExecute = false is important for having a direct process handle
                    // and potentially for console control, though for GenerateConsoleCtrlEvent,
                    // we primarily need the process ID to attach to its console (if it has one).
                    UseShellExecute = false, // Set to true on purpose to allow setting WindowStyle to hidden.
                    //Arguments = GetStartArguments(),
                    //IMPORTANT:
                    // CreateNoWindow = true, // This would hide the Apache console window.
                    // For Ctrl+C to work as described (stopping Apache by "pressing Control-C
                    // in the console window where Apache is running"), Apache needs to be
                    // running in a visible console window that it owns.
                    // If we set CreateNoWindow = true, sending Ctrl+C might not work as expected
                    // because Apache might not set up its console handlers in the same way.
                    CreateNoWindow = false, // Apache needs its own console window for Ctrl+C.
                    WindowStyle = ProcessWindowStyle.Hidden,
                    //RedirectStandardOutput = true,
                    //RedirectStandardError = true
                };


                //_serverProcess = await Task.Run(() => StartProcessInNewGroup(_executablePath, arguments));
                _serverProcess = Process.Start(startInfo);
                await Task.Delay(GetStartupDelay());

                if (!IsRunning)
                {
                    OnErrorOccurred($"{ServerName} failed to start. Exit code: {_serverProcess.ExitCode}");
                    return false;
                }
                //TODO: Pass the process ID to the main form.
                OnStatusChanged($"{ServerName} started successfully (PID: {_serverProcess.Id})");
                return true;
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"Failed to start {ServerName}: {ex.Message}");
                return false;
            }
        }
        public override async Task<bool> StopAsync()
        {
            // Implement Apache stop logic here
            throw new NotImplementedException();
        }
        protected override async Task<bool> PerformGracefulShutdown()
        {
            // Implement graceful shutdown logic here
            throw new NotImplementedException();
        }

    }
}
