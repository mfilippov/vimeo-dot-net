using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class VideoTests : BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyRetrievesVideosById()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var video = await AuthenticatedClient.GetVideoAsync(clipId);
                video.ShouldNotBeNull();
                video.Id.ShouldBe(clipId);
            });
        }

        [Fact]
        public async Task ShouldCorrectlyRetrievesVideosByUserId()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var videos = await AuthenticatedClient.GetVideosAsync(VimeoSettings.UserId);
                videos.ShouldNotBeNull();
                videos.Data.Count.ShouldBeGreaterThan(0);
            });
        }

        [Fact]
        public async Task ShouldCorrectlyRetrievesVideosByMe()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var videos = await AuthenticatedClient.GetVideosAsync(UserId.Me);
                videos.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveSecondPage()
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    var videos = await AuthenticatedClient.GetVideosAsync(VimeoSettings.UserId, 2, 1);
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
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var video = await AuthenticatedClient.GetVideoAsync(clipId, new[] {"uri", "name"});
                video.ShouldNotBeNull();
                video.Uri.ShouldNotBeNull();
                video.Name.ShouldNotBeNull();
                video.Pictures.ShouldBeNull();
            });
        }

        [Fact]
        public async Task ShouldCorrectlyGetUserAlbumVideosByUserId()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                await AuthenticatedClient.WithTestAlbum(async albumId =>
                {
                    await AuthenticatedClient.AddToAlbumAsync(UserId.Me, albumId, clipId);
                    var videos = await AuthenticatedClient.GetAlbumVideosAsync(VimeoSettings.UserId, albumId);
                    videos.ShouldNotBeNull();
                    videos.Data.Count.ShouldBeGreaterThan(0);
                });
            });
        }

        [Fact]
        public async Task ShouldCorrectlyGetUserAlbumVideosByMe()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                await AuthenticatedClient.WithTestAlbum(async albumId =>
                {
                    await AuthenticatedClient.AddToAlbumAsync(UserId.Me, albumId, clipId);
                    var videos = await AuthenticatedClient.GetAlbumVideosAsync(UserId.Me, albumId);
                    videos.ShouldNotBeNull();
                    videos.Data.Count.ShouldBeGreaterThan(0);
                });
            });
        }

        [Fact]
        public async Task ShouldCorrectlyGetAccountAlbumVideosWithFields()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                await AuthenticatedClient.WithTestAlbum(async albumId =>
                {
                    await AuthenticatedClient.AddToAlbumAsync(UserId.Me, albumId, clipId);
                    var videos =
                        await AuthenticatedClient.GetAlbumVideosAsync(albumId, 1, null, fields: new[] {"uri", "name"});
                    videos.ShouldNotBeNull();
                    videos.Data.Count.ShouldBeGreaterThan(0);
                    var video = videos.Data[0];
                    video.ShouldNotBeNull();
                    video.Uri.ShouldNotBeNull();
                    video.Name.ShouldNotBeNull();
                    video.Pictures.ShouldBeNull();
                });
            });
        }

        [Fact]
        public async Task ShouldCorrectlyGetVideoThumbnails()
        {
            var pictures = await AuthenticatedClient.GetPicturesAsync(VimeoSettings.PublicVideoId);
            pictures.ShouldNotBeNull();
            pictures.Data.Count.ShouldBeGreaterThan(0);
            var uriParts = pictures.Data[0].Uri.Split('/');
            var pictureId = long.Parse(uriParts[uriParts.Length - 1]);
            var picture = await AuthenticatedClient.GetPictureAsync(VimeoSettings.PublicVideoId, pictureId);
            picture.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetAccountVideoWithUnauthenticatedToken()
        {
            var client = await CreateUnauthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.PublicVideoId);
            video.ShouldNotBeNull();
            video.Pictures.Uri.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyUpdateVideoEmbedPrivacy()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var video = await AuthenticatedClient.GetVideoAsync(clipId);
                video.Privacy.EmbedPrivacy.ShouldBe(VideoEmbedPrivacyEnum.Public);
                await AuthenticatedClient.UpdateVideoMetadataAsync(clipId, new VideoUpdateMetadata
                {
                    EmbedPrivacy = VideoEmbedPrivacyEnum.Private
                });
                video = await AuthenticatedClient.GetVideoAsync(clipId);
                video.Privacy.EmbedPrivacy.ShouldBe(VideoEmbedPrivacyEnum.Private);

                await AuthenticatedClient.UpdateVideoMetadataAsync(clipId, new VideoUpdateMetadata
                {
                    EmbedPrivacy = VideoEmbedPrivacyEnum.Public
                });
                video = await AuthenticatedClient.GetVideoAsync(clipId);
                video.Privacy.EmbedPrivacy.ShouldBe(VideoEmbedPrivacyEnum.Public);
            });
        }

        [Fact]
        public async Task ShouldCorrectlyWorkWithDomainsForEmbedding()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var account = await AuthenticatedClient.GetAccountInformationAsync();
                if (account.AccountType == AccountTypeEnum.Basic || account.AccountType == AccountTypeEnum.Unknown)
                {
                    // Skip test if account type does not support domains whitelist
                    return;
                }

                await AuthenticatedClient.UpdateVideoMetadataAsync(clipId, new VideoUpdateMetadata
                {
                    EmbedPrivacy = VideoEmbedPrivacyEnum.Whitelist
                });
                var video = await AuthenticatedClient.GetVideoAsync(clipId);
                video.Privacy.EmbedPrivacy.ShouldBe(VideoEmbedPrivacyEnum.Whitelist);

                await AuthenticatedClient.AllowEmbedVideoOnDomainAsync(clipId, "example.com");
                var domains = await AuthenticatedClient.GetAllowedDomainsForEmbeddingVideoAsync(clipId);
                domains.Data.ShouldNotBeNull();
                domains.Data.Count.ShouldBe(1);
                domains.Data[0].ShouldNotBeNull();
                domains.Data[0].Domain.ShouldBe("example.com");

                await AuthenticatedClient.DisallowEmbedVideoOnDomainAsync(clipId, "example.com");
                domains = await AuthenticatedClient.GetAllowedDomainsForEmbeddingVideoAsync(clipId);
                domains.Data.ShouldNotBeNull();
                domains.Data.Count.ShouldBe(0);

                await AuthenticatedClient.UpdateVideoMetadataAsync(clipId, new VideoUpdateMetadata
                {
                    EmbedPrivacy = VideoEmbedPrivacyEnum.Public
                });
            });
        }

        [Fact]
        public async Task ShouldCorrectlyGetPictureFromVideo()
        {
            var pictures = await AuthenticatedClient.GetPicturesAsync(VimeoSettings.PublicVideoId);
            pictures.Data.Count.ShouldBeGreaterThan(0);
            var picture = pictures.Data[0];
            var parts = picture.Uri.Split('/');
            var pictureId = long.Parse(parts[parts.Length - 1]);
            var pictureById = await AuthenticatedClient.GetPictureAsync(VimeoSettings.PublicVideoId, pictureId);
            pictureById.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyAssignEmbedPresetToVideo()
        {
            if (VimeoSettings.EmbedPresetId == 0) {
                return;
            }

            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                await AuthenticatedClient.AssignEmbedPresetToVideoAsync(clipId, VimeoSettings.EmbedPresetId);
                var video = await AuthenticatedClient.GetVideoAsync(clipId, new[] {"embed_presets"});
                video.ShouldNotBeNull();
                video.EmbedPresets.ShouldNotBeNull();
                video.EmbedPresets.Id.ShouldBe(VimeoSettings.EmbedPresetId);
            });
        }

        [Fact]
        public async Task ShouldCorrectlyUnassignEmbedPresetFromVideo()
        {
            if (VimeoSettings.EmbedPresetId == 0)
                return;
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var video = await AuthenticatedClient.GetVideoAsync(clipId, new[] {"embed_presets"});
                var oldPresetId = video?.EmbedPresets?.Id;
                await AuthenticatedClient.UnassignEmbedPresetFromVideoAsync(clipId, VimeoSettings.EmbedPresetId);
                video = await AuthenticatedClient.GetVideoAsync(clipId, new[] {"embed_presets"});
                video.ShouldNotBeNull();
                if (oldPresetId == VimeoSettings.EmbedPresetId)
                {
                    video.EmbedPresets.ShouldBeNull();
                }
                else
                {
                    video.EmbedPresets?.Id.ShouldBe(oldPresetId);
                }
            });
        }
    }
}