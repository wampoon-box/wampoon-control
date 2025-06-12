using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frostybee.PwampAdmin
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Set up global exception handlers before running the application.
            SetupGlobalExceptionHandlers();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
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
            LogException(e.Exception, "UI Thread Exception");

            DialogResult result = MessageBox.Show(
                $"An unexpected error occurred:\n\n{e.Exception.Message}\n\nDo you want to continue?",
                "Application Error",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error);

            if (result == DialogResult.No)
            {
                Application.Exit();
            }
        }

        // Handles exceptions on non-UI threads.
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            LogException(ex, "Non-UI Thread Exception");

            string message = $"A fatal error occurred:\n\n{ex.Message}\n\nThe application will now close.";
            MessageBox.Show(message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            // Application is terminating, perform cleanup if needed.
            Environment.Exit(1);
        }

        // Handles unobserved Task exceptions.
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogException(e.Exception, "Unobserved Task Exception");

            // Mark as observed to prevent application termination.
            e.SetObserved();

            MessageBox.Show(
                $"An error occurred in a background task:\n\n{e.Exception.GetBaseException().Message}",
                "Background Task Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private static void LogException(Exception ex, string source)
        {
            try
            {
                string logPath = Path.Combine(Application.StartupPath, "ErrorLog.txt");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {source}\n" +
                                 $"Exception: {ex.GetType().Name}\n" +
                                 $"Message: {ex.Message}\n" +
                                 $"Stack Trace:\n{ex.StackTrace}\n" +
                                 new string('-', 80) + "\n";

                File.AppendAllText(logPath, logEntry);
            }
            catch
            {
                // If logging fails, we don't want to throw another exception.
            }
        }
    }
}

