using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using VimeoDotNet.Authorization;
using VimeoDotNet.Tests.Settings;

namespace VimeoDotNet.Tests
{
    public class BaseTest
    {
        protected readonly VimeoApiTestSettings VimeoSettings;
        protected readonly IVimeoClient AuthenticatedClient;

        // http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        protected const string TestTextTrackFilePath = @"VimeoDotNet.Tests.Resources.test.vtt";

        protected const string TextThumbnailFilePath = @"VimeoDotNet.Tests.Resources.test.png";

        protected BaseTest()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            VimeoSettings = SettingsLoader.LoadSettings();
            AuthenticatedClient = CreateAuthenticatedClient();
        }

        protected async Task<VimeoClient> CreateUnauthenticatedClient()
        {
            var authorizationClient = new AuthorizationClient(VimeoSettings.ClientId, VimeoSettings.ClientSecret);
            var unauthenticatedToken = await authorizationClient.GetUnauthenticatedTokenAsync();
            return new VimeoClient(unauthenticatedToken.AccessToken);
        }

        protected IVimeoClient CreateAuthenticatedClient()
        {
            return new VimeoClient(VimeoSettings.AccessToken);
        }
    }
}