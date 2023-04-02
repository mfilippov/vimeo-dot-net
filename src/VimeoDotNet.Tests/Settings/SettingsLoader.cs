using System;
using System.IO;
using Newtonsoft.Json;

namespace VimeoDotNet.Tests.Settings
{
    internal static class SettingsLoader
    {
        public static VimeoApiTestSettings LoadSettings()
        {
            var fromEnv = GetSettingsFromEnvVars();
            return !string.IsNullOrEmpty(fromEnv.AccessToken) ? fromEnv : new VimeoApiTestSettings();
        }

        private static VimeoApiTestSettings GetSettingsFromEnvVars()
        {
            long.TryParse(Environment.GetEnvironmentVariable("VIMEO_EMBED_PRESET_ID"), out var embedPresetId);
            return new VimeoApiTestSettings
            {
                ClientId = Environment.GetEnvironmentVariable("VIMEO_CLIENT_ID"),
                ClientSecret = Environment.GetEnvironmentVariable("VIMEO_CLIENT_SECRET"),
                AccessToken = Environment.GetEnvironmentVariable("VIMEO_ACCESS_TOKEN"),
                EmbedPresetId = embedPresetId
            };
        }
    }
}