using System;
using System.IO;
using System.Linq;

namespace PwampConsole.Controllers
{
    /// <summary>
    /// Backup of configuration methods from ApacheManager and MySqlManager
    /// </summary>
    public static class ConfigurationBackup
    {
        public static bool UpdateApacheConfiguration(string executablePath, string configPath)
        {
            try
            {
                string baseDirectory = Path.GetDirectoryName(executablePath);
                baseDirectory = Directory.GetParent(baseDirectory).FullName; // Go up one level from bin to apache directory
                string currentDirectory = Directory.GetParent(baseDirectory).FullName; // Go up one more level to app directory

                Console.WriteLine($"Updating Apache config with path: {currentDirectory}");

                // Check if the config file exists
                if (!File.Exists(configPath))
                {
                    Console.WriteLine("Error: Apache config file not found.");
                    return false;
                }

                // Read the config file
                string configContent = File.ReadAllText(configPath);
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
                    File.WriteAllText(configPath, configContent);
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

        public static bool UpdateMySqlConfiguration(string executablePath, string configPath, string dataDirectory)
        {
            try
            {
                string baseDirectory = Path.GetDirectoryName(executablePath);
                baseDirectory = Directory.GetParent(baseDirectory).FullName; // Go up one level from bin to mysql directory
                string currentDirectory = Directory.GetParent(baseDirectory).FullName; // Go up one more level to app directory

                Console.WriteLine($"Updating MySQL config with path: {currentDirectory}");

                // Check if the config file exists, if not create it
                if (!File.Exists(configPath))
                {
                    // Create a basic MySQL config file
                    string mysqlConfig =
$@"[mysqld]
# Basic settings
basedir={currentDirectory.Replace("\\", "/")}/mysql
datadir={currentDirectory.Replace("\\", "/")}/mysql/data
port=3306
# Allow connections from any IP
bind-address=0.0.0.0

[client]
port=3306
";

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(configPath));

                    // Write the new config
                    File.WriteAllText(configPath, mysqlConfig);
                    Console.WriteLine("Created new MySQL configuration file.");
                }
                else
                {
                    // Read existing config
                    string configContent = File.ReadAllText(configPath);
                    string originalContent = configContent;

                    // Update basedir path
                    configContent = System.Text.RegularExpressions.Regex.Replace(
                        configContent,
                        @"(basedir\s*=\s*).*",
                        $"$1{currentDirectory.Replace("\\", "/")}/mysql"
                    );

                    // Update datadir path
                    configContent = System.Text.RegularExpressions.Regex.Replace(
                        configContent,
                        @"(datadir\s*=\s*).*",
                        $"$1{currentDirectory.Replace("\\", "/")}/mysql/data"
                    );

                    // Check if any changes were made
                    if (originalContent == configContent)
                    {
                        Console.WriteLine("No changes needed to MySQL config file.");
                    }
                    else
                    {
                        // Write the updated config back to the file
                        File.WriteAllText(configPath, configContent);
                        Console.WriteLine("Successfully updated MySQL config file with current paths.");
                    }
                }

                // Ensure the data directory exists
                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                    Console.WriteLine("Created MySQL data directory.");

                    // Check if MySQL needs initialization
                    bool needsInit = !Directory.EnumerateFileSystemEntries(dataDirectory).Any();
                    if (needsInit)
                    {
                        Console.WriteLine("MySQL data directory is empty, database initialization may be required.");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating MySQL config: {ex.Message}");
                return false;
            }
        }
    }
} 