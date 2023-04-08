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
            const int clipId = 530969457; 
            MockHttpRequest($"/videos/{clipId}/tags/test-tag1", "PUT",string.Empty, 200,
                GetJson("Tag.put-tag-530969457.json"));
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
            
            MockHttpRequest($"/videos/{clipId}", "GET",string.Empty, 200,
                GetJson("Video.video-530969457.json"));

            var video = await AuthenticatedClient.GetVideoAsync(clipId);
            video.Tags.Count.ShouldBe(1);

            MockHttpRequest($"/tags/test-tag1", "GET",string.Empty, 200,
                GetJson("Tag.put-tag-530969457.json"));
            var tagResult = await AuthenticatedClient.GetVideoTagAsync("test-tag1");
            tagResult.Id.ShouldBe("test-tag1");

            MockHttpRequest($"/tags/test/videos?page=1&per_page=10&sort=name&direction=asc&fields=uri,name",
                "GET",string.Empty,
                200, GetJson("Tag.video-by-tag-test.json"));
            var videoResult = await AuthenticatedClient.GetVideoByTag("test", 1, 10,
                GetVideoByTagSort.Name, GetVideoByTagDirection.Asc, new[] {"uri", "name"});
        
            videoResult.Page.ShouldBe(1);
            videoResult.PerPage.ShouldBe(10);
            videoResult.Data.Count.ShouldBeGreaterThan(0);
            videoResult.Data[0].Name.ShouldNotBeEmpty();
            videoResult.Data[0].Uri.ShouldNotBeEmpty();
            
                
            MockHttpRequest($"/videos/{clipId}/tags", "GET",string.Empty,
                200, GetJson("Tag.tags-for-video-530969457.json"));
            var tags = await AuthenticatedClient.GetVideoTags(clipId);
            foreach (var t in tags.Data)
            {
                MockHttpRequest($"/videos/{clipId}/tags/{t.Id}", "DELETE", string.Empty,
                    200, string.Empty);
                await AuthenticatedClient.DeleteVideoTagAsync(clipId, t.Id);
            }
        }
    }
}