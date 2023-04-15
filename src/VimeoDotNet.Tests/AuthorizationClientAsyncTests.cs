using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Authorization;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class AuthorizationClientAsyncTests: BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyGetUnauthenticatedToken()
        {
            const string clientId = "b9ba3be18a6747e30b60a5108be3c567ee362535";
            const string clientSecret = "yN9Os06F8I410SZnYmkKymy3kIoaxLX3QzJZ91ZHFdr9o9on7fviI/ZCxiWeT47piuf5" +
                                        "A+1oMn2ks9JmAuhnR5ilvqVZPhJ3qH6nIEBwjlaHXa5qEygrPPSuDya8L+QW";

            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/oauth/authorize/client",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = "grant_type=client_credentials",
                ResponseJsonFile = "User.unauthenticated-token.json",
                RequestHeaders = new Dictionary<string, string>
                {
                    ["Authorization"] = "Basic YjliYTNiZTE4YTY3NDdlMzBiNjBhNTEwOGJlM2M1NjdlZTM2Mj" +
                                        "UzNTp5TjlPczA2RjhJNDEwU1puWW1rS3lteTNrSW9heExYM1F6Slo5MVpIRmRyOW85b243Z" +
                                        "nZpSS9aQ3hpV2VUNDdwaXVmNUErMW9NbjJrczlKbUF1aG5SNWlsdnFWWlBoSjNxSDZuSUVC" +
                                        "d2psYUhYYTVxRXlnclBQU3VEeWE4TCtRVw=="
                },
                AuthBypass = true
            });
            var client = new AuthorizationClient(clientId, clientSecret);

            var token = await client.GetUnauthenticatedTokenAsync();
            token.AccessToken.ShouldNotBeNull();
            token.User.ShouldBeNull();
        }

        [Fact]
        public async Task VerifyAuthenticatedAccess()
        {
            const string clientId = "b9ba3be18a6747e30b60a5108be3c567ee362535";
            const string clientSecret = "yN9Os06F8I410SZnYmkKymy3kIoaxLX3QzJZ91ZHFdr9o9on7fviI/ZCxiWeT47piuf5" +
                                        "A+1oMn2ks9JmAuhnR5ilvqVZPhJ3qH6nIEBwjlaHXa5qEygrPPSuDya8L+QW";
            const string accessToken = "5oGVeY4GQKr4l4T/wYS64Q==";
            MockHttpRequest(new RequestSettings{ 
                UrlSuffix = "/oauth/verify",
                ResponseJsonFile = "User.oauth-verify.json",
                RequestHeaders = new Dictionary<string, string>
                {
                    ["Authorization"] = "Bearer 5oGVeY4GQKr4l4T/wYS64Q=="
                }
            });
            MockHttpRequest(new RequestSettings{ 
                UrlSuffix = "/oauth/verify",
                ResponseJsonFile = "User.oauth-verify.json",
            });
            var client = new AuthorizationClient(clientId, clientSecret);

            var b = await client.VerifyAccessTokenAsync(accessToken);
            b.ShouldBeTrue();
            b = await client.VerifyAccessTokenAsync("abadaccesstoken");
            b.ShouldBeFalse();
        }
    }
}