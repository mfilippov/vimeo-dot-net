using System;
using System.IO;
using Newtonsoft.Json;

namespace VimeoDotNet.Tests.Settings
{
    internal static class SettingsLoader
    {
        private const string SettingsFile = "vimeoSettings.json";

        public static VimeoApiTestSettings LoadSettings()
        {
            var fromEnv = GetSettingsFromEnvVars();
            if (fromEnv.UserId != 0)
                return fromEnv;
            if (!File.Exists(SettingsFile))
            {
                // File was not found so create a new one with blanks 
                SaveSettings(new VimeoApiTestSettings());

                throw new Exception(string.Format(
                    "The file {0} was not found. A file was created, please fill in the information", SettingsFile));
            }

            var json = File.ReadAllText(SettingsFile);
            return JsonConvert.DeserializeObject<VimeoApiTestSettings>(json);
        }

        private static VimeoApiTestSettings GetSettingsFromEnvVars()
        {
            long.TryParse(Environment.GetEnvironmentVariable("UserId"), out var userId);
            long.TryParse(Environment.GetEnvironmentVariable("AlbumId"), out var albumId);
            long.TryParse(Environment.GetEnvironmentVariable("ChannelId"), out var channelId);
            long.TryParse(Environment.GetEnvironmentVariable("VideoId"), out var videoId);
            long.TryParse(Environment.GetEnvironmentVariable("TextTrackId"), out var textTrackId);
            return new VimeoApiTestSettings
            {
                ClientId = Environment.GetEnvironmentVariable("ClientId"),
                ClientSecret = Environment.GetEnvironmentVariable("ClientSecret"),
                AccessToken = Environment.GetEnvironmentVariable("AccessToken"),
                UserId = userId,
                AlbumId = albumId,
                ChannelId = channelId,
                VideoId = videoId,
                TextTrackId = textTrackId
            };
        }

        private static void SaveSettings(VimeoApiTestSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(SettingsFile, json);
        }
    }
}