﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wampoon.ControlPanel.Helpers;
using Wampoon.ControlPanel.UI;
using Wampoon.ControlPanel.Enums;

namespace Wampoon.ControlPanel.Controllers
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
        private bool _disposed = false;
        public event EventHandler<ServerLogEventArgs> OnLogServerMessage;

        public int? ProcessId => _serverProcess?.Id;
        public abstract string ServerName { get; set; }

        /// <summary>
        /// Determine whether the server process's output and error streams can be monitored.
        /// </summary>
        protected abstract bool CanMonitorOutput { get; set; }
        public bool IsRunning => _serverProcess != null && !_serverProcess.HasExited;
        
        protected bool IsMySqlServer => ServerName.Contains(PackageType.MySQL.ToServerName()) || ServerName.Contains(PackageType.MariaDB.ToServerName());

        internal ServerManagerBase(string executablePath, string configPath = null)
        {
            _executablePath = executablePath;
            _configPath = configPath;
        }

        protected abstract Task<bool> PerformGracefulShutdown();
        protected abstract string GetStartArguments();
        protected abstract int GetStartupDelay();
        protected abstract ProcessStartInfo GetProcessStartInfo();

        protected bool ValidateExecutableExists()
        {
            if (!File.Exists(_executablePath))
            {
                LogError(string.Format(AppConstants.Messages.EXECUTABLE_NOT_FOUND, _executablePath));
                return false;
            }
            return true;
        }

        protected virtual void LogMessage(string message, LogType logType = LogType.Info)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                OnLogServerMessage?.Invoke(this, new ServerLogEventArgs(message, logType));
            }
        }

        protected virtual void LogError(string message, LogType logType = LogType.Error)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                LogMessage(message, logType); 
            }
        }

        public async Task<bool> StartAsync()
        {
            try
            {
                if (IsRunning)
                {
                    LogMessage($"Is already running!");
                    return true;
                }

                if (!File.Exists(_executablePath))
                {
                    LogError($"Executable not found: {_executablePath}");
                    return false;
                }

                LogMessage($"Starting...");

                //_serverProcess = await Task.Run(() => StartProcessInNewGroup(_executablePath, arguments));
                _serverProcess = new Process()
                {
                    StartInfo = GetProcessStartInfo()
                };

                if (CanMonitorOutput)
                {
                    ConfigOutputMonitoring();                    
                }

                _serverProcess.Start();

                if (CanMonitorOutput)
                {
                    LogMessage($"Configuring output monitoring...");
                    _serverProcess.BeginOutputReadLine();
                    _serverProcess.BeginErrorReadLine();
                }

                await Task.Delay(GetStartupDelay());

                if (!IsRunning)
                {
                    LogError($"Failed to start, please try again! Exit code: {_serverProcess.ExitCode}");
                    return false;
                }
                _serverProcess.Exited += (sender, e) =>
                {
                    LogError($"Has exited with code: {_serverProcess.ExitCode}");
                };
                //TODO: Pass the process ID to the main form.
                LogMessage($"Started successfully (PID: {_serverProcess.Id})", LogType.Success);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogError($"Failed to start: {ex.Message}");
                return false;
            }
        }

        private void ConfigOutputMonitoring()
        {
            _serverProcess.OutputDataReceived += OnOutputDataReceived;
            _serverProcess.ErrorDataReceived += OnErrorDataReceived;
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                if (IsMySqlServer)
                {
                    MainForm.Instance?.AddMySqlLog($"{e.Data}", LogType.Error);
                }
                else
                {
                    LogError($"{e.Data}");
                }
            }
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                if (IsMySqlServer)
                {
                    MainForm.Instance?.AddMySqlLog($"{e.Data}", LogType.Info);
                }
                else
                {
                    LogMessage($"{e.Data}");
                }
            }
        }

        public async Task<bool> StopAsync()
        {
            if (!IsRunning)
            {
                LogMessage($"Is not running.");
                return true;
            }

            try
            {
                LogMessage($"Trying to stop gracefully...");

                //FIXME:
                //TODO: log the amount of seconds the user has to wait for the graceful shutdown to complete.

                //-- 1) First off, we attempt a graceful process shutdown.
                if (await PerformGracefulShutdown())
                {
                    LogMessage($"Stopped gracefully!", LogType.Success);
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
                ErrorLogHelper.LogExceptionInfo(ex);
                LogError($"Failed to stop: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ForceStop()
        {
            if (!IsRunning)
            {
                LogMessage($"Is not running.");
                return true;
            }
            try
            {
                LogMessage($"Stopping it forcefully...");
                _serverProcess.Kill();
                //_serverProcess.WaitForExit();
                bool exited = await Task.Run(() => _serverProcess.WaitForExit(AppConstants.Timeouts.PROCESS_WAIT_TIMEOUT_MS));

                if (exited)
                {
                    LogMessage($"Forcefully stopped...");
                    return true;
                }
                else
                {
                    LogMessage("Failed to forcefully stop. Please try to terminate the process using the Task Manager...");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogError($"Failed to stop forcefully: {ex.Message}");
                return false;
            }
        }

        public virtual void Dispose()
        {
            if (_disposed)
                return;

            if (IsRunning)
            {
                try
                {
                    StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    ErrorLogHelper.LogExceptionInfo(ex);
                    LogError($"Error during disposal: {ex.Message}");
                }
            }

            if (_serverProcess != null)
            {
                _serverProcess.OutputDataReceived -= OnOutputDataReceived;
                _serverProcess.ErrorDataReceived -= OnErrorDataReceived;
                _serverProcess.Dispose();
                _serverProcess = null;
            }

            _disposed = true;
        }
    }
}
