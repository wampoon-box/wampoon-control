using System;
using System.IO;
using System.Windows.Forms;

namespace Frostybee.Models
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
            // Default values
            ApacheExePath = @"C:\xampp\apache\bin\httpd.exe";
            ApacheWorkingDir = @"C:\xampp\apache\bin";
            MySqlExePath = @"C:\xampp\mysql\bin\mysqld.exe";
            MySqlWorkingDir = @"C:\xampp\mysql\bin";
            PhpMyAdminUrl = "http://localhost/phpmyadmin/";
        }
    }
}
