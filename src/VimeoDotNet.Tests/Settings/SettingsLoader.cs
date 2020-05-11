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
                        case "ClientId":
                            fromEnv.ClientId = parts[1].Trim(' ', '\n', '\r');
                            break;
                        case "ClientSecret":
                            fromEnv.ClientSecret = parts[1].Trim(' ', '\n', '\r');
                            break;
                        case "AccessToken":
                            fromEnv.AccessToken = parts[1].Trim(' ', '\n', '\r');
                            break;
                        case "EmbedPresetId":
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
            long.TryParse(Environment.GetEnvironmentVariable("EmbedPresetId"), out var embedPresetId);
            return new VimeoApiTestSettings
            {
                ClientId = Environment.GetEnvironmentVariable("ClientId"),
                ClientSecret = Environment.GetEnvironmentVariable("ClientSecret"),
                AccessToken = Environment.GetEnvironmentVariable("AccessToken"),
                EmbedPresetId = embedPresetId
            };
        }
    }
}