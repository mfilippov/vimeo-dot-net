using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class VideoTests : BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyDeleteVideo()
        {
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestFilePath), "video/mp4"))
            {
                var length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                var completedRequest = await client.UploadEntireFileAsync(file);
                completedRequest.ShouldNotBeNull();
                completedRequest.IsVerifiedComplete.ShouldBeTrue();
                completedRequest.BytesWritten.ShouldBe(length);
                completedRequest.ClipUri.ShouldNotBeNull();
                completedRequest.ClipId.HasValue.ShouldBeTrue();
                completedRequest.ClipId.ShouldNotBeNull();
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
                (await client.GetVideoAsync(completedRequest.ClipId.Value)).ShouldBeNull();
            }
        }

        [Fact]
        public async Task ShouldCorrectlyRetrievesVideosById()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.ShouldNotBeNull();
            video.Id.ShouldBe(VimeoSettings.VideoId);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrievesVideosByUserId()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetVideosAsync(VimeoSettings.UserId);
            videos.ShouldNotBeNull();
            videos.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrievesVideosByMe()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetVideosAsync(UserId.Me);
            videos.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyRetriveSecondPage()
        {
            var client = CreateAuthenticatedClient();

            for (var i = 0; i < 5; i++)
            {
                try
                {
                    var videos = await client.GetVideosAsync(VimeoSettings.UserId, 2, 1);
                    videos.ShouldNotBeNull();
                    return;
                }
                catch (VimeoApiException ex)
                {
                    if (ex.Message.Contains("Please try again."))
                    {
                        continue;
                    }

                    throw;
                }
            }
        }

        [Fact]
        public async Task ShouldCorrectlyGetVideoWithFields()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.VideoId, new[] {"uri", "name"});
            video.ShouldNotBeNull();
            video.Uri.ShouldNotBeNull();
            video.Name.ShouldNotBeNull();
            video.Pictures.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetUserAlbumVideosByUserId()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetAlbumVideosAsync(VimeoSettings.UserId, VimeoSettings.AlbumId);
            videos.ShouldNotBeNull();
            videos.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGetUserAlbumVideosByMe()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetAlbumVideosAsync(UserId.Me, VimeoSettings.AlbumId);
            videos.ShouldNotBeNull();
            videos.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGetAccountAlbumVideosWithFields()
        {
            var client = CreateAuthenticatedClient();
            var videos =
                await client.GetAlbumVideosAsync(VimeoSettings.AlbumId, 1, null, fields: new[] {"uri", "name"});
            videos.ShouldNotBeNull();
            videos.Data.Count.ShouldBeGreaterThan(0);
            var video = videos.Data[0];
            video.ShouldNotBeNull();
            video.Uri.ShouldNotBeNull();
            video.Name.ShouldNotBeNull();
            video.Pictures.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetVideoThumbnails()
        {
            var client = CreateAuthenticatedClient();
            var pictures = await client.GetPicturesAsync(VimeoSettings.VideoId);
            pictures.ShouldNotBeNull();
            pictures.Data.Count.ShouldBeGreaterThan(0);
            var uriParts = pictures.Data[0].Uri.Split('/');
            var pictureId = long.Parse(uriParts[uriParts.Length - 1]);
            var picture = await client.GetPictureAsync(VimeoSettings.VideoId, pictureId);
            picture.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetAccountVideoWithUnauthenticatedToken()
        {
            var client = await CreateUnauthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.ShouldNotBeNull();
            video.Pictures.Uri.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyUpdateVideoMetadataAndAllowedDomain()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Privacy.EmbedPrivacy.ShouldBe(VideoEmbedPrivacyEnum.Public);
            await client.UpdateVideoMetadataAsync(VimeoSettings.VideoId, new VideoUpdateMetadata
            {
                EmbedPrivacy = VideoEmbedPrivacyEnum.Private
            });
            video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Privacy.EmbedPrivacy.ShouldBe(VideoEmbedPrivacyEnum.Private);

            await Should.ThrowAsync<VimeoApiException>(async () =>
                await client.UpdateVideoAllowedDomainAsync(VimeoSettings.VideoId, "example.com"));

            await client.UpdateVideoMetadataAsync(VimeoSettings.VideoId, new VideoUpdateMetadata
            {
                EmbedPrivacy = VideoEmbedPrivacyEnum.Public
            });
            video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.Privacy.EmbedPrivacy.ShouldBe(VideoEmbedPrivacyEnum.Public);
        }

        [Fact]
        public async Task ShouldCorrectlyGetPuctureFromVideo()
        {
            var client = CreateAuthenticatedClient();
            var pictures = await client.GetPicturesAsync(VimeoSettings.VideoId);
            pictures.Data.Count.ShouldBeGreaterThan(0);
            var picture = pictures.Data[0];
            var parts = picture.Uri.Split('/');
            var pictureId = long.Parse(parts[parts.Length - 1]);
            var pictureById = await client.GetPictureAsync(VimeoSettings.VideoId, pictureId);
            pictureById.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyAssignEmbedPresetToVideo()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();
            await client.AssignEmbedPresetToVideoAsync(VimeoSettings.VideoId, VimeoSettings.EmbedPresetId);
            var video = await client.GetVideoAsync(VimeoSettings.VideoId, new[] { "embed_presets" });
            video.ShouldNotBeNull();
            video.EmbedPresets.ShouldNotBeNull();
            video.EmbedPresets.Id.ShouldBe(VimeoSettings.EmbedPresetId);
        }

        [Fact]
        public async Task ShouldCorrectlyUnassignEmbedPresetFromVideo()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;

            var client = CreateAuthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.VideoId, new[] { "embed_presets" });
            var oldPresetId = video?.EmbedPresets?.Id;
            await client.UnassignEmbedPresetFromVideoAsync(VimeoSettings.VideoId, VimeoSettings.EmbedPresetId);
            video = await client.GetVideoAsync(VimeoSettings.VideoId, new[] { "embed_presets" });
            video.ShouldNotBeNull();
            if (oldPresetId == VimeoSettings.EmbedPresetId)
            {
                video.EmbedPresets.ShouldBeNull();
            }
            else
            {
                video.EmbedPresets?.Id.ShouldBe(oldPresetId);
            }
        }
    }
}