using System;
using System.IO;
using Newtonsoft.Json; 

namespace VimeoDotNet.Tests.Settings
{
    internal class SettingsLoader
    {
        private const string SETTINGS_FILE = "vimeoSettings.json";

        public static VimeoApiTestSettings LoadSettings()
        {
            if (!File.Exists(SETTINGS_FILE))
            {
                // File was not found so create a new one with blanks 
                SaveSettings(new VimeoApiTestSettings());

                throw new Exception(string.Format("The file {0} was not found. A file was created, please fill in the information", SETTINGS_FILE));
            }

            var json = File.ReadAllText(SETTINGS_FILE);
            return JsonConvert.DeserializeObject<VimeoApiTestSettings>(json);
        }

        public static void SaveSettings(VimeoApiTestSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            System.IO.File.WriteAllText(SETTINGS_FILE, json);
        }
    }
}
