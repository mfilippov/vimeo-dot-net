using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class TagTests : BaseTest, IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            var client = CreateAuthenticatedClient();
            await CleanupTags(client, VimeoSettings.VideoId);
            var video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Tags.Count.ShouldBe(0);
        }

        public async Task DisposeAsync()
        {
            await CleanupTags(AuthenticatedClient, VimeoSettings.VideoId);
            var video = await AuthenticatedClient.GetVideoAsync(VimeoSettings.VideoId);
            video.Tags.Count.ShouldBe(0);
        }

        private static async Task CleanupTags(IVimeoClient client, long clipId)
        {
            var tags = await client.GetVideoTags(clipId);
            foreach (var tag in tags.Data)
            {
                await client.DeleteVideoTagAsync(clipId, tag.Id);
            }
        }
        
        [Fact]
        public async Task ShouldCorrectlyAddTagVideoTag()
        {
            var tag = await AuthenticatedClient.AddVideoTagAsync(VimeoSettings.VideoId, "test-tag1");

            tag.ShouldNotBeNull();
            tag.Id.ShouldBe("test-tag1");
            tag.Name.ShouldBe("test-tag1");
            tag.Canonical.ShouldBe("test-tag1");
            tag.Uri.ShouldNotBeEmpty();
            tag.Metadata.ShouldNotBeNull();
            tag.Metadata.Connections.ShouldNotBeNull();
            tag.Metadata.Connections.Videos.Uri.ShouldNotBeEmpty();
            tag.Metadata.Connections.Videos.Options.ShouldNotBeEmpty();
            tag.Metadata.Connections.Videos.Total.ShouldBeGreaterThan(0);
            
            var video = await AuthenticatedClient.GetVideoAsync(VimeoSettings.VideoId);
            video.Tags.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ShouldCorrectlyGetVideoTag()
        {
            await AuthenticatedClient.AddVideoTagAsync(VimeoSettings.VideoId, "test-tag1");
            var result = await AuthenticatedClient.GetVideoTagAsync("test-tag1");
            result.Id.ShouldBe("test-tag1");
        }

        [Fact]
        public async Task ShouldCorrectlyGetVideoByTag()
        {
            var client = await CreateUnauthenticatedClient();
            var result = await client.GetVideoByTag("test", 1, 10, GetVideoByTagSort.Name,
                GetVideoByTagDirection.Asc, new[] {"uri", "name"});
            
            result.Page.ShouldBe(1);
            result.PerPage.ShouldBe(10);
            result.Data.Count.ShouldBeGreaterThan(0);
            result.Data[0].Name.ShouldNotBeEmpty();
            result.Data[0].Uri.ShouldNotBeEmpty();
        }
    }
}