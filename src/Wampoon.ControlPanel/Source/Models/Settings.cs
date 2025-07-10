using System;
using System.IO;
using System.Windows.Forms;
using Wampoon.ControlPanel.Helpers;

namespace Wampoon.ControlPanel.Models
{
    public class Settings
    {
        // Apache settings
        public string ApacheExePath { get; set; }
        public string ApacheWorkingDir { get; set; }
        
        // MySQL settings
        public string MySqlExePath { get; set; }
        public string MySqlWorkingDir { get; set; }
        
        // phpMyAdmin settings
        public string PhpMyAdminUrl { get; set; }

        public Settings()
        {
            // Default values using AppConstants
            ApacheExePath = Path.Combine(AppConstants.DefaultPaths.APACHE_BIN_PATH, AppConstants.FileNames.APACHE_EXECUTABLE);
            ApacheWorkingDir = AppConstants.DefaultPaths.APACHE_BIN_PATH;
            MySqlExePath = Path.Combine(AppConstants.DefaultPaths.MYSQL_BIN_PATH, AppConstants.FileNames.MYSQL_EXECUTABLE);
            MySqlWorkingDir = AppConstants.DefaultPaths.MYSQL_BIN_PATH;
            PhpMyAdminUrl = AppConstants.Urls.PHPMYADMIN_URL;
        }
    }
}
