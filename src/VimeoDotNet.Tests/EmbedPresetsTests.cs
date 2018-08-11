using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class EmbedPresetsTests : BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyRetrieveMyEmbedPresetById()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();
            var preset = await client.GetEmbedPresetAsync(UserId.Me, VimeoSettings.EmbedPresetId);
            preset.ShouldNotBeNull();
            preset.Id.ShouldBe(VimeoSettings.EmbedPresetId);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveUserEmbedPresetById()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();
            var preset = await client.GetEmbedPresetAsync(VimeoSettings.UserId, VimeoSettings.EmbedPresetId);
            preset.ShouldNotBeNull();
            preset.Id.ShouldBe(VimeoSettings.EmbedPresetId);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveMyEmbedPresets()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();
            var presets = await client.GetEmbedPresetsAsync(UserId.Me);
            presets.ShouldNotBeNull();
            presets.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveUserEmbedPresets()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();
            var presets = await client.GetEmbedPresetsAsync(VimeoSettings.UserId);
            presets.ShouldNotBeNull();
            presets.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGetEmbedPresetWithFields()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();
            var preset = await client.GetEmbedPresetAsync(UserId.Me, VimeoSettings.EmbedPresetId, new[] { "uri", "name" });
            preset.ShouldNotBeNull();
            preset.Uri.ShouldNotBeNull();
            preset.Name.ShouldNotBeNull();
            preset.Settings.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetEmbedPresetsWithFields()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();
            var presets = await client.GetEmbedPresetsAsync(UserId.Me, fields: new[] { "uri", "name" });
            presets.ShouldNotBeNull();
            presets.Data.Count.ShouldBeGreaterThan(0);
            presets.Data[0].ShouldNotBeNull();
            presets.Data[0].Uri.ShouldNotBeNull();
            presets.Data[0].Name.ShouldNotBeNull();
            presets.Data[0].Settings.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveSecondPage()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();

            for (var i = 0; i < 5; i++)
            {
                try
                {
                    var presets = await client.GetEmbedPresetsAsync(UserId.Me, 2, 1);
                    presets.ShouldNotBeNull();
                    return;
                }
                catch (VimeoApiException ex)
                {
                    if (ex.Message.Contains("Please try again."))
                    {
                        continue;
                    }

                    throw;
                }
            }
        }
    }
}
