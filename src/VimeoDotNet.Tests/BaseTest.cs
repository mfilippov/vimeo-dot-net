using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Authorization;
using VimeoDotNet.Constants;
using VimeoDotNet.Tests.Settings;

namespace VimeoDotNet.Tests
{
    public class BaseTest : IDisposable
    {
        protected readonly VimeoApiTestSettings VimeoSettings;
        protected readonly IVimeoClient AuthenticatedClient;

        // http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        protected const string TestTextTrackFilePath = @"VimeoDotNet.Tests.Resources.test.vtt";

        protected const string TextThumbnailFilePath = @"VimeoDotNet.Tests.Resources.test.png";

        private TestHttpServer _testHttpServer;

        private readonly ConcurrentDictionary<string, Action<HttpListenerRequest, HttpListenerResponse>> _requestMocks = new ();

        protected static string GetJson(string name)
        {
            var path = $"VimeoDotNet.Tests.TestData.{name}";
            using var resourceStream = typeof(BaseTest).Assembly.GetManifestResourceStream(path);
            if (resourceStream == null)
            {
                throw new Exception($"can't find resource stream: '{path}'");
            }
            var rdr = new StreamReader(resourceStream);
            var json = rdr.ReadToEnd();
            rdr.Close();
            resourceStream.Close();
            return json;
        }

        protected BaseTest()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            VimeoSettings = SettingsLoader.LoadSettings();
            AuthenticatedClient = CreateAuthenticatedClient();
            _testHttpServer = new TestHttpServer();
        }

        protected void MockHttpRequest(string urlSuffix, string method, string requestBody, string responseBody)
        {
            var route = $"{urlSuffix}:{method}";
            if (_requestMocks.Count == 0)
            {
                Request.MockProtocol = "http";
                Request.MockHostName = "localhost";
                Request.MockPort = _testHttpServer.Start(_requestMocks);
            }
            var added = _requestMocks.TryAdd(route, (request, response) =>
            {
                request.Url?.PathAndQuery.ShouldBe(urlSuffix);
                request.HttpMethod.ShouldBe(method);
                var rdr = new StreamReader(request.InputStream);
                var rBody = rdr.ReadToEnd();
                rBody.ShouldBe(requestBody);
                response.StatusCode = 200;
                response.ContentType = "application/json";
                var wrt = new StreamWriter(response.OutputStream);
                wrt.Write(responseBody);
                wrt.Close();
                response.Close();
            });
            if (!added)
            {
                throw new Exception($"can't register handler for url: '{urlSuffix}'");
            }
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

        public void Dispose()
        {
            if (_testHttpServer == null)
            {
                return;
            }
            _testHttpServer.Stop();
            _testHttpServer = null;
        }
    }
}