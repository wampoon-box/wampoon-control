using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frostybee.Pwamp.Helpers;
using Frostybee.Pwamp.UI;

namespace Frostybee.Pwamp
{
    internal static class Program
    {

        private static Mutex mutex = null;        

        // Custom message identifier.
        public static readonly int WM_SHOW_RUNNING_INSTANCE = NativeApi.RegisterWindowMessage("WM_SHOW_RUNNING_INSTANCE_PWAMP_Admin");

        [STAThread]
        static void Main()
        {
            bool createdNew = false;
            const string mutexName = "PWAMP_ADMIN_608CB914-44C3-4329-9E1F-3C44C9610BB9";

            // Set up global exception handlers before running the application.
            SetupGlobalExceptionHandlers();
            
            mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                // Another instance is running, send message to show it.
                ShowFirstInstance();
                return;
            }

            try
            {
                // No other instance is running, proceed with application startup.
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            finally
            {
                mutex?.ReleaseMutex();
                mutex?.Dispose();
            }
        }

        private static void ShowFirstInstance()
        {
            NativeApi.EnumWindows((hWnd, lParam) =>
            {
                uint processId;
                NativeApi.GetWindowThreadProcessId(hWnd, out processId);

                // Check if this window belongs to our application.
                if (processId != 0)
                {
                    try
                    {
                        var process = System.Diagnostics.Process.GetProcessById((int)processId);
                        if (process.ProcessName.Equals(System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            // We found the app's running process, send our custom message.
                            NativeApi.PostMessage(hWnd, WM_SHOW_RUNNING_INSTANCE, IntPtr.Zero, IntPtr.Zero);
                        }
                    }
                    catch { /* Ignore if process no longer exists */ }
                }
                // Continue enumeration.
                return true; 
            }, IntPtr.Zero);
        }


        private static void SetupGlobalExceptionHandlers()
        {
            // Handle UI thread exceptions.
            Application.ThreadException += Application_ThreadException;

            // Handle non-UI thread exceptions.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Handle Task exceptions (for async/await scenarios).
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // Set the unhandled exception mode.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        }

        // Handles exceptions on the UI thread.
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                ErrorLogHelper.ShowErrorReport(e.Exception, "UI Thread Exception");
                
                DialogResult result = MessageBox.Show(
                    "An unexpected error occurred. Do you want to continue running the application?",
                    "Application Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            catch
            {
                // Fallback if error reporting fails.
                MessageBox.Show($"A critical error occurred: {e.Exception.Message}", "Fatal Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        // Handles exceptions on non-UI threads.
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            
            try
            {
                ErrorLogHelper.ShowErrorReport(ex, "Non-UI Thread Exception - Application will close");
            }
            catch
            {
                // Fallback if error reporting fails.
                MessageBox.Show($"A fatal error occurred: {ex.Message}\n\nThe application will now close.", 
                    "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            // Application is terminating, perform cleanup if needed.
            Environment.Exit(1);
        }

        // Handles unobserved Task exceptions.
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Mark as observed to prevent application termination.
            e.SetObserved();

            try
            {
                ErrorLogHelper.ShowErrorReport(e.Exception.GetBaseException(), "Unobserved Task Exception");
            }
            catch
            {
                // Fallback if error reporting fails.
                MessageBox.Show($"An error occurred in a background task: {e.Exception.GetBaseException().Message}",
                    "Background Task Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }       
    }
}

