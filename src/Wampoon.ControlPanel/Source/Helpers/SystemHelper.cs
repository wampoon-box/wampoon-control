using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using Wampoon.ControlPanel.Helpers;
using static Wampoon.ControlPanel.Helpers.ErrorLogHelper;

namespace Wampoon.ControlPanel.Helpers
{
    public class SystemHelper
    {

        /// <summary>
        /// Opens a specified URL in the default web browser after validating its format and protocol.
        /// </summary>
        /// <param name="url">The input string representing the web address to be opened in the browser.</param>
        public static void OpenUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("URL is empty or invalid.", "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate URL format and protocol.
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                MessageBox.Show("Invalid URL format. Only HTTP and HTTPS URLs are supported.",
                    "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                MessageBox.Show($"Could not open URL: {uri}\n\nError: {ex.Message}",
                    "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ExecuteFile(string fileName, string parameters, ProcessWindowStyle windowStyle)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = parameters,
                WindowStyle = windowStyle,
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }
        /// <summary>
        /// Gets the current version of the installer assembly.
        /// </summary>
        /// <returns>The version string (e.g., "1.0.0.0") or "Unknown" if version cannot be determined.</returns>
        internal static string GetInstallerVersion()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return version?.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Gets a formatted version string for display purposes.
        /// </summary>
        /// <returns>A formatted version string (e.g., "v1.0.0") or empty string if version cannot be determined.</returns>
        internal static string GetFormattedInstallerVersion()
        {
            var version = GetInstallerVersion();
            if (version == "Unknown")
                return "";

            // Remove the last .0 if it exists for cleaner display (e.g., "1.0.0.0" becomes "v1.0.0")
            if (version.EndsWith(".0"))
                version = version.Substring(0, version.LastIndexOf(".0"));

            return $"v{version}";
        }
    }
}
