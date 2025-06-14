using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.PwampAdmin.Helpers
{

    internal static class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static readonly string LogFilePath = Path.Combine(LogDirectory, "error.log");

        static Logger()
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }
            }
            catch
            {
                // Silently fail if we can't create the log directory.
            }
        }

        internal static void LogError(Exception ex)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}";
            WriteToFile(logEntry);
        }

        internal static void LogError(string message)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}{Environment.NewLine}";
            WriteToFile(logEntry);
        }

        private static void WriteToFile(string message)
        {
            try
            {
                lock (_lock)
                {
                    // Check if rotation is needed.
                    if (ShouldRotateLog())
                    {
                        RotateLogFile();
                    }

                    using (var stream = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(message);
                    }
                }
            }
            catch
            {
                // Silently fail to prevent logger from crashing the application.
            }
        }

        private static bool ShouldRotateLog()
        {
            try
            {
                if (!File.Exists(LogFilePath))
                    return false;

                var fileInfo = new FileInfo(LogFilePath);
                var fileAge = DateTime.Now - fileInfo.CreationTime;
                // 2 months (approximately).
                return fileAge.TotalDays >= 60; 
            }
            catch
            {
                return false;
            }
        }

        private static void RotateLogFile()
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMM");
                var rotatedFileName = $"error_{timestamp}.log";
                var rotatedFilePath = Path.Combine(LogDirectory, rotatedFileName);

                File.Move(LogFilePath, rotatedFilePath);
            }
            catch
            {
                // Silently fail. This is to ensure that the logger does not crash the application if rotation fails.
            }
        }
    }
}
