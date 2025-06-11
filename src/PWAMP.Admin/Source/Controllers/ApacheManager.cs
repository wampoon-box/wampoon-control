using Frostybee.Pwamp.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frostybee.Pwamp.Controllers
{
    internal class ApacheManager : ServerManagerBase
    {
        public override string ServerName { get; set; } = "Apache";
        protected override bool CanMonitorOutput { get; set; } = false;
        

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
            // Allocate 2 seconds delay needed for Apache to start up.
            return 2000;
        }

        protected override ProcessStartInfo GetProcessStartInfo()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(_executablePath)
            {
                // UseShellExecute = false is important for having a direct process handle
                // and potentially for console control, though for GenerateConsoleCtrlEvent,
                // we primarily need the process ID to attach to its console (if it has one).
                UseShellExecute = false, // Set to true on purpose to allow setting WindowStyle to hidden.
                //Arguments = GetStartArguments(),
                //IMPORTANT:
                // CreateNoWindow = true, // This would hide the Apache console window.
                //
                // For Ctrl+C to work as described (stopping Apache by "pressing Control-C
                // in the console window where Apache is running"), Apache needs to be
                // running in a visible console window that it owns.
                // If we set CreateNoWindow = true, sending Ctrl+C might not work as expected
                // because Apache might not set up its console handlers in the same way.
                CreateNoWindow = true, // Apache needs its own console window for Ctrl+C.
                WindowStyle = ProcessWindowStyle.Hidden,
                //RedirectStandardOutput = true,
                //RedirectStandardError = true
            };
            return startInfo;
        }

        protected override async Task<bool> PerformGracefulShutdown()
        {
            if (!IsRunning)
            {
                LogError("process is not running or has already exited.");
                return false;
            }

            try
            {
                // Attach to the console of the Apache process
                if (NativeApi.AttachConsole((uint)_serverProcess.Id))
                {
                    // Disable Ctrl-C handling for our own process temporarily
                    // so we don't inadvertently stop our WinForms app.
                    NativeApi.SetConsoleCtrlHandler(null, true);

                    // Send the CTRL_C_EVENT to the attached console (shared by Apache)
                    // The 0 means it's sent to all processes attached to the console that share
                    // the same CTRL+C signal handler (which httpd.exe running in console mode should).
                    bool ctrlCSent = NativeApi.GenerateConsoleCtrlEvent(NativeApi.CtrlTypes.CTRL_C_EVENT, 0);
                    if (ctrlCSent)
                    {

                        // Wait for a moment for Apache to shut down
                        // You might need to adjust the timeout.
                        _serverProcess.WaitForExit(5000);
                        //var shutdownCompleted = await Task.Run(() => _serverProcess.WaitForExit(5000));
                        await Task.Delay(500);
                        if (_serverProcess.HasExited)
                        {
                            LogMessage("stopped successfully (Ctrl+C sent).");
                            return true;
                        }
                        else
                        {
                            LogError("Ctrl+C signal sent, but Apache has not exited yet. It might be shutting down or requires manual intervention.");
                        }
                    }
                    else
                    {
                        LogError($"Failed to send Ctrl+C signal. Error code: {Marshal.GetLastWin32Error()}");
                        //TODO: Force shutdown if Ctrl+C fails.
                        return false;
                    }

                    // Re-enable Ctrl-C handling for our process if it was disabled.
                    NativeApi.SetConsoleCtrlHandler(null, false);

                    // Detach from the console.
                    NativeApi.FreeConsole();
                }
                else
                {
                    // If Apache was started with CreateNoWindow = true, or if it's a GUI app,
                    // or if it's running as a service, AttachConsole will fail.
                    MessageBox.Show($"Could not attach to Apache's console. Error code: {Marshal.GetLastWin32Error()}. " +
                                    "Ensure Apache was started as a console application from this tool.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping Apache: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsRunning)
                {
                    //FIXME: dispose and create a new process instance?
                    // Or just stop the current process and dispose it upon closing the application?s
                    _serverProcess.Dispose();
                    _serverProcess = null;
                }
                //else if (IsRunning) // Still running
                //{
                //    // Keep stop button enabled if it didn't exit
                //    btnStopApache.Enabled = true;
                //}
                //else // Process was null
                //{
                //    btnStartApache.Enabled = true;
                //    btnStopApache.Enabled = false;
                //}
            }
            return false;
        }

    }
}
