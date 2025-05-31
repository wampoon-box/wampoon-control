using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public abstract Task<bool> StartAsync();
        public abstract  Task<bool> StopAsync();
        protected abstract Task<bool> PerformGracefulShutdown();
        protected abstract string GetStartArguments();        
        protected abstract int GetStartupDelay();

        protected virtual void OnStatusChanged(string message)
        {
            StatusChanged?.Invoke(this, message);
        }

        protected virtual void OnErrorOccurred(string message)
        {
            ErrorOccurred?.Invoke(this, message);
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
