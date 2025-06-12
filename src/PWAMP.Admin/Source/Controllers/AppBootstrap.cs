using System;
using System.IO;
using System.Reflection;
using System.Text;
using Frostybee.PwampAdmin.Controllers;
using Frostybee.PwampAdmin.Helpers;

namespace Frostybee.PwampAdmin.Controllers
{
    /// <summary>
    /// Handles Apache bootstrap configuration by creating custom path definitions
    /// and starting the Apache server with the proper configuration.
    /// </summary>
    public class AppBootstrap
    {
        private string _currentDirectory;
        private string _apacheDirectory;
        private string _documentRoot;
        private string _customConfigPath;
        private string _phpMyAdminDirectory;
        private string _httpdAliasConfigPath;

        public AppBootstrap()
        {
            ConfigureApache();
        }

        private void ConfigureApache()
        {
            // Get the directory where the application executable is located.
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            if (string.IsNullOrEmpty(assemblyLocation))
            {
                // Fallback to current directory if assembly location is not available.
                _currentDirectory = Directory.GetCurrentDirectory();
            }
            else
            {
                _currentDirectory = Path.GetDirectoryName(assemblyLocation);
            }

            // Validate that we have a valid directory.
            {
                throw new InvalidOperationException("Unable to determine application directory");
            }

            // Use ServerPathManager to get Apache paths.
            _apacheDirectory = ServerPathManager.GetServerBaseDirectory("Apache");
            _documentRoot = ServerPathManager.GetSpecialPath("Apache", "DocumentRoot");

            // If ServerPathManager doesn't return paths, fall back to manual construction.
            if (string.IsNullOrEmpty(_apacheDirectory))
            {
                _apacheDirectory = Path.Combine(_currentDirectory, "apps", "apache");
            }

            if (string.IsNullOrEmpty(_documentRoot))
            {
                _documentRoot = Path.Combine(_currentDirectory, "htdocs");
            }

            // Set up PhpMyAdmin path (assuming it's in apps/phpmyadmin).
            _phpMyAdminDirectory = Path.Combine(_currentDirectory, "apps", "phpmyadmin");

            _customConfigPath = Path.Combine(_apacheDirectory, "conf", "custom_path.conf");
            _httpdAliasConfigPath = Path.Combine(_apacheDirectory, "conf", "httpd-alias.conf");
        }

