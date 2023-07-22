using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Shouldly;
using VimeoDotNet.Authorization;
using VimeoDotNet.Constants;

#if NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET45 || NET451 || NET452 || NET6 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
namespace System.Runtime.CompilerServices
{
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static class IsExternalInit { }
}
#endif

namespace VimeoDotNet.Tests
{
    public class BaseTest : IDisposable
    {
        protected readonly IVimeoClient AuthenticatedClient;

        // http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        protected const string TestTextTrackFilePath = @"VimeoDotNet.Tests.TestData.File.test.vtt";

        protected const string TextThumbnailFilePath = @"VimeoDotNet.Tests.TestData.File.test.png";
        protected const string TestVideoFilePath = @"VimeoDotNet.Tests.TestData.File.test.mp4";

        private TestHttpServer _testHttpServer;

        private readonly ConcurrentDictionary<string, Queue<Action<HttpListenerRequest, HttpListenerResponse>>>
            _requestMocks = new();

        protected BaseTest()
        {
            _testHttpServer = new TestHttpServer();
            AuthenticatedClient = CreateAuthenticatedClient();
        }

        protected static Stream GetFileFromEmbeddedResources(string relativePath)
        {
            var assembly = typeof(BaseTest).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream(relativePath);
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
            request.Headers["Authorization"].ShouldNotBeNull();
            var parts = request.Headers["Authorization"].Split(new[] { ' ' }, 2);
            parts.Length.ShouldBe(2);
            return parts[1].Trim() == AccessToken;
        }

        public class RequestSettings
        {
            public enum HttpMethod
            {
                Delete,
                Head,
                Get,
                Patch,
                Post,
                Put,
            }

            public bool AuthBypass { get; init; }
            public string UrlSuffix { get; init; }
            public HttpMethod Method { get; init; } = HttpMethod.Get;
            public string RequestTextBody { get; init; } = string.Empty;
            [CanBeNull] public string RequestBinaryFile { get; init; }
            [CanBeNull] public byte[] RequestBinaryBody { get; init; }
            [CanBeNull] public string ResponseJsonFile { get; init; }
            public Dictionary<string, string> RequestHeaders { get; init; } = new();
            public int StatusCode { get; init; } = 200;
            public Dictionary<string, string> CustomResponseHeaders { get; init; } = new();
        }

        protected void MockHttpRequest(RequestSettings settings)
        {
            var route = $"{settings.UrlSuffix}:{settings.Method.ToString().ToUpperInvariant()}";
            if (_requestMocks.Count == 0)
            {
                Request.MockProtocol = "http";
                Request.MockHostName = "localhost";
                Request.MockPort = _testHttpServer.Start(_requestMocks);
            }

            if (!_requestMocks.ContainsKey(route))
            {
                _requestMocks.TryAdd(route, new Queue<Action<HttpListenerRequest, HttpListenerResponse>>())
                    .ShouldBeTrue();
            }

            var queue = _requestMocks[route];

            queue.Enqueue((request, response) =>
            {
                request.Url?.PathAndQuery.ShouldBe(settings.UrlSuffix);
                request.HttpMethod.ShouldBe(settings.Method.ToString().ToUpperInvariant());
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
                    foreach (var key in settings.RequestHeaders.Keys)
                    {
                        request.Headers[key].ShouldBe(settings.RequestHeaders[key]);
                    }

                    if (settings.RequestBinaryFile != null)
                    {
                        var hasher = SHA256.Create();
                        var expectedHash = hasher.ComputeHash(GetFileFromEmbeddedResources(settings.RequestBinaryFile));
                        var actualHash = hasher.ComputeHash(request.InputStream);
                        actualHash.ShouldBe(expectedHash);
                    }
                    else if (settings.RequestBinaryBody != null)
                    {
                        var hasher = SHA256.Create();
                        var expectedHash = hasher.ComputeHash(new MemoryStream(settings.RequestBinaryBody));
                        var actualHash = hasher.ComputeHash(request.InputStream);
                        actualHash.ShouldBe(expectedHash);
                    }
                    else
                    {
                        var rdr = new StreamReader(request.InputStream);
                        var actualRequestBody = rdr.ReadToEnd();
                        actualRequestBody.ShouldBe(settings.RequestTextBody);
                    }

                    response.StatusCode = settings.StatusCode;
                    response.ContentType = "application/json";
                    foreach (var key in settings.CustomResponseHeaders.Keys)
                    {
                        response.Headers.Add(key, settings.CustomResponseHeaders[key]);
                    }
                    var wrt = new StreamWriter(response.OutputStream);
                    wrt.Write(settings.ResponseJsonFile != null ? GetJson(settings.ResponseJsonFile) : string.Empty);
                    wrt.Close();
                    response.Close();
                }
            });
        }

        private const string AccessToken = "5oGVeY4GQKr4l4T/wYS64Q==";
        private const string ClientId = "b9ba3be18a6747e30b60a5108be3c567ee362535";

        private const string ClientSecret = "yN9Os06F8I410SZnYmkKymy3kIoaxLX3QzJZ91ZHFdr9o9on7fviI/ZCxiWeT47" +
                                            "piuf5A+1oMn2ks9JmAuhnR5ilvqVZPhJ3qH6nIEBwjlaHXa5qEygrPPSuDya8L+QW";
        
        protected async Task<VimeoClient> CreateUnauthenticatedClient()
        {
            var authorizationClient = new AuthorizationClient(ClientId, ClientSecret);
            var unauthenticatedToken = await authorizationClient.GetUnauthenticatedTokenAsync();
            return new VimeoClient(unauthenticatedToken.AccessToken);
        }

        protected static IVimeoClient CreateAuthenticatedClient()
        {
            return new VimeoClient(AccessToken);
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