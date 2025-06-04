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

        /// <summary>
        /// Determine whether the server process's output and error streams can be monitored.
        /// </summary>
        protected abstract bool CanMonitorOutput { get; set; }
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

        protected virtual void LogMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                StatusChanged?.Invoke(this, string.Format("[{0:HH:mm:ss}] [{1}] {2}",
                    DateTime.Now,
                    ServerName,
                    message));
            }

        }

        protected virtual void LogError(string message)
        {
            // Note: This method might be redundant. However, it is good to keep it for now just in case we want
            // to log errors on the GUI differently in the future.
            if (!string.IsNullOrWhiteSpace(message))
            {
                ErrorOccurred?.Invoke(this, string.Format("[{0:HH:mm:ss}] [{1}] {2}",
                    DateTime.Now,
                    ServerName,
                    message));
            }
        }

        public async Task<bool> StartAsync()
        {
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

                LogMessage($"is being started..");

                //_serverProcess = await Task.Run(() => StartProcessInNewGroup(_executablePath, arguments));
                _serverProcess = Process.Start(GetProcessStartInfo());

                await Task.Delay(GetStartupDelay());

                if (!IsRunning)
                {
                    LogError($"failed to start, please try again! Exit code: {_serverProcess.ExitCode}");
                    return false;
                }
                if (CanMonitorOutput)
                {
                    ConfigOutputMonitoring();

                }
                _serverProcess.Exited += (sender, e) =>
                {
                    LogMessage($"has exited with code: {_serverProcess.ExitCode}");
                };
                //TODO: Pass the process ID to the main form.
                LogMessage($"started successfully (PID: {_serverProcess.Id})");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"failed to start: {ex.Message}");
                return false;
            }
        }

        private void ConfigOutputMonitoring()
        {
            _serverProcess.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    LogMessage($"[OUT] {e.Data}");
                }
            };
            _serverProcess.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    LogError($"[ERR] {e.Data}");
                }
            };
        }

        public async Task<bool> StopAsync()
        {
            if (!IsRunning)
            {
                LogMessage($"is not running.");
                return true;
            }

            try
            {
                LogMessage($"trying to stop gracefully...");

                //FIXME:
                //TODO: log the amount of seconds the user has to wait for the graceful shutdown to complete.

                //-- 1) First off, we attempt a graceful process shutdown.
                if (await PerformGracefulShutdown())
                {
                    LogMessage($"stopped gracefully!");
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
                LogError($"failed to stop: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ForceStop()
        {
            if (!IsRunning)
            {
                LogMessage($"is not running.");
                return true;
            }
            try
            {
                LogMessage($"is being forcefully stopped..");
                _serverProcess.Kill();
                //_serverProcess.WaitForExit();
                bool exited = await Task.Run(() => _serverProcess.WaitForExit(5000));

                if (exited)
                {
                    LogMessage($"forcely stopped...");
                    return true;
                }
                else
                {
                    LogMessage("failed to forcefully stop. Please try to terminate the process using the Task Manager...");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogError($"failed to stop forcefully: {ex.Message}");
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
