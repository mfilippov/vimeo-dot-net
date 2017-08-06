using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Should;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Tests.Settings;
using VimeoDotNet.Parameters;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class VimeoClientIntegrationTests
    {
        private readonly VimeoApiTestSettings _vimeoSettings;

		private const string TESTFILEPATH = @"Resources\test.mp4";
		// http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

		private const string TESTTEXTTRACKFILEPATH = @"Resources\test.vtt";

        public VimeoClientIntegrationTests()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            _vimeoSettings = Settings.SettingsLoader.LoadSettings(); 
        }
        
        [Fact]
        public void Integration_VimeoClient_GetReplaceVideoUploadTicket_CanGenerateStreamingTicket()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var ticket = client.GetReplaceVideoUploadTicket(_vimeoSettings.VideoId);

            // assert
            ticket.ShouldNotBeNull();
        }

        [Fact]
        public void Integration_VimeoClient_GetUploadTicket_CanGenerateStreamingTicket()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var ticket = client.GetUploadTicket();

            // assert
            ticket.ShouldNotBeNull();
        }

        [Fact]
        public void Integration_VimeoClient_UploadEntireFile_UploadsFile()
        {
            // arrange
            long length;
            IUploadRequest completedRequest;
            using (var file = new BinaryContent(GetFullPath(TESTFILEPATH)))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();

                // act
                completedRequest = client.UploadEntireFile(file);

                Debug.Assert(completedRequest.ClipId != null, "completedRequest.ClipId != null");
                client.DeleteVideo(completedRequest.ClipId.Value);
            }

            // assert
            completedRequest.ShouldNotBeNull();
            completedRequest.AllBytesWritten.ShouldBeTrue();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldEqual(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void Integration_VimeoClient_DeleteVideo_DeletesVideo()
        {
            // arrange
            long length;
            IUploadRequest completedRequest;
            using (var file = new BinaryContent(GetFullPath(TESTFILEPATH)))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                // act
                completedRequest = client.UploadEntireFile(file);
                completedRequest.AllBytesWritten.ShouldBeTrue();
                completedRequest.ShouldNotBeNull();
                completedRequest.IsVerifiedComplete.ShouldBeTrue();
                completedRequest.BytesWritten.ShouldEqual(length);
                completedRequest.ClipUri.ShouldNotBeNull();
                completedRequest.ClipId.HasValue.ShouldBeTrue();
                Debug.Assert(completedRequest.ClipId != null, "completedRequest.ClipId != null");
                client.DeleteVideo(completedRequest.ClipId.Value);
                client.GetVideo(completedRequest.ClipId.Value).ShouldBeNull();
            }
            // assert            
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountInformation_RetrievesCurrentAccountInfo()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var account = client.GetAccountInformation();

            // assert
            account.ShouldNotBeNull();
        }

		[Fact]
		public void Integration_VimeoClient_UpdateAccountInformation_UpdatesCurrentAccountInfo()
		{
			// first, ensure we can retrieve the current user...
			var client = CreateAuthenticatedClient();
			var original = client.GetAccountInformation();
			original.ShouldNotBeNull();

			// next, update the user record with some new values...
			var testName = "King Henry VIII";
			var testBio = "";
			var testLocation = "England";

			var updated = client.UpdateAccountInformation(new EditUserParameters
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
		        updated.name.ShouldEqual(testName);
			if (string.IsNullOrEmpty(testBio))
				updated.bio.ShouldBeNull();
			else
		        updated.bio.ShouldEqual(testBio);

			if (string.IsNullOrEmpty(testLocation))
		        updated.location.ShouldBeNull();
			else
		        updated.location.ShouldEqual(testLocation);

			// restore the original values...
			var final = client.UpdateAccountInformation(new Parameters.EditUserParameters
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
				final.name.ShouldEqual(original.name);
			}
				
			if (string.IsNullOrEmpty(original.bio))
			{
				final.bio.ShouldBeNull();
			}
			else
			{
				final.bio.ShouldEqual(original.bio);
			}
				
			if (string.IsNullOrEmpty(original.location))
			{
				final.location.ShouldBeNull();
			} 
			else
			{
				final.location.ShouldEqual(original.location);
			}			
		}


        [Fact]
        public void Integration_VimeoClient_GetUserInformation_RetrievesUserInfo()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var user = client.GetUserInformation(_vimeoSettings.UserId);

            // assert
            user.ShouldNotBeNull();
            user.id.Value.ShouldEqual(_vimeoSettings.UserId);
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountVideos_RetrievesCurrentAccountVideos()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var videos = client.GetUserVideos(_vimeoSettings.UserId); 

            // assert
            videos.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideos_SecondPage()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var videos = await client.GetVideosAsync(page: 2, perPage: 5);

            // assert
            videos.ShouldNotBeNull();
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountVideo_RetrievesVideo()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var video = client.GetVideo(_vimeoSettings.VideoId);

            // assert
            video.ShouldNotBeNull();
            video.pictures.Any(a => a.uri != null).ShouldBeTrue();
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountAlbumVideos_RetrievesCurrentAccountAlbumVideos()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var videos = client.GetAlbumVideos(_vimeoSettings.AlbumId, 1, null);

            // assert
            videos.ShouldNotBeNull();
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountAlbumVideo_RetrievesVideo()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var video = client.GetAlbumVideo(_vimeoSettings.AlbumId, _vimeoSettings.VideoId);

            // assert
            video.ShouldNotBeNull();
        }

        [Fact]
        public void Integration_VimeoClient_GetUserAlbumVideos_RetrievesUserAlbumVideos()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var videos = client.GetUserAlbumVideos(_vimeoSettings.UserId, _vimeoSettings.AlbumId);

            // assert
            videos.ShouldNotBeNull();;
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void Integration_VimeoClient_GetUserAlbumVideo_RetrievesVideo()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var video = client.GetUserAlbumVideo(_vimeoSettings.UserId, _vimeoSettings.AlbumId, _vimeoSettings.VideoId);

            // assert
            video.ShouldNotBeNull();
        }

		[Fact]
		public void Integration_VimeoClient_GetAccountAlbums_NotNull()
		{
			// arrange
			var client = CreateAuthenticatedClient();

			// act
			var albums = client.GetAlbums();

			// assert
			albums.ShouldNotBeNull();
		}

		[Fact]
		public void Integration_VimeoClient_AlbumVideoManagement()
		{
			var client = CreateAuthenticatedClient();

			// assume this album and video are configured in the current account...
			var albumId = _vimeoSettings.AlbumId;
			var videoId = _vimeoSettings.VideoId;

			// then remove it...
			var isRemoved = client.RemoveFromAlbum(albumId, videoId);
			var removedVideo = client.GetAlbumVideo(albumId, videoId);
			var isAbsent = removedVideo == null;

			isRemoved.ShouldBeTrue("RemoveFromAlbum failed.");
			isRemoved.ShouldEqual(isAbsent, "Returned value does not match actual abscence of video.");

		    // add it...
		    var isAdded = client.AddToAlbum(albumId, videoId);
		    var addedVideo = client.GetAlbumVideo(albumId, videoId);
		    var isPresent = addedVideo != null;

		    isAdded.ShouldBeTrue("AddToAlbum failed.");
		    isAdded.ShouldEqual(isPresent, "Returned value does not match actual presence of video.");
		}

		[Fact]
		public void Integration_VimeoClient_AlbumManagement()
		{
			var client = CreateAuthenticatedClient();
		
			// create a new album...
			const string originalName = "Unit Test Album";
			const string originalDesc = "This album was created via an automated test, and should be deleted momentarily...";

			var newAlbum = client.CreateAlbum(new EditAlbumParameters()
			{
				Name = originalName,
				Description = originalDesc,
				Sort = EditAlbumSortOption.Newest,
				Privacy = EditAlbumPrivacyOption.Password,
				Password = "test"
			});

			newAlbum.ShouldNotBeNull();
		    newAlbum.name.ShouldEqual(originalName);

		    newAlbum.description.ShouldEqual(originalDesc);

		        // retrieve albums for the current user...there should be at least one now...
			var albums = client.GetAlbums();

			albums.total.ShouldBeGreaterThan(0);

			// update the album...
			const string updatedName = "Unit Test Album (Updated)";
			var updatedAlbum = client.UpdateAlbum(newAlbum.GetAlbumId().Value, new EditAlbumParameters()
			{
				Name = updatedName,
				Privacy = EditAlbumPrivacyOption.Anybody
			});

			updatedAlbum.name.ShouldEqual(updatedName);

			// delete the album...
			var isDeleted = client.DeleteAlbum(updatedAlbum.GetAlbumId().Value);

			isDeleted.ShouldBeTrue();
		}

		[Fact]
		public void Integration_VimeoClient_GetUserAlbums_NotNull()
		{
			// arrange
			var client = CreateAuthenticatedClient();

			// act
			var albums = client.GetAlbums(_vimeoSettings.UserId);

			// assert
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
			// arrange
			var client = CreateAuthenticatedClient();
			var original = await client.GetTextTrackAsync(_vimeoSettings.VideoId, _vimeoSettings.TextTrackId);

			original.ShouldNotBeNull();

			// act
			// update the text track record with some new values...
			var testName = "NewTrackName";
			var testType = "subtitles";
			var testLanguage = "fr";
			var testActive = false;

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
			// assert
			testName.ShouldEqual(updated.name);
			testType.ShouldEqual(updated.type);
			testLanguage.ShouldEqual(updated.language);
			testActive.ShouldEqual(updated.active);

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
				original.name.ShouldEqual(final.name);
			}

			if (string.IsNullOrEmpty(original.type))
			{
				final.type.ShouldBeNull();
			}
			else
			{
				original.type.ShouldEqual(final.type);
			}

			if (string.IsNullOrEmpty(original.language))
			{
				final.language.ShouldBeNull();
			}
			else
			{
				original.language.ShouldEqual(final.language);
			}

			original.active.ShouldEqual(final.active);
		}

		[Fact]
		public async Task Integration_VimeoClient_UploadTextTrackFileAsync()
		{
			// arrange
			var client = CreateAuthenticatedClient();
			TextTrack completedRequest;
			using (var file = new BinaryContent(GetFullPath(TESTTEXTTRACKFILEPATH)))
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
		    var trackId = System.Convert.ToInt64(uri.Substring(uri.LastIndexOf('/') + 1));
		    await client.DeleteTextTrackAsync(_vimeoSettings.VideoId, trackId);
		}

		[Fact]
		public async Task Integration_VimeoClient_DeleteTextTrack()
		{
			// arrange
			TextTrack completedRequest;
			var client = CreateAuthenticatedClient();
			using (var file = new BinaryContent(GetFullPath(TESTTEXTTRACKFILEPATH)))
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
			var trackId = System.Convert.ToInt64(uri.Substring(uri.LastIndexOf('/') + 1));
			// act
			await client.DeleteTextTrackAsync(_vimeoSettings.VideoId, trackId);

			//assert
			var texttrack = await client.GetTextTrackAsync(_vimeoSettings.VideoId, trackId);
			texttrack.ShouldBeNull();
		}

		private VimeoClient CreateUnauthenticatedClient()
        {
            return new VimeoClient(_vimeoSettings.ClientId, _vimeoSettings.ClientSecret);
        }

        private VimeoClient CreateAuthenticatedClient()
        {
            return new VimeoClient(_vimeoSettings.AccessToken);
        }

        private string GetFullPath(string relativePath)
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory()); // /bin/debug
            return Path.Combine(dir.Parent.Parent.FullName, relativePath);
        }
    }
}