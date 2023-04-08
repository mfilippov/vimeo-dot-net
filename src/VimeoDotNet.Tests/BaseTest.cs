using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        protected BaseTest()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            VimeoSettings = SettingsLoader.LoadSettings();
            _testHttpServer = new TestHttpServer();
            AuthenticatedClient = CreateAuthenticatedClient();
        }

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

        private bool CheckAuth(HttpListenerRequest request)
        {
            var urlWithoutAuth = new HashSet<string> { "/oauth/authorize/client" };
            if (urlWithoutAuth.Contains(request.Url?.PathAndQuery))
            {
                return true;
            }
            request.Headers["Authorization"].ShouldNotBeNull();
            var parts = request.Headers["Authorization"].Split(new [] {' '}, 2);
            parts.Length.ShouldBe(2);
            return parts[1].Trim() == VimeoSettings.AccessToken;
        }
        
        protected void MockHttpRequest(string urlSuffix, string method, string requestBody, int statusCode, string responseBody)
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
                response.Headers.Add("x-ratelimit-limit", "1000");
                response.Headers.Add("x-ratelimit-remaining", "998");
                response.Headers.Add("x-ratelimit-reset", "2023-04-08T09:21:35+00:00");
                if (!CheckAuth(request))
                {
                    response.StatusCode = 401;
                    var wrt = new StreamWriter(response.OutputStream);
                    wrt.Write("""{ "error": "Bad access token" }""");
                    wrt.Write(responseBody);
                    wrt.Close();
                    response.Close();
                }
                else
                {
                    var rdr = new StreamReader(request.InputStream);
                    var actualRequestBody = rdr.ReadToEnd();
                    actualRequestBody.ShouldBe(requestBody);
                    response.StatusCode = statusCode;
                    response.ContentType = "application/json";
                    var wrt = new StreamWriter(response.OutputStream);
                    wrt.Write(responseBody);
                    wrt.Close();
                    response.Close();
                }
                
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
            Request.MockProtocol = null;
            Request.MockHostName = null;
            Request.MockPort = 0;
            _requestMocks.Clear();
        }
    }
}