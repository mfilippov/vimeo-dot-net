using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Authorization;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;
using VimeoDotNet.Tests.Settings;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class VimeoClientAsyncTests
    {
        private readonly VimeoApiTestSettings _vimeoSettings;

        private const string Testfilepath = @"Resources\test.mp4";
        // http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        private const string Testtexttrackfilepath = @"Resources\test.vtt";

        public VimeoClientAsyncTests()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            _vimeoSettings = SettingsLoader.LoadSettings(); 
        }
        
        [Fact]
        public async Task Integration_VimeoClient_GetReplaceVideoUploadTicket_CanGenerateStreamingTicket()
        {
            var client = CreateAuthenticatedClient();
            var ticket = await client.GetReplaceVideoUploadTicketAsync(_vimeoSettings.VideoId);
            ticket.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUploadTicket_CanGenerateStreamingTicket()
        {
            var client = CreateAuthenticatedClient();
            var ticket = await client.GetUploadTicketAsync();
            ticket.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadEntireFile_UploadsFile()
        {
            long length;
            IUploadRequest completedRequest;
            using (var file = new BinaryContent(GetFullPath(Testfilepath)))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file);
                Debug.Assert(completedRequest.ClipId != null, "completedRequest.ClipId != null");
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }
            completedRequest.ShouldNotBeNull();
            completedRequest.AllBytesWritten.ShouldBeTrue();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Integration_VimeoClient_DeleteVideo_DeletesVideo()
        {
            using (var file = new BinaryContent(GetFullPath(Testfilepath)))
            {
                var length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                var completedRequest = await client.UploadEntireFileAsync(file);
                completedRequest.AllBytesWritten.ShouldBeTrue();
                completedRequest.ShouldNotBeNull();
                completedRequest.IsVerifiedComplete.ShouldBeTrue();
                completedRequest.BytesWritten.ShouldBe(length);
                completedRequest.ClipUri.ShouldNotBeNull();
                completedRequest.ClipId.HasValue.ShouldBeTrue();
                Debug.Assert(completedRequest.ClipId != null, "completedRequest.ClipId != null");
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
                (await client.GetVideoAsync(completedRequest.ClipId.Value)).ShouldBeNull();
            }
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountInformation_RetrievesCurrentAccountInfo()
        {
            var client = CreateAuthenticatedClient();
            var account = await client.GetAccountInformationAsync();
            account.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_UpdateAccountInformation_UpdatesCurrentAccountInfo()
        {
            // first, ensure we can retrieve the current user...
            var client = CreateAuthenticatedClient();
            var original = await client.GetAccountInformationAsync();
            original.ShouldNotBeNull();

            // next, update the user record with some new values...
            var testName = "King Henry VIII";
            var testBio = "";
            var testLocation = "England";

            var updated = await client.UpdateAccountInformationAsync(new EditUserParameters
            {
                Name = testName,
                Bio = testBio,
                Location = testLocation
            });

            // inspect the result and ensure the values match what we expect...
            // the vimeo api will set string fields to null if the value passed in is an empty string
            // so check against null if that is what we are passing in, otherwise, expect the passed value...
            if (string.IsNullOrEmpty(testName))
                updated.name.ShouldBeNull();
            else
                updated.name.ShouldBe(testName);
            if (string.IsNullOrEmpty(testBio))
                updated.bio.ShouldBeNull();
            else
                updated.bio.ShouldBe(testBio);

            if (string.IsNullOrEmpty(testLocation))
                updated.location.ShouldBeNull();
            else
                updated.location.ShouldBe(testLocation);

            // restore the original values...
            var final = await client.UpdateAccountInformationAsync(new EditUserParameters
            {
                Name = original.name ?? string.Empty,
                Bio = original.bio ?? string.Empty,
                Location = original.location ?? string.Empty
            });

            // inspect the result and ensure the values match our originals...
            if (string.IsNullOrEmpty(original.name))
            {
                final.name.ShouldBeNull();
            }
            else
            {
                final.name.ShouldBe(original.name);
            }
                
            if (string.IsNullOrEmpty(original.bio))
            {
                final.bio.ShouldBeNull();
            }
            else
            {
                final.bio.ShouldBe(original.bio);
            }
                
            if (string.IsNullOrEmpty(original.location))
            {
                final.location.ShouldBeNull();
            } 
            else
            {
                final.location.ShouldBe(original.location);
            }
        }


        [Fact]
        public async Task Integration_VimeoClient_GetUserInformation_RetrievesUserInfo()
        {
            var client = CreateAuthenticatedClient();
            var user = await client.GetUserInformationAsync(_vimeoSettings.UserId);
            user.ShouldNotBeNull();
            user.id.ShouldNotBeNull();
            Debug.Assert(user.id != null, "user.id != null");
            user.id.Value.ShouldBe(_vimeoSettings.UserId);
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideos_RetrievesCurrentAccountVideos()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetUserVideosAsync(_vimeoSettings.UserId); 
            videos.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideos_SecondPage()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetVideosAsync(page: 2, perPage: 5);
            videos.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideo_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetVideoAsync(_vimeoSettings.VideoId);
            video.ShouldNotBeNull();
            video.pictures.uri.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountAlbumVideos_RetrievesCurrentAccountAlbumVideos()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetAlbumVideosAsync(_vimeoSettings.AlbumId, 1, null);
            videos.ShouldNotBeNull();
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountAlbumVideo_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetAlbumVideoAsync(_vimeoSettings.AlbumId, _vimeoSettings.VideoId);
            video.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUserAlbumVideos_RetrievesUserAlbumVideos()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetUserAlbumVideosAsync(_vimeoSettings.UserId, _vimeoSettings.AlbumId);
            videos.ShouldNotBeNull();
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUserAlbumVideo_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetUserAlbumVideoAsync(_vimeoSettings.UserId, _vimeoSettings.AlbumId, _vimeoSettings.VideoId);
            video.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountAlbums_NotNull()
        {
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync();
            albums.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_AlbumVideoManagement()
        {
            var client = CreateAuthenticatedClient();

            // assume this album and video are configured in the current account...
            var albumId = _vimeoSettings.AlbumId;
            var videoId = _vimeoSettings.VideoId;

            // then remove it...
            var isRemoved = await client.RemoveFromAlbumAsync(albumId, videoId);
            var removedVideo = await client.GetAlbumVideoAsync(albumId, videoId);
            var isAbsent = removedVideo == null;

            isRemoved.ShouldBeTrue("RemoveFromAlbum failed.");
            isRemoved.ShouldBe(isAbsent, "Returned value does not match actual abscence of video.");

            // add it...
            var isAdded = await client.AddToAlbumAsync(albumId, videoId);
            var addedVideo = await client.GetAlbumVideoAsync(albumId, videoId);
            var isPresent = addedVideo != null;

            isAdded.ShouldBeTrue("AddToAlbum failed.");
            isAdded.ShouldBe(isPresent, "Returned value does not match actual presence of video.");
        }

        [Fact]
        public async Task Integration_VimeoClient_AlbumManagement()
        {
            var client = CreateAuthenticatedClient();
        
            // create a new album...
            const string originalName = "Unit Test Album";
            const string originalDesc = "This album was created via an automated test, and should be deleted momentarily...";

            var newAlbum = await client.CreateAlbumAsync(new EditAlbumParameters
            {
                Name = originalName,
                Description = originalDesc,
                Sort = EditAlbumSortOption.Newest,
                Privacy = EditAlbumPrivacyOption.Password,
                Password = "test"
            });

            newAlbum.ShouldNotBeNull();
            newAlbum.name.ShouldBe(originalName);

            newAlbum.description.ShouldBe(originalDesc);

                // retrieve albums for the current user...there should be at least one now...
            var albums = await client.GetAlbumsAsync();

            albums.total.ShouldBeGreaterThan(0);

            // update the album...
            const string updatedName = "Unit Test Album (Updated)";
            var albumId = newAlbum.GetAlbumId();
            Debug.Assert(albumId != null, $"{nameof(albumId)} != null");
            var updatedAlbum = await client.UpdateAlbumAsync(albumId.Value, new EditAlbumParameters
            {
                Name = updatedName,
                Privacy = EditAlbumPrivacyOption.Anybody
            });

            updatedAlbum.name.ShouldBe(updatedName);

            // delete the album...
            albumId = updatedAlbum.GetAlbumId();
            Debug.Assert(albumId != null, $"{nameof(albumId)} != null");
            var isDeleted = await client.DeleteAlbumAsync(albumId.Value);

            isDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUserAlbums_NotNull()
        {
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync(_vimeoSettings.UserId);
            albums.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetTextTracksAsync()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var texttracks = await client.GetTextTracksAsync(_vimeoSettings.VideoId);
            
            // assert
            texttracks.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetTextTrackAsync()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var texttrack = await client.GetTextTrackAsync(_vimeoSettings.VideoId, _vimeoSettings.TextTrackId);

            // assert
            texttrack.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_UpdateTextTrackAsync()
        {
            var client = CreateAuthenticatedClient();
            var original = await client.GetTextTrackAsync(_vimeoSettings.VideoId, _vimeoSettings.TextTrackId);

            original.ShouldNotBeNull();

            // update the text track record with some new values...
            const string testName = "NewTrackName";
            const string testType = "subtitles";
            const string testLanguage = "fr";
            const bool testActive = false;

            var updated = await client.UpdateTextTrackAsync(
                                    _vimeoSettings.VideoId,
                                    _vimeoSettings.TextTrackId,
                                    new TextTrack
                                    {
                                        name = testName,
                                        type = testType,
                                        language = testLanguage,
                                        active = testActive
                                    });

            // inspect the result and ensure the values match what we expect...
            testName.ShouldBe(updated.name);
            testType.ShouldBe(updated.type);
            testLanguage.ShouldBe(updated.language);
            updated.active.ShouldBeFalse();

            // restore the original values...
            var final = await client.UpdateTextTrackAsync(
                                    _vimeoSettings.VideoId,
                                    _vimeoSettings.TextTrackId,
                                    new TextTrack
                                    {
                                        name = original.name,
                                        type = original.type,
                                        language = original.language,
                                        active = original.active
                                    });

            // inspect the result and ensure the values match our originals...
            if (string.IsNullOrEmpty(original.name))
            {
                final.name.ShouldBeNull();
            }
            else
            {
                original.name.ShouldBe(final.name);
            }

            if (string.IsNullOrEmpty(original.type))
            {
                final.type.ShouldBeNull();
            }
            else
            {
                original.type.ShouldBe(final.type);
            }

            if (string.IsNullOrEmpty(original.language))
            {
                final.language.ShouldBeNull();
            }
            else
            {
                original.language.ShouldBe(final.language);
            }

            original.active.ShouldBe(final.active);
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadTextTrackFileAsync()
        {
            // arrange
            var client = CreateAuthenticatedClient();
            TextTrack completedRequest;
            using (var file = new BinaryContent(GetFullPath(Testtexttrackfilepath)))
            {
                // act
                completedRequest = await client.UploadTextTrackFileAsync(
                                file,
                                _vimeoSettings.VideoId,
                                new TextTrack
                                {
                                    active = false,
                                    name = "UploadTest",
                                    language = "en",
                                    type = "captions"
                                });
            }

            // assert
            completedRequest.ShouldNotBeNull();
            completedRequest.uri.ShouldNotBeNull();

            // cleanup
            var uri = completedRequest.uri;
            var trackId = Convert.ToInt64(uri.Substring(uri.LastIndexOf('/') + 1));
            await client.DeleteTextTrackAsync(_vimeoSettings.VideoId, trackId);
        }

        [Fact]
        public async Task Integration_VimeoClient_DeleteTextTrack()
        {
            // arrange
            TextTrack completedRequest;
            var client = CreateAuthenticatedClient();
            using (var file = new BinaryContent(GetFullPath(Testtexttrackfilepath)))
            {
                completedRequest = await client.UploadTextTrackFileAsync(
                                file,
                                _vimeoSettings.VideoId,
                                new TextTrack
                                {
                                    active = false,
                                    name = "DeleteTest",
                                    language = "en",
                                    type = "captions"
                                });
            }
            completedRequest.ShouldNotBeNull();
            completedRequest.uri.ShouldNotBeNull();
            var uri = completedRequest.uri;
            var trackId = Convert.ToInt64(uri.Substring(uri.LastIndexOf('/') + 1));
            // act
            await client.DeleteTextTrackAsync(_vimeoSettings.VideoId, trackId);

            //assert
            var texttrack = await client.GetTextTrackAsync(_vimeoSettings.VideoId, trackId);
            texttrack.ShouldBeNull();
        }

        [Fact]
        public async Task GetAccountVideoWtihUnauthenticatedToken()
        {
            var client = await CreateUnauthenticatedClient();
            var video = await client.GetVideoAsync(_vimeoSettings.VideoId);
            video.ShouldNotBeNull();
            video.pictures.uri.ShouldNotBeNull();
        }

        private async Task<VimeoClient> CreateUnauthenticatedClient()
        {
            var authorizationClient = new AuthorizationClient(_vimeoSettings.ClientId, _vimeoSettings.ClientSecret);
            var unauthenticatedToken = await authorizationClient.GetUnauthenticatedTokenAsync();
            return new VimeoClient(unauthenticatedToken.access_token);
        }

        private VimeoClient CreateAuthenticatedClient()
        {
            return new VimeoClient(_vimeoSettings.AccessToken);
        }

        private static string GetFullPath(string relativePath)
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory()); // /bin/debug
            return Path.Combine(dir.Parent?.Parent?.FullName ?? throw new InvalidOperationException(), relativePath);
        }
    }
}