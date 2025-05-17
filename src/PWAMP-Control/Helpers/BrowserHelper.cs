using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PwampControl.Helpers
{
    public class BrowserHelper
    {
        /// <summary>
        /// Opens a URL in the default browser
        /// </summary>
        public static void OpenUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("URL is empty or invalid.", "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open URL: " + url + "\n\nError: " + ex.Message, 
                    "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
