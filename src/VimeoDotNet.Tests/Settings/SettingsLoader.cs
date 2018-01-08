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

                throw new Exception(
                    $"The file {SettingsFile} was not found. A file was created, please fill in the information");
            }

            var json = File.ReadAllText(SettingsFile);
            return JsonConvert.DeserializeObject<VimeoApiTestSettings>(json);
        }

        private static VimeoApiTestSettings GetSettingsFromEnvVars()
        {
            long.TryParse(Environment.GetEnvironmentVariable("UserId"), out var userId);
            long.TryParse(Environment.GetEnvironmentVariable("AlbumId"), out var albumId);
            long.TryParse(Environment.GetEnvironmentVariable("VideoId"), out var videoId);
            long.TryParse(Environment.GetEnvironmentVariable("TextTrackId"), out var textTrackId);
            long.TryParse(Environment.GetEnvironmentVariable("PublicUserId"), out var publicUserId);
            return new VimeoApiTestSettings
            {
                ClientId = Environment.GetEnvironmentVariable("ClientId"),
                ClientSecret = Environment.GetEnvironmentVariable("ClientSecret"),
                AccessToken = Environment.GetEnvironmentVariable("AccessToken"),
                UserId = userId,
                AlbumId = albumId,
                VideoId = videoId,
                TextTrackId = textTrackId,
                PublicUserId = publicUserId
            };
        }

        private static void SaveSettings(VimeoApiTestSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(SettingsFile, json);
        }
    }
}