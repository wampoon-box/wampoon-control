using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PwampConsole.Controllers
{
    public abstract class ServerManagerBase : IDisposable
    {
        protected Process _serverProcess;
        protected string _executablePath;
        protected string _configPath;
        protected bool _isRunning;
        protected string _serverName;

        /// <summary>
        /// Initialize the server manager with base directory and server name
        /// </summary>
        protected ServerManagerBase(string baseDirectory, string serverName)
        {
            _serverName = serverName;
            _isRunning = false;

            // Initialize paths (will be implemented by derived classes)
            InitializePaths(baseDirectory);
        }

        /// <summary>
        /// Initialize paths to server executable and configuration files
        /// </summary>
        protected abstract void InitializePaths(string baseDirectory);

        /// <summary>
        /// Get the command line arguments needed to start the server
        /// </summary>
        protected abstract string GetStartArguments();

        /// <summary>
        /// Get the command line arguments needed to stop the server
        /// </summary>
        protected abstract string GetStopArguments();

        /// <summary>
        /// Get the command line arguments needed to restart the server
        /// </summary>
        protected abstract string GetRestartArguments();

        /// <summary>
        /// Start the server with the specified configuration
        /// </summary>
        public virtual bool StartServer()
        {
            if (_isRunning)
            {
                Console.WriteLine($"{_serverName} is already running.");
                return true;
            }

            try
            {
                // Verify paths exist
                if (!File.Exists(_executablePath))
                {
                    Console.WriteLine($"Error: {_serverName} executable not found at {_executablePath}");
                    return false;
                }

                if (!string.IsNullOrEmpty(_configPath) && !File.Exists(_configPath))
                {
                    Console.WriteLine($"Error: {_serverName} config file not found at {_configPath}");
                    return false;
                }

                // Create process start info
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = _executablePath,
                    Arguments = GetStartArguments(),
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                // Start server process
                _serverProcess = new Process
                {
                    StartInfo = startInfo,
                    EnableRaisingEvents = true
                };

                // Set up event handlers for output and error
                _serverProcess.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"{_serverName}: {e.Data}");
                };

                _serverProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"{_serverName} Error: {e.Data}");
                };

                // Set up exit handler
                _serverProcess.Exited += (sender, e) =>
                {
                    _isRunning = false;
                    Console.WriteLine($"{_serverName} exited with code: {_serverProcess.ExitCode}");
                };

                // Start the process
                bool started = _serverProcess.Start();

                if (started)
                {
                    _serverProcess.BeginOutputReadLine();
                    _serverProcess.BeginErrorReadLine();
                    _isRunning = true;

                    Console.WriteLine($"{_serverName} started successfully.");

                    // Wait a moment to see if server crashes immediately
                    Task.Delay(1000).Wait();

                    if (_serverProcess.HasExited)
                    {
                        _isRunning = false;
                        Console.WriteLine($"{_serverName} failed to start. Exit code: {_serverProcess.ExitCode}");
                        return false;
                    }

                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to start {_serverName}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting {_serverName}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Stop the server if it's running
        /// </summary>
        public virtual bool StopServer()
        {
            if (!_isRunning)
            {
                Console.WriteLine($"{_serverName} is not running.");
                return true;
            }

            try
            {
                // Create process start info for stopping server
                ProcessStartInfo stopInfo = new ProcessStartInfo
                {
                    FileName = _executablePath,
                    Arguments = GetStopArguments(),
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process stopProcess = new Process { StartInfo = stopInfo })
                {
                    stopProcess.Start();
                    stopProcess.WaitForExit(10000); // Wait up to 10 seconds
                }

                // Give server a moment to fully shut down
                Task.Delay(2000).Wait();

                if (!_serverProcess.HasExited)
                {
                    Console.WriteLine($"{_serverName} did not exit gracefully. Trying to terminate the process.");
                    _serverProcess.Kill();
                }

                _isRunning = false;
                Console.WriteLine($"{_serverName} stopped.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping {_serverName}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Restart the server
        /// </summary>
        public virtual bool RestartServer()
        {
            try
            {
                string restartArgs = GetRestartArguments();

                // If restart arguments are provided, use them directly
                if (!string.IsNullOrEmpty(restartArgs))
                {
                    ProcessStartInfo restartInfo = new ProcessStartInfo
                    {
                        FileName = _executablePath,
                        Arguments = restartArgs,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    using (Process restartProcess = new Process { StartInfo = restartInfo })
                    {
                        restartProcess.Start();
                        string output = restartProcess.StandardOutput.ReadToEnd();
                        string error = restartProcess.StandardError.ReadToEnd();

                        restartProcess.WaitForExit(10000);

                        if (!string.IsNullOrEmpty(output))
                            Console.WriteLine(output);

                        if (!string.IsNullOrEmpty(error))
                            Console.WriteLine(error);
                    }
                }
                // Otherwise stop and start the server
                else
                {
                    StopServer();
                    StartServer();
                }

                Console.WriteLine($"{_serverName} restarted.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restarting {_serverName}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Check if server is currently running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (_serverProcess != null && !_serverProcess.HasExited)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Dispose method to clean up resources
        /// </summary>
        public virtual void Dispose()
        {
            if (_isRunning)
            {
                StopServer();
            }

            _serverProcess?.Dispose();
        }
    }
}
