using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwampConsole.Controllers
{
    /// <summary>
    /// Apache server manager implementation
    /// </summary>
    public class ApacheManager : ServerManagerBase
    {
        public ApacheManager(string baseDirectory = null)
            : base(baseDirectory ?? AppDomain.CurrentDomain.BaseDirectory, "Apache")
        {
        }

        protected override void InitializePaths(string baseDirectory)
        {
            // Path to Apache executable (httpd.exe) relative to the application
            _executablePath = Path.Combine(baseDirectory, "apache", "bin", "httpd.exe");

            // Path to Apache config file
            _configPath = Path.Combine(baseDirectory, "apache", "conf", "httpd.conf");
        }

        protected override string GetStartArguments()
        {
            return $"-k start -f \"{_configPath}\"";
        }

        protected override string GetStopArguments()
        {
            return $"-k stop -f \"{_configPath}\"";
        }

        protected override string GetRestartArguments()
        {
            return $"-k restart -f \"{_configPath}\"";
        }

        public override bool UpdateConfiguration()
        {
            try
            {
                string baseDirectory = Path.GetDirectoryName(_executablePath);
                baseDirectory = Directory.GetParent(baseDirectory).FullName; // Go up one level from bin to apache directory
                string currentDirectory = Directory.GetParent(baseDirectory).FullName; // Go up one more level to app directory

                Console.WriteLine($"Updating Apache config with path: {currentDirectory}");

                // Check if the config file exists
                if (!File.Exists(_configPath))
                {
                    Console.WriteLine("Error: Apache config file not found.");
                    return false;
                }

                // Read the config file
                string configContent = File.ReadAllText(_configPath);
                string originalContent = configContent;

                // Replace paths in the config file with the current directory
                // We need to escape backslashes for the regex and config file
                string escapedPath = currentDirectory.Replace("\\", "/");

                // Update ServerRoot directive
                configContent = System.Text.RegularExpressions.Regex.Replace(
                    configContent,
                    @"(ServerRoot\s+)""?([^""]*?)""?(\s|$)",
                    $"$1\"{Path.Combine(escapedPath, "apache").Replace("\\", "/")}\"$3"
                );

                // Update DocumentRoot directive
                configContent = System.Text.RegularExpressions.Regex.Replace(
                    configContent,
                    @"(DocumentRoot\s+)""?([^""]*?)""?(\s|$)",
                    $"$1\"{Path.Combine(escapedPath, "apache/htdocs").Replace("\\", "/")}\"$3"
                );

                // Update <Directory> sections
                configContent = System.Text.RegularExpressions.Regex.Replace(
                    configContent,
                    @"(<Directory\s+)""?([^""]*?)""?(\s*>)",
                    match =>
                    {
                        string directiveStart = match.Groups[1].Value;
                        string currentPath = match.Groups[2].Value;
                        string directiveEnd = match.Groups[3].Value;

                        if (currentPath.StartsWith("${") ||
                            currentPath == "/" ||
                            currentPath.StartsWith("http"))
                        {
                            return match.Value;
                        }

                        string newPath = Path.Combine(escapedPath, "apache/htdocs").Replace("\\", "/");
                        return $"{directiveStart}\"{newPath}\"{directiveEnd}";
                    }
                );

                // Check if any changes were made
                if (originalContent == configContent)
                {
                    Console.WriteLine("No changes needed to Apache config file.");
                }
                else
                {
                    // Write the updated config back to the file
                    File.WriteAllText(_configPath, configContent);
                    Console.WriteLine("Successfully updated Apache config file with current paths.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Apache config: {ex.Message}");
                return false;
            }
        }
    }

}
