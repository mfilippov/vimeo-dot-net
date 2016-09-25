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
            var fromEnv = GetSettingsFromEnvVars();
            if (fromEnv.UserId != 0)
                return fromEnv;
            var json = File.ReadAllText(SETTINGS_FILE);
            return JsonConvert.DeserializeObject<VimeoApiTestSettings>(json);
        }

        private static VimeoApiTestSettings GetSettingsFromEnvVars()
        {
            long userId;
            long.TryParse(Environment.GetEnvironmentVariable("UserId"), out userId);
            long albumId;
            long.TryParse(Environment.GetEnvironmentVariable("AlbumId"), out albumId);
            long channelId;
            long.TryParse(Environment.GetEnvironmentVariable("ChannelId"), out channelId);
            long videoId;
            long.TryParse(Environment.GetEnvironmentVariable("VideoId"), out videoId);
            return new VimeoApiTestSettings()
            {
                ClientId = Environment.GetEnvironmentVariable("ClientId"),
                ClientSecret = Environment.GetEnvironmentVariable("ClientSecret"),
                AccessToken = Environment.GetEnvironmentVariable("AccessToken"),
                UserId = userId,
                AlbumId = albumId,
                ChannelId = channelId,
                VideoId = videoId,
            };
        }

        public static void SaveSettings(VimeoApiTestSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            System.IO.File.WriteAllText(SETTINGS_FILE, json);
        }
    }
}
