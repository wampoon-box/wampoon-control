using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Frostybee.Helpers
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
    }
}