        /// <summary>
        /// Creates the custom Apache configuration file with path definitions.
        /// </summary>
        /// <returns>True if the configuration file was created successfully, false otherwise.</returns>
        public bool CreateCustomConfiguration()
        {
            try
            {
                // Ensure the conf directory exists.
                var confDirectory = Path.GetDirectoryName(_customConfigPath);
                if (!Directory.Exists(confDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(confDirectory);
                    }
                    catch (IOException)
                    {
                        // Directory might have been created by another thread, check again.
                        if (!Directory.Exists(confDirectory))
                        {
                            throw;
                        }
                    }
                }

                // Create the custom configuration content.
                var configContent = new StringBuilder();
                configContent.AppendLine($"Define SRVROOT \"{_apacheDirectory}\"");
                configContent.AppendLine($"Define DOCROOT \"{_documentRoot}\"");

                // Write the configuration to the file.
                File.WriteAllText(_customConfigPath, configContent.ToString(), Encoding.UTF8);

                // Also update the httpd-alias.conf file.
                if (!UpdateHttpdAliasConfiguration())
                {
                    System.Diagnostics.Debug.WriteLine("Warning: Failed to update httpd-alias.conf");
                    // Don't fail the entire operation if alias config update fails.
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the error (you might want to use your logging mechanism here).
                System.Diagnostics.Debug.WriteLine($"Error creating custom Apache configuration: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if the custom configuration file exists.
        /// </summary>
        public bool CustomConfigExists => File.Exists(_customConfigPath);

        /// <summary>
        /// Checks if the httpd-alias.conf file exists.
        /// </summary>
        public bool HttpdAliasConfigExists => File.Exists(_httpdAliasConfigPath);

        /// <summary>
        /// Updates only the httpd-alias.conf file without affecting other configurations.
        /// </summary>
        /// <returns>True if the file was updated successfully, false otherwise.</returns>
        public bool UpdateHttpdAliasConfigurationOnly()
        {
            return UpdateHttpdAliasConfiguration();
        }

        /// <summary>
        /// Gets the current PMAROOT path from the httpd-alias.conf file.
        /// </summary>
        /// <returns>The current PMAROOT path, or null if not found.</returns>
        public string GetCurrentPmaRootPath()
        {
            try
            {
                if (!File.Exists(_httpdAliasConfigPath))
                {
                    return null;
                }

                var lines = File.ReadAllLines(_httpdAliasConfigPath);
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith("Define PMAROOT", StringComparison.OrdinalIgnoreCase))
                    {
                        // Extract the path from the Define statement
                        var parts = trimmedLine.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            return parts[1]; // The path is between quotes
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading PMAROOT path: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Updates the httpd-alias.conf file to use the correct PMAROOT path.
        /// </summary>
        /// <returns>True if the file was updated successfully, false otherwise.</returns>
        private bool UpdateHttpdAliasConfiguration()
        {
            try
            {
                if (!File.Exists(_httpdAliasConfigPath))
                {
                    System.Diagnostics.Debug.WriteLine($"httpd-alias.conf file not found: {_httpdAliasConfigPath}");
                    return false;
                }

                // Read all lines from the file
                var lines = File.ReadAllLines(_httpdAliasConfigPath);
                bool updated = false;

                // Update the first line if it contains the PMAROOT Define statement
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    if (line.StartsWith("Define PMAROOT", StringComparison.OrdinalIgnoreCase))
                    {
                        // Replace the hardcoded path with the relative path
                        lines[i] = $"Define PMAROOT \"{_phpMyAdminDirectory}\"";
                        updated = true;
                        System.Diagnostics.Debug.WriteLine($"Updated PMAROOT path to: {_phpMyAdminDirectory}");
                        break; // Only update the first occurrence
                    }
                }

                if (updated)
                {
                    // Write the updated content back to the file
                    File.WriteAllLines(_httpdAliasConfigPath, lines, Encoding.UTF8);
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No PMAROOT Define statement found in httpd-alias.conf");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating httpd-alias.conf: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validates that all required directories and files exist for Apache to run.
        /// </summary>
        /// <returns>True if all requirements are met, false otherwise.</returns>
        public bool ValidateApacheEnvironment()
        {
            try
            {
                // Use ServerPathManager to check if Apache is available
                if (!ServerPathManager.IsServerAvailable("Apache"))
                {
                    System.Diagnostics.Debug.WriteLine("Apache is not available according to ServerPathManager");
                    return false;
                }

                // Check if Apache directory exists
                if (!Directory.Exists(_apacheDirectory))
                {
                    System.Diagnostics.Debug.WriteLine($"Apache directory not found: {_apacheDirectory}");
                    return false;
                }

                // Use ServerPathManager to get and validate Apache executable
                var apacheExecutablePath = ServerPathManager.GetExecutablePath("Apache");
                if (string.IsNullOrEmpty(apacheExecutablePath) || !File.Exists(apacheExecutablePath))
                {
                    System.Diagnostics.Debug.WriteLine($"Apache executable not found: {apacheExecutablePath}");
                    return false;
                }

                // Use ServerPathManager to get and validate Apache configuration
                var apacheConfigPath = ServerPathManager.GetConfigPath("Apache");
                if (string.IsNullOrEmpty(apacheConfigPath) || !File.Exists(apacheConfigPath))
                {
                    System.Diagnostics.Debug.WriteLine($"Apache configuration file not found: {apacheConfigPath}");
                    return false;
                }

                // Check if document root directory exists, create if it doesn't
                if (!Directory.Exists(_documentRoot))
                {
                    try
                    {
                        Directory.CreateDirectory(_documentRoot);
                        System.Diagnostics.Debug.WriteLine($"Created document root directory: {_documentRoot}");
                    }
                    catch (IOException ioEx)
                    {
                        // Directory might have been created by another thread, check again
                        if (!Directory.Exists(_documentRoot))
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to create document root directory: {ioEx.Message}");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to create document root directory: {ex.Message}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error validating Apache environment: {ex.Message}");
                return false;
            }
        }
    }
}
