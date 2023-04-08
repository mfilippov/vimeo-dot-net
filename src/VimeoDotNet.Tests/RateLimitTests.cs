using System;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Models;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class RateLimitTests : BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyUpdateRateLimitPerRequest()
        {
            MockHttpRequest("/me/albums", "GET",string.Empty, 200,
                GetJson("Album.albums.json"));
            var client = CreateAuthenticatedClient();

            client.RateLimit.ShouldBe(0);
            client.RateLimitRemaining.ShouldBe(0);
            client.RateLimitReset.ShouldBeGreaterThan(DateTime.MinValue);
            client.RateLimitReset.Kind.ShouldBe(DateTimeKind.Utc);

            var albums = await client.GetAlbumsAsync(UserId.Me);
            albums.ShouldNotBeNull();

            client.RateLimit.ShouldBeGreaterThan(0);
            client.RateLimitRemaining.ShouldBeGreaterThan(0);
            client.RateLimitReset.ShouldBeGreaterThan(DateTime.MinValue);
            client.RateLimitReset.Kind.ShouldBe(DateTimeKind.Utc);
        }
    }
}