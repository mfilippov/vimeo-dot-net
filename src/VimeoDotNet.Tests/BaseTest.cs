using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Shouldly;
using VimeoDotNet.Authorization;
using VimeoDotNet.Constants;
using VimeoDotNet.Tests.Settings;

#if NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET45 || NET451 || NET452 || NET6 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit { }
}
#endif

namespace VimeoDotNet.Tests
{
    public class BaseTest : IDisposable
    {
        protected readonly VimeoApiTestSettings VimeoSettings;
        protected readonly IVimeoClient AuthenticatedClient;

        // http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        protected const string TestTextTrackFilePath = @"VimeoDotNet.Tests.TestData.File.test.vtt";

        protected const string TextThumbnailFilePath = @"VimeoDotNet.Tests.Resources.test.png";
        protected const string TestVideoFilePath = @"VimeoDotNet.Tests.TestData.File.test.mp4";

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

        protected static Stream GetFileFromEmbeddedResources(string relativePath)
        {
            var assembly = typeof(BaseTest).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream(relativePath);
        }

        private static string GetJson(string name)
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
            request.Headers["Authorization"].ShouldNotBeNull();
            var parts = request.Headers["Authorization"].Split(new [] {' '}, 2);
            parts.Length.ShouldBe(2);
            return parts[1].Trim() == VimeoSettings.AccessToken;
        }

        public class RequestSettings
        {
            public enum HttpMethod
            {
                DELETE,
                GET,
                PATCH,
                POST,
                PUT,
            }
            public string UrlSuffix { get; init; }
            public HttpMethod Method { get; init; } = HttpMethod.GET;
            public string RequestTextBody { get; init; } = string.Empty;
            public string RequestBinaryFile { get; init; } = null;
            public int StatusCode { get; init; } = 200;
            [CanBeNull] public string ResponseJsonFile { get; init; }

            public bool AuthBypass { get; init; } = false;
        }

        protected void MockHttpRequest(RequestSettings settings)
        {
            var route = $"{settings.UrlSuffix}:{settings.Method}";
            if (_requestMocks.Count == 0)
            {
                Request.MockProtocol = "http";
                Request.MockHostName = "localhost";
                Request.MockPort = _testHttpServer.Start(_requestMocks);
            }
            var added = _requestMocks.TryAdd(route, (request, response) =>
            {
                request.Url?.PathAndQuery.ShouldBe(settings.UrlSuffix);
                request.HttpMethod.ShouldBe(settings.Method.ToString());
                response.Headers.Add("x-ratelimit-limit", "1000");
                response.Headers.Add("x-ratelimit-remaining", "998");
                response.Headers.Add("x-ratelimit-reset", "2023-04-08T09:21:35+00:00");
                if (!settings.AuthBypass && !CheckAuth(request))
                {
                    response.StatusCode = 401;
                    var wrt = new StreamWriter(response.OutputStream);
                    wrt.Write("""{ "error": "Bad access token" }""");
                    wrt.Write(settings.ResponseJsonFile != null ? GetJson(settings.ResponseJsonFile) : string.Empty);
                    wrt.Close();
                    response.Close();
                }
                else
                {
                    if (settings.RequestBinaryFile == null)
                    {
                        var rdr = new StreamReader(request.InputStream);
                        var actualRequestBody = rdr.ReadToEnd();
                        actualRequestBody.ShouldBe(settings.RequestTextBody);
                    }
                    else
                    {
                        var hasher = SHA256.Create();
                        var expectedHash = hasher.ComputeHash(GetFileFromEmbeddedResources(settings.RequestBinaryFile));
                        var actualHash = hasher.ComputeHash(request.InputStream);
                        actualHash.ShouldBe(expectedHash);
                    }
                    response.StatusCode = settings.StatusCode;
                    response.ContentType = "application/json";
                    var wrt = new StreamWriter(response.OutputStream);
                    wrt.Write(settings.ResponseJsonFile != null ? GetJson(settings.ResponseJsonFile) : string.Empty);
                    wrt.Close();
                    response.Close();
                }
                
            });
            if (!added)
            {
                throw new Exception($"can't register handler for url: '{settings.UrlSuffix}'");
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
