using Shouldly;
using VimeoDotNet.Authorization;
using VimeoDotNet.Tests.Settings;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class AuthorizationClientAsyncTests
    {
        private readonly VimeoApiTestSettings _vimeoSettings;

        public AuthorizationClientAsyncTests()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            _vimeoSettings = SettingsLoader.LoadSettings();
        }

        [Fact]
        public async void ShouldCorrectlyGetUnauthenticatedToken()
        {
            var client = new AuthorizationClient(_vimeoSettings.ClientId, _vimeoSettings.ClientSecret);

            var token = await client.GetUnauthenticatedTokenAsync();
            token.access_token.ShouldNotBeNull();
            token.user.ShouldBeNull();
        }
    }
}