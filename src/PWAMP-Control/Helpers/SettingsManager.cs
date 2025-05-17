using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using PwampControl.Models;

namespace PwampControl.Helpers
{
    public class SettingsManager
    {
        private const string SettingsFileName = "settings.json";
        private static readonly string SettingsFilePath = Path.Combine(
            Path.GetDirectoryName(Application.ExecutablePath), 
            SettingsFileName);

        /// <summary>
        /// Loads settings from the JSON file. If the file doesn't exist, returns default settings.
        /// </summary>
        public static Settings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    var serializer = new JavaScriptSerializer();
                    var settings = serializer.Deserialize<Settings>(json);
                    
                    // Validate settings and return defaults if any required setting is missing
                    if (settings != null && 
                        !string.IsNullOrEmpty(settings.ApacheExePath) && 
                        !string.IsNullOrEmpty(settings.MySqlExePath))
                    {
                        return settings;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading settings: " + ex.Message + "\nUsing default settings instead.", 
                    "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Return default settings if file doesn't exist or is invalid
            return new Settings();
        }

        /// <summary>
        /// Saves settings to the JSON file.
        /// </summary>
        public static bool SaveSettings(Settings settings)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(settings);
                File.WriteAllText(SettingsFilePath, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving settings: " + ex.Message, 
                    "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
