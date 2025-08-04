using System;

namespace Wampoon.ControlPanel.Helpers
{
    internal static class AppConstants
    {
        public const string APP_NAME = "Wampoon Control Panel";                
        public const string GITHUB_REPO_URI = "https://github.com/wampoon-box/wampoon-control";
        public const string APP_LOG_FOLDER = "wampoon-logs";
        public const string APP_LOG_FILE = "wampoon-error.log";
        
        // Log file names.
        public const string APACHE_ERROR_LOG = "apache_error.log";
        public const string APACHE_ACCESS_LOG = "apache_access.log";
        public const string APACHE_DIAGNOSTICS_LOG = "apache-diagnostics.log";
        public const string PHP_ERROR_LOG = "php_error.log";
        public const string MARIADB_ERROR_LOG = "mariadb_error.log";

        // Network Configuration.
        public static class Ports
        {
            public const int APACHE_DEFAULT = 80;
            public const int MYSQL_DEFAULT = 3306;
            public const int HTTPS_DEFAULT = 443;
        }

        // Timeout and Delay Values.
        public static class Timeouts
        {
            public const int APACHE_STARTUP_DELAY_MS = 2000;
            public const int MYSQL_STARTUP_DELAY_MS = 3000;
            public const int PROCESS_WAIT_TIMEOUT_MS = 5000;
            public const int GRACEFUL_SHUTDOWN_DELAY_MS = 3000;
        }

        // File Extensions and Names.
        public static class FileNames
        {
            public const string APACHE_EXECUTABLE = "httpd.exe";
            public const string MYSQL_EXECUTABLE = "mariadbd.exe";
            public const string APACHE_CONFIG = "httpd.conf";
            public const string MYSQL_CONFIG = "my.ini";
            public const string PHPMYADMIN_CONFIG = "config.inc.php";
            public const string EXECUTABLE_EXTENSION = ".exe";
        }

        // URL Templates..
        public static class Urls
        {
            public const string LOCALHOST_HTTP = "http://localhost";
            public const string PHPMYADMIN_PATH = "/phpmyadmin";
            public const string PHPMYADMIN_URL = LOCALHOST_HTTP + PHPMYADMIN_PATH;
            public const string HTTP_PROTOCOL = "http://";
            public const string HTTPS_PROTOCOL = "https://";
        }

        // Directory Names.
        public static class Directories
        {
            public const string APACHE_CONF = "conf";
            public const string APACHE_BIN = "bin";
            public const string APACHE_LOGS = "logs";
            public const string MYSQL_DATA = "data";
            public const string CUSTOM_CONFIG_NAME = "httpd-wampoon-variables.conf";
        }
        
        // UI Constants
        public static class UI
        {
            public const int SHADOW_OFFSET = 3;
            public const int SHADOW_SIZE = 6;
            public const int BORDER_RADIUS = 8;
            public const int BORDER_WIDTH = 12;
            public const int BUTTON_BORDER_RADIUS = 12;
        }

        // Default Paths.
        public static class DefaultPaths
        {
            public const string PAMPP_ROOT = @"C:\wampoon\apps";
            public const string APACHE_BASE = PAMPP_ROOT + @"\apache";
            public const string MYSQL_BASE = PAMPP_ROOT + @"\mysql";
            public const string APACHE_BIN_PATH = APACHE_BASE + @"\bin";
            public const string MYSQL_BIN_PATH = MYSQL_BASE + @"\bin";
        }

        // Message Templates.
        public static class Messages
        {
            public const string PORT_IN_USE = "Port {0} is in use. {1} server cannot be started.";
            public const string ERROR_STARTING = "Error starting {0}: {1}";
            public const string FAILED_TO_OPERATION = "Failed to {0}: {1}";
            public const string EXECUTABLE_NOT_FOUND = "Executable not found: {0}";
            public const string CONFIG_NOT_FOUND = "Configuration file not found: {0}";
            public const string SERVER_MANAGER_REINITIALIZED = "Server manager reinitialized successfully.";
            public const string SERVER_MANAGER_NOT_INITIALIZED = "Server manager is not initialized. Attempting to reinitialize...";
        }
    }
}
