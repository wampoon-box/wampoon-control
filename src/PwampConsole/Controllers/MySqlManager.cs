using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwampConsole.Controllers
{
    /// <summary>
    /// MySQL server manager implementation
    /// </summary>
    public class MySqlManager : ServerManagerBase
    {
        private string _dataDirectory;
        private string _defaultsFilePath;

        public MySqlManager(string baseDirectory = null)
            : base(baseDirectory ?? AppDomain.CurrentDomain.BaseDirectory, "MySQL")
        {
        }

        protected override void InitializePaths(string baseDirectory)
        {
            // Path to MySQL executable relative to the application
            _executablePath = Path.Combine(baseDirectory, "mysql", "bin", "mysqld.exe");

            // Path to MySQL data directory
            _dataDirectory = Path.Combine(baseDirectory, "mysql", "data");

            // Path to MySQL configuration file (my.ini)
            _configPath = Path.Combine(baseDirectory, "mysql", "my.ini");

            // Create a defaults file path if needed
            _defaultsFilePath = Path.Combine(baseDirectory, "mysql", "my-defaults.ini");
        }

        protected override string GetStartArguments()
        {
            return $"--defaults-file=\"{_configPath}\" --console";
        }

        protected override string GetStopArguments()
        {
            // MySQL doesn't have a direct stop argument, typically done via mysqladmin
            // We'll handle this differently in StopServer
            return "";
        }

        protected override string GetRestartArguments()
        {
            // MySQL doesn't have a direct restart command
            return "";
        }

        public override bool StopServer()
        {
            if (!_isRunning)
            {
                Console.WriteLine("MySQL is not running.");
                return true;
            }

            try
            {
                // Path to mysqladmin executable
                string mysqlAdminPath = Path.Combine(
                    Path.GetDirectoryName(_executablePath),
                    "mysqladmin.exe"
                );

                if (File.Exists(mysqlAdminPath))
                {
                    // Use mysqladmin to shutdown the server gracefully
                    ProcessStartInfo stopInfo = new ProcessStartInfo
                    {
                        FileName = mysqlAdminPath,
                        // Adjust these parameters as needed for your MySQL setup
                        Arguments = $"-u root shutdown",
                        UseShellExecute = false,
                        CreateNoWindow = false
                    };

                    using (Process stopProcess = new Process { StartInfo = stopInfo })
                    {
                        stopProcess.Start();
                        stopProcess.WaitForExit(10000); // Wait up to 10 seconds
                    }

                    // Give MySQL a moment to fully shut down
                    Task.Delay(2000).Wait();
                }

                // If server is still running, try to terminate the process
                if (!_serverProcess.HasExited)
                {
                    Console.WriteLine("MySQL did not exit gracefully. Trying to terminate the process.");
                    _serverProcess.Kill();
                }

                _isRunning = false;
                Console.WriteLine("MySQL server stopped.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping MySQL: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Initialize the MySQL database if needed
        /// </summary>
        public bool InitializeDatabase()
        {
            try
            {
                // Check if data directory is empty
                if (!Directory.EnumerateFileSystemEntries(_dataDirectory).Any())
                {
                    Console.WriteLine("Initializing MySQL database...");

                    // Run mysqld with --initialize option
                    ProcessStartInfo initInfo = new ProcessStartInfo
                    {
                        FileName = _executablePath,
                        Arguments = $"--initialize-insecure --basedir=\"{Path.GetDirectoryName(Path.GetDirectoryName(_executablePath))}\" --datadir=\"{_dataDirectory}\"",
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    using (Process initProcess = new Process { StartInfo = initInfo })
                    {
                        initProcess.Start();

                        string output = initProcess.StandardOutput.ReadToEnd();
                        string error = initProcess.StandardError.ReadToEnd();

                        initProcess.WaitForExit();

                        if (!string.IsNullOrEmpty(output))
                            Console.WriteLine(output);

                        if (!string.IsNullOrEmpty(error))
                            Console.WriteLine(error);

                        if (initProcess.ExitCode != 0)
                        {
                            Console.WriteLine($"MySQL initialization failed with exit code: {initProcess.ExitCode}");
                            return false;
                        }
                    }

                    Console.WriteLine("MySQL database initialized successfully.");
                }
                else
                {
                    Console.WriteLine("MySQL data directory already contains files, skipping initialization.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing MySQL database: {ex.Message}");
                return false;
            }
        }
    }
}
