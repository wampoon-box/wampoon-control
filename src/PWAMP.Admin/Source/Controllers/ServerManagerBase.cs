using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pwamp.Admin.Controllers
{
    /// <summary>
    /// This class is intended to be a base class for server management functionality.
    /// It can be extended by other classes to implement specific server management features.    
    /// </summary>
    internal abstract class ServerManagerBase : IDisposable
    {
        protected Process _serverProcess;
        protected string _executablePath;
        protected string _configPath;
        public event EventHandler<string> StatusChanged;
        public event EventHandler<string> ErrorOccurred;
        public int? ProcessId => _serverProcess?.Id;
        public abstract string ServerName { get; set; }
        public bool IsRunning => _serverProcess != null && !_serverProcess.HasExited;

        protected ServerManagerBase(string executablePath, string configPath = null)
        {
            _executablePath = executablePath;
            _configPath = configPath;
        }
        protected abstract Task<bool> PerformGracefulShutdown();
        protected abstract string GetStartArguments();
        protected abstract int GetStartupDelay();
        protected abstract ProcessStartInfo GetProcessStartInfo();


        protected virtual void OnStatusChanged(string message)
        {
            StatusChanged?.Invoke(this, message);
        }

        protected virtual void OnErrorOccurred(string message)
        {
            ErrorOccurred?.Invoke(this, message);
        }

        public async Task<bool> StartAsync()
        {
            try
            {
                if (IsRunning)
                {
                    OnStatusChanged($"{ServerName} is already running!");
                    return true;
                }

                if (!File.Exists(_executablePath))
                {
                    OnErrorOccurred($"{ServerName} executable not found: {_executablePath}");
                    return false;
                }

                OnStatusChanged($"Starting {ServerName}...");

                //_serverProcess = await Task.Run(() => StartProcessInNewGroup(_executablePath, arguments));
                _serverProcess = Process.Start(GetProcessStartInfo());
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


        public async Task<bool> StopAsync()
        {
            if (!IsRunning)
            {
                OnStatusChanged($"{ServerName} is not running.");
                return true;
            }

            try
            {
                OnStatusChanged($"Stopping {ServerName} gracefully...");

                //FIXME:
                //TODO: log the amount of seconds the user has to wait for the graceful shutdown to complete.

                //-- 1) First off, we attempt a graceful process shutdown.
                if (await PerformGracefulShutdown())
                {
                    OnStatusChanged($"{ServerName} stopped gracefully!");
                    return true;
                }
                else
                {
                    //-- 2) If graceful shutdown fails, we need to force-kill the process!
                    return await ForceStop();
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"Failed to stop {ServerName}: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ForceStop()
        {
            if (!IsRunning)
            {
                OnStatusChanged($"{ServerName} is not running.");
                return true;
            }
            try
            {
                OnStatusChanged($"Force-stopping {ServerName}...");
                _serverProcess.Kill();
                //_serverProcess.WaitForExit();
                bool exited = await Task.Run(() => _serverProcess.WaitForExit(5000));

                if (exited)
                {
                    OnStatusChanged($"{ServerName} forcely stopped...");
                    return true;
                }
                else
                {
                    OnStatusChanged("Forcefully stopping ${ServerName}, trying TerminateProcess...");
                    return false;
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"Failed to force-stop {ServerName}: {ex.Message}");
                return false;
            }
           
        }

        public virtual void Dispose()
        {
            if (IsRunning)
            {
                Task.Run(async () => await StopAsync()).Wait(5000);
            }
            _serverProcess?.Dispose();
        }
    }
}
