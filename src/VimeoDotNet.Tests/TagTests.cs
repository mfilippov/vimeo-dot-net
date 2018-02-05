using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class TagTests : BaseTest
    {
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
            var client = CreateAuthenticatedClient();
            await CleanupTags(client, VimeoSettings.VideoId);
            var video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Tags.Count.ShouldBe(0);
            var tag = await client.AddVideoTagAsync(VimeoSettings.VideoId, "test-tag1");
            await CleanupTags(client, VimeoSettings.VideoId);
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
            video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Tags.Count.ShouldBe(1);
            
            video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Tags.Count.ShouldBe(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGetVideoTag()
        {
            var client = CreateAuthenticatedClient();
            await CleanupTags(client, VimeoSettings.VideoId);
            var video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Tags.Count.ShouldBe(0);

            await client.AddVideoTagAsync(VimeoSettings.VideoId, "test-tag1");

            var result = await client.GetVideoTagAsync("test-tag1");
            
            await CleanupTags(client, VimeoSettings.VideoId);
            
            result.Id.ShouldBe("test-tag1");

            video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Tags.Count.ShouldBe(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGetVideoByTag()
        {
            var client = CreateAuthenticatedClient();
            await CleanupTags(client, VimeoSettings.VideoId);
            
            await client.AddVideoTagAsync(VimeoSettings.VideoId, "test-tag1");

            var result = await client.GetVideoByTag("test-tag1", 1, 10, GetVideoByTagSort.Name,
                GetVideoByTagDirection.Asc, new[] {"uri", "name"});
            await CleanupTags(client, VimeoSettings.VideoId);
            
            result.Page.ShouldBe(1);
            result.PerPage.ShouldBe(10);
            result.Data.Count.ShouldBeGreaterThan(0);
            result.Data[0].Name.ShouldNotBeEmpty();
            result.Data[0].Uri.ShouldNotBeEmpty();
        }
    }
}