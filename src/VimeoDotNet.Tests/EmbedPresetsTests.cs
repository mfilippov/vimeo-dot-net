using Shouldly;
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
            const int embedPresetId = 120476914;
            MockHttpRequest($"/me/presets/{embedPresetId}", "GET", string.Empty, 
                200, GetJson("Presets.get-presets-120476914.json"));
            var client = CreateAuthenticatedClient();
            var preset = await client.GetEmbedPresetAsync(UserId.Me, embedPresetId);
            preset.ShouldNotBeNull();
            preset.Id.ShouldBe(embedPresetId);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveUserEmbedPresetById()
        {
            const int userId = 2433258;
            const int embedPresetId = 120476914;
            MockHttpRequest($"/users/{userId}/presets/{embedPresetId}","GET", string.Empty,
                200, GetJson("Presets.get-presets-120476914.json"));
            var client = CreateAuthenticatedClient();
            var preset = await client.GetEmbedPresetAsync(userId, embedPresetId);
            preset.ShouldNotBeNull();
            preset.Id.ShouldBe(embedPresetId);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveMyEmbedPresets()
        {
            MockHttpRequest($"/me/presets", "GET", string.Empty,
                200, GetJson("Presets.presets-2433258.json"));
            var client = CreateAuthenticatedClient();
            var presets = await client.GetEmbedPresetsAsync(UserId.Me);
            presets.ShouldNotBeNull();
            presets.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveUserEmbedPresets()
        {
            const int userId = 2433258;
            MockHttpRequest($"/users/{userId}/presets", "GET", string.Empty,
                200,GetJson("Presets.presets-2433258.json"));
            var client = CreateAuthenticatedClient();
            var presets = await client.GetEmbedPresetsAsync(userId);
            presets.ShouldNotBeNull();
            presets.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGetEmbedPresetWithFields()
        {
            const int embedPresetId = 120476914;
            MockHttpRequest($"""/me/presets/{embedPresetId}?fields=uri,name""", 
                "GET", string.Empty,
                200, GetJson("Presets.get-presets-120476914-with-fields.json"));
            var client = CreateAuthenticatedClient();
            var preset = await client.GetEmbedPresetAsync(UserId.Me, embedPresetId, new[] { "uri", "name" });
            preset.ShouldNotBeNull();
            preset.Uri.ShouldNotBeNull();
            preset.Name.ShouldNotBeNull();
            preset.Settings.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetEmbedPresetsWithFields()
        {
            MockHttpRequest($"/me/presets?fields=uri,name", "GET", string.Empty, 200,
                GetJson("Presets.presets-2433258-with-fields.json"));
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
            MockHttpRequest($"/me/presets?page=2&per_page=1", "GET", string.Empty, 400,
                GetJson("Presets.get-presets-invalid-page.json"));
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
