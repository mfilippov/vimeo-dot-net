using System;
using System.IO;
using Newtonsoft.Json;

namespace VimeoDotNet.Tests.Settings
{
    internal static class SettingsLoader
    {
        private const string TestPropertiesFile = "test.properties";

        public static VimeoApiTestSettings LoadSettings()
        {
            var fromEnv = GetSettingsFromEnvVars();
            if (!string.IsNullOrEmpty(fromEnv.ClientId))
                return fromEnv;
            
            if (File.Exists(TestPropertiesFile))
            {
                foreach (var line in File.ReadLines(TestPropertiesFile))
                {
                    var parts = line.Trim(' ', '\n', '\r').Split(new [] {'='}, 2);
                    switch (parts[0])
                    {
                        case "VIMEO_CLIENT_ID":
                            fromEnv.ClientId = parts[1].Trim(' ', '\n', '\r');
                            break;
                        case "VIMEO_CLIENT_SECRET":
                            fromEnv.ClientSecret = parts[1].Trim(' ', '\n', '\r');
                            break;
                        case "VIMEO_ACCESS_TOKEN":
                            fromEnv.AccessToken = parts[1].Trim(' ', '\n', '\r');
                            break;
                        case "VIMEO_EMBED_PRESET_ID":
                            fromEnv.EmbedPresetId = long.Parse(parts[1].Trim(' ', '\n', '\r'));
                            break;
                        default:
                            throw new Exception($"Unknown property: '{parts[0]}'");
                    }
                }
            }

            return fromEnv;
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