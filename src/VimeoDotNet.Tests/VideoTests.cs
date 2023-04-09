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
            const int clipId = 530969457;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                ResponseJsonFile = "Video.video-530969457.json"
            });
            var video = await AuthenticatedClient.GetVideoAsync(clipId);
            video.ShouldNotBeNull();
            video.Id.ShouldBe(clipId);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrievesVideosByUserId()
        {
            const long userId = 2433258;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/users/{userId}/videos",
                ResponseJsonFile = "Video.user-videos.json"
            });
            var videos = await AuthenticatedClient.GetVideosAsync(userId);
            videos.ShouldNotBeNull();
            videos.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyRetrievesVideosByMe()
        {
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/me/videos",
                ResponseJsonFile = "Video.user-videos.json"
            });
            var videos = await AuthenticatedClient.GetVideosAsync(UserId.Me);
            videos.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyRetrieveSecondPage()
        {
            const long userId = 2433258;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/users/{userId}/videos?page=2&per_page=1",
                StatusCode = 400,
                ResponseJsonFile = "Video.page-not-exists.json"
            });
            try
            {
                var videos = await AuthenticatedClient.GetVideosAsync(userId, 2, 1);
                videos.ShouldNotBeNull();
            }
            catch (VimeoApiException ex)
            {
                ex.Message.ShouldContain("There isn't enough content to display the page you requested.");
            }
        }

        [Fact]
        public async Task ShouldCorrectlyGetVideoWithFields()
        {
            const int clipId = 530969457;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/videos/530969457?fields=uri,name",
                ResponseJsonFile = "Video.videos-with-fields.json"
            });
            var video = await AuthenticatedClient.GetVideoAsync(clipId, new[] {"uri", "name"});
            video.ShouldNotBeNull();
            video.Uri.ShouldNotBeNull();
            video.Name.ShouldNotBeNull();
            video.Pictures.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetUserAlbumVideosByUserId()
        {
            const int albumId = 10303877;
            const int clipId = 530969457;
            const long userId = 2433258;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/users/2433258/albums/10303877/videos/530969457",
                StatusCode = 204,
                Method = RequestSettings.HttpMethod.Put
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/users/2433258/albums/10303877/videos",
                ResponseJsonFile = "Video.album-10303877.json"
            });
            await AuthenticatedClient.AddToAlbumAsync(userId, albumId, clipId);
            var videos = await AuthenticatedClient.GetAlbumVideosAsync(userId, albumId);
            videos.ShouldNotBeNull();
            videos.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGetUserAlbumVideosByMe()
        {
            const int albumId = 10303877;
            const int clipId = 530969457;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/albums/10303877/videos/530969457",
                StatusCode = 204,
                Method = RequestSettings.HttpMethod.Put
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/albums/10303877/videos",
                ResponseJsonFile = "Video.album-10303877.json"
            });
            await AuthenticatedClient.AddToAlbumAsync(UserId.Me, albumId, clipId);
            var videos = await AuthenticatedClient.GetAlbumVideosAsync(UserId.Me, albumId);
            videos.ShouldNotBeNull();
            videos.Data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGetAccountAlbumVideosWithFields()
        {
            const int albumId = 10303877;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/me/albums/{albumId}/videos?fields=uri,name",
                ResponseJsonFile = "Video.album-10303877-videos-with-fields.json"
            });
            var videos =
                await AuthenticatedClient.GetAlbumVideosAsync(UserId.Me, albumId, fields: new[] {"uri", "name"});
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
            const int clipId = 530969457;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/pictures",
                ResponseJsonFile = "Video.video-530969457-pictures.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/pictures/1651999932",
                ResponseJsonFile = "Video.video-530969457-pictures-1651999932.json"
            });
            var pictures = await AuthenticatedClient.GetPicturesAsync(clipId);
            pictures.ShouldNotBeNull();
            pictures.Data.Count.ShouldBeGreaterThan(0);
            var uriParts = pictures.Data[0].Uri.Split('/');
            var pictureId = long.Parse(uriParts[uriParts.Length - 1]);
            var picture = await AuthenticatedClient.GetPictureAsync(clipId, pictureId);
            picture.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetAccountVideoWithUnauthenticatedToken()
        {
            const int clipId = 417178750;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/oauth/authorize/client",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = "grant_type=client_credentials",
                ResponseJsonFile = "User.unauthenticated-token.json",
                AuthBypass = true
            });
            MockHttpRequest(new RequestSettings
            {
                AuthBypass = true,
                UrlSuffix = $"/videos/{clipId}",
                ResponseJsonFile = "Video.video-417178750.json"
            });
            var client = await CreateUnauthenticatedClient();
            var video = await client.GetVideoAsync(clipId);
            video.ShouldNotBeNull();
            video.Pictures.Uri.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyUpdateVideoEmbedPrivacy()
        {
            const int clipId = 530969457;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                ResponseJsonFile = "Video.video-530969457.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/videos/530969457",
                Method = RequestSettings.HttpMethod.Patch,
                RequestTextBody = "privacy.embed=private",
                ResponseJsonFile = "Video.patch-530969457-embed-private-true.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                ResponseJsonFile = "Video.video-530969457-after-patch-embed-private-true.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/videos/530969457",
                Method = RequestSettings.HttpMethod.Patch,
                RequestTextBody = "privacy.embed=public",
                ResponseJsonFile = "Video.patch-530969457-embed-public-true.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                ResponseJsonFile = "Video.video-530969457-after-patch-embed-public-true.json"
            });
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
         
        }

        [Fact]
        public async Task ShouldCorrectlyWorkWithDomainsForEmbedding()
        {
            const int clipId = 530969457;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                Method = RequestSettings.HttpMethod.Patch,
                RequestTextBody = "privacy.embed=whitelist",
                ResponseJsonFile = "Video.patch-530969457-embed-whitelist-true.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                Method = RequestSettings.HttpMethod.Get,
                ResponseJsonFile = "Video.patch-530969457-embed-whitelist-true.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/privacy/domains/example.com",
                Method = RequestSettings.HttpMethod.Put,
                StatusCode = 204
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/privacy/domains",
                Method = RequestSettings.HttpMethod.Get,
                ResponseJsonFile = "Video.video-530969457-domains-after-put.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/privacy/domains/example.com",
                Method = RequestSettings.HttpMethod.Delete,
                StatusCode = 204
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/privacy/domains",
                Method = RequestSettings.HttpMethod.Get,
                ResponseJsonFile = "Video.video-530969457-domains-after-delete.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                Method = RequestSettings.HttpMethod.Patch,
                RequestTextBody = "privacy.embed=public",
                ResponseJsonFile = "Video.patch-530969457-embed-public-true.json"
            });
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
        }

        [Fact]
        public async Task ShouldCorrectlyGetPictureFromVideo()
        {
            const int clipId = 530969457;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                Method = RequestSettings.HttpMethod.Patch,
                RequestTextBody = "privacy.embed=whitelist",
                ResponseJsonFile = "Video.video-530969457.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/pictures",
                ResponseJsonFile = "Video.video-530969457-pictures.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/pictures/1651999932",
                ResponseJsonFile = "Video.video-530969457-pictures-1651999932.json"
            });

            var pictures = await AuthenticatedClient.GetPicturesAsync(clipId);
            pictures.Data.Count.ShouldBeGreaterThan(0);
            var picture = pictures.Data[0];
            var parts = picture.Uri.Split('/');
            var pictureId = long.Parse(parts[parts.Length - 1]);
            var pictureById = await AuthenticatedClient.GetPictureAsync(clipId, pictureId);
            pictureById.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyAssignEmbedPresetToVideo()
        {
            const int clipId = 530969457;
            const int embedPresetId = 120476914;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/presets/{embedPresetId}",
                Method = RequestSettings.HttpMethod.Put,
                StatusCode = 204
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}?fields=embed_presets",
                ResponseJsonFile = "Video.after-put-video-530969457-presets-120476914.json"
            });
           
            await AuthenticatedClient.AssignEmbedPresetToVideoAsync(clipId, embedPresetId);
            var video = await AuthenticatedClient.GetVideoAsync(clipId, new[] {"embed_presets"});
            video.ShouldNotBeNull();
            video.EmbedPresets.ShouldNotBeNull();
            video.EmbedPresets.Id.ShouldBe(embedPresetId);
        }

        [Fact]
        public async Task ShouldCorrectlyUnassignEmbedPresetFromVideo()
        {
            const int clipId = 530969457;
            const int embedPresetId = 120476914;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}?fields=embed_presets",
                ResponseJsonFile = "Video.after-put-video-530969457-presets-120476914.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/presets/{embedPresetId}",
                Method = RequestSettings.HttpMethod.Delete,
                StatusCode = 204
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}?fields=embed_presets",
                ResponseJsonFile = "Video.after-delete-video-530969457-presets-120476914.json"
            });
            var video = await AuthenticatedClient.GetVideoAsync(clipId, new[] {"embed_presets"});
            var oldPresetId = video?.EmbedPresets?.Id;
            await AuthenticatedClient.UnassignEmbedPresetFromVideoAsync(clipId, embedPresetId);
            video = await AuthenticatedClient.GetVideoAsync(clipId, new[] {"embed_presets"});
            video.ShouldNotBeNull();
            if (oldPresetId == embedPresetId)
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