using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PwampControl.Models;

namespace PwampControl.Helpers
{
    public class ProcessManager
    {
        /// <summary>
        /// Delegate for process status change events
        /// </summary>
        public delegate void ProcessStatusChangedEventHandler(string processName, bool isRunning, string statusText, System.Drawing.Color statusColor);
        
        /// <summary>
        /// Event that fires when a process status changes
        /// </summary>
        public event ProcessStatusChangedEventHandler ProcessStatusChanged;

        // Process references
        private Process _apacheProcess;
        private Process _mysqlProcess;
        
        // Settings
        private readonly Settings _settings;
        
        // Form reference for invoking UI updates from background threads
        private readonly Form _parentForm;

        public ProcessManager(Settings settings, Form parentForm)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (parentForm == null)
                throw new ArgumentNullException("parentForm");
                
            _settings = settings;
            _parentForm = parentForm;
        }

        /// <summary>
        /// Starts the Apache process
        /// </summary>
        public async Task<bool> StartApacheAsync()
        {
            _apacheProcess = await StartProcessAsync(
                _settings.ApacheExePath,
                _settings.ApacheWorkingDir,
                "Apache");
            
            return _apacheProcess != null;
        }

        /// <summary>
        /// Stops the Apache process
        /// </summary>
        public async Task<bool> StopApacheAsync()
        {
            bool result = await StopProcessAsync(_apacheProcess, "Apache");
            if (result)
            {
                _apacheProcess = null;
            }
            return result;
        }

        /// <summary>
        /// Starts the MySQL process
        /// </summary>
        public async Task<bool> StartMySqlAsync()
        {
            _mysqlProcess = await StartProcessAsync(
                _settings.MySqlExePath,
                _settings.MySqlWorkingDir,
                "MySQL");
            
            return _mysqlProcess != null;
        }

        /// <summary>
        /// Stops the MySQL process
        /// </summary>
        public async Task<bool> StopMySqlAsync()
        {
            bool result = await StopProcessAsync(_mysqlProcess, "MySQL");
            if (result)
            {
                _mysqlProcess = null;
            }
            return result;
        }

        /// <summary>
        /// Checks the status of the Apache process
        /// </summary>
        public void CheckApacheStatus()
        {
            CheckProcessStatus(ref _apacheProcess, "Apache");
        }

        /// <summary>
        /// Checks the status of the MySQL process
        /// </summary>
        public void CheckMySqlStatus()
        {
            CheckProcessStatus(ref _mysqlProcess, "MySQL");
        }

        /// <summary>
        /// Checks the status of all processes
        /// </summary>
        public void CheckAllProcesses()
        {
            CheckApacheStatus();
            CheckMySqlStatus();
        }

        /// <summary>
        /// Stops all processes when the application is closing
        /// </summary>
        public async Task StopAllProcessesAsync()
        {
            await StopApacheAsync();
            await StopMySqlAsync();
        }

        /// <summary>
        /// Generic method to start a process
        /// </summary>
        private async Task<Process> StartProcessAsync(string exePath, string workingDir, string processName)
        {
            // Check if already running
            if (IsProcessRunning(processName))
            {
                OnProcessStatusChanged(processName, true, "Running", System.Drawing.Color.Green);
                return GetProcessReference(processName);
            }

            // Validate paths
            if (!File.Exists(exePath))
            {
                MessageBox.Show("Executable not found:\n" + exePath + "\nPlease check the path configuration.", 
                    "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OnProcessStatusChanged(processName, false, "Not Found", System.Drawing.Color.Red);
                return null;
            }
            
            if (!Directory.Exists(workingDir))
            {
                MessageBox.Show("Working directory not found:\n" + workingDir + "\nPlease check the path configuration.", 
                    "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OnProcessStatusChanged(processName, false, "Bad Dir", System.Drawing.Color.Red);
                return null;
            }

            Process newProcess = null;
            try
            {
                OnProcessStatusChanged(processName, false, "Starting...", System.Drawing.Color.Orange);

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    WorkingDirectory = workingDir,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                await Task.Run(() => {
                    newProcess = Process.Start(startInfo);
                    if (newProcess != null)
                    {
                        // Give it a moment to potentially crash on startup
                        Thread.Sleep(1000);
                        
                        // Check if it exited immediately
                        if (newProcess.HasExited)
                        {
                            newProcess.Dispose();
                            newProcess = null;
                            throw new Exception(Path.GetFileName(exePath) + " exited unexpectedly shortly after start.");
                        }
                        
                        newProcess.EnableRaisingEvents = true;
                        newProcess.Exited += (sender, e) => Process_Exited(sender, e, processName);
                    }
                });

                if (newProcess == null)
                {
                    throw new Exception("Process failed to start.");
                }

                // Update status after successful start
                OnProcessStatusChanged(processName, true, "Running", System.Drawing.Color.Green);
                return newProcess;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start " + Path.GetFileName(exePath) + ":\n" + ex.Message, 
                    "Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                if (newProcess != null)
                {
                    newProcess.Dispose();
                }
                
                OnProcessStatusChanged(processName, false, "Error", System.Drawing.Color.Red);
                return null;
            }
        }

        /// <summary>
        /// Generic method to stop a process
        /// </summary>
        private async Task<bool> StopProcessAsync(Process processToStop, string processName)
        {
            if (processToStop == null)
            {
                OnProcessStatusChanged(processName, false, "Stopped", System.Drawing.Color.Red);
                return true; // Already stopped
            }

            try
            {
                if (processToStop.HasExited)
                {
                    processToStop.Dispose();
                    OnProcessStatusChanged(processName, false, "Stopped", System.Drawing.Color.Red);
                    return true;
                }

                OnProcessStatusChanged(processName, true, "Stopping...", System.Drawing.Color.Orange);

                bool success = await Task.Run(() =>
                {
                    try
                    {
                        // Try to close main window gracefully
                        if (!processToStop.HasExited && processToStop.MainWindowHandle != IntPtr.Zero)
                        {
                            processToStop.CloseMainWindow();
                            if (processToStop.WaitForExit(3000))
                            {
                                return true;
                            }
                        }

                        // If still running, forcefully kill
                        if (!processToStop.HasExited)
                        {
                            processToStop.Kill();
                            return processToStop.WaitForExit(5000);
                        }
                        
                        return true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Process already exited between checks
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });

                if (success)
                {
                    processToStop.Dispose();
                    OnProcessStatusChanged(processName, false, "Stopped", System.Drawing.Color.Red);
                    return true;
                }
                else
                {
                    MessageBox.Show("Error stopping " + processName + ". You may need to stop it manually via Task Manager.", 
                        "Stop Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping " + processName + ":\n" + ex.Message + "\n\nYou may need to stop it manually via Task Manager.", 
                    "Stop Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        /// <summary>
        /// Checks if a process is running
        /// </summary>
        private bool IsProcessRunning(string processName)
        {
            Process process = GetProcessReference(processName);
            
            if (process != null && !process.HasExited)
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Gets the process reference based on the process name
        /// </summary>
        private Process GetProcessReference(string processName)
        {
            string lowerName = processName.ToLower();
            
            if (lowerName == "apache")
                return _apacheProcess;
            else if (lowerName == "mysql")
                return _mysqlProcess;
            else
                return null;
        }

        /// <summary>
        /// Checks the status of a process and updates the UI
        /// </summary>
        private void CheckProcessStatus(ref Process process, string processName)
        {
            bool isRunning = false;
            string statusText = "Stopped";
            System.Drawing.Color statusColor = System.Drawing.Color.Red;

            try
            {
                if (process != null && !process.HasExited)
                {
                    isRunning = true;
                    statusText = "Running";
                    statusColor = System.Drawing.Color.Green;
                }
                else
                {
                    process = null;
                    isRunning = false;
                    statusText = "Stopped";
                    statusColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception)
            {
                process = null;
                isRunning = false;
                statusText = "Error";
                statusColor = System.Drawing.Color.OrangeRed;
            }

            OnProcessStatusChanged(processName, isRunning, statusText, statusColor);
        }

        /// <summary>
        /// Event handler for process exited event
        /// </summary>
        private void Process_Exited(object sender, EventArgs e, string processName)
        {
            Process exitedProcess = sender as Process;
            if (exitedProcess == null) return;

            // Determine which process exited
            if (processName.Equals("Apache", StringComparison.OrdinalIgnoreCase))
            {
                SafeInvoke(() =>
                {
                    if (_apacheProcess != null)
                        _apacheProcess.Dispose();
                    _apacheProcess = null;
                    OnProcessStatusChanged("Apache", false, "Stopped", System.Drawing.Color.Red);
                });
            }
            else if (processName.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                SafeInvoke(() =>
                {
                    if (_mysqlProcess != null)
                        _mysqlProcess.Dispose();
                    _mysqlProcess = null;
                    OnProcessStatusChanged("MySQL", false, "Stopped", System.Drawing.Color.Red);
                });
            }

            try
            {
                if (exitedProcess != null)
                {
                    exitedProcess.Exited -= (s, args) => Process_Exited(s, args, processName);
                }
            }
            catch { } // Ignore errors if already disposed
        }

        /// <summary>
        /// Safely invokes a method on the UI thread
        /// </summary>
        private void SafeInvoke(Action action)
        {
            if (_parentForm.InvokeRequired)
            {
                _parentForm.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Raises the ProcessStatusChanged event
        /// </summary>
        private void OnProcessStatusChanged(string processName, bool isRunning, string statusText, System.Drawing.Color statusColor)
        {
            if (ProcessStatusChanged != null)
            {
                ProcessStatusChanged(processName, isRunning, statusText, statusColor);
            }
        }
    }
}
