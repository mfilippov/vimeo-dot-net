using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class TagTests : BaseTest
    {
        [Fact]
        public async Task TagInteractionTest()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var tag = await AuthenticatedClient.AddVideoTagAsync(clipId, "test-tag1");

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

                var video = await AuthenticatedClient.GetVideoAsync(clipId);
                video.Tags.Count.ShouldBe(1);

                var tagResult = await AuthenticatedClient.GetVideoTagAsync("test-tag1");
                tagResult.Id.ShouldBe("test-tag1");
                
                var videoResult = await AuthenticatedClient.GetVideoByTag("test", 1, 10, GetVideoByTagSort.Name,
                    GetVideoByTagDirection.Asc, new[] {"uri", "name"});
            
                videoResult.Page.ShouldBe(1);
                videoResult.PerPage.ShouldBe(10);
                videoResult.Data.Count.ShouldBeGreaterThan(0);
                videoResult.Data[0].Name.ShouldNotBeEmpty();
                videoResult.Data[0].Uri.ShouldNotBeEmpty();
                
                var tags = await AuthenticatedClient.GetVideoTags(clipId);
                foreach (var t in tags.Data)
                {
                    await AuthenticatedClient.DeleteVideoTagAsync(clipId, t.Id);
                }
            });
        }
    }
}