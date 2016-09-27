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
    //[Ignore] // Comment this line to run integration tests.
    public class VimeoClient_IntegrationTests
    {
        private VimeoApiTestSettings vimeoSettings;

		private const string TESTFILEPATH = @"Resources\test.mp4";
		// http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        public VimeoClient_IntegrationTests()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            vimeoSettings = Settings.SettingsLoader.LoadSettings(); 
        }
        
        [Fact]
        public void Integration_VimeoClient_GetReplaceVideoUploadTicket_CanGenerateStreamingTicket()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            UploadTicket ticket = client.GetReplaceVideoUploadTicket(vimeoSettings.VideoId);

            // assert
            ticket.ShouldNotBeNull();
        }

        [Fact]
        public void Integration_VimeoClient_GetUploadTicket_CanGenerateStreamingTicket()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            UploadTicket ticket = client.GetUploadTicket();

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
                VimeoClient client = CreateAuthenticatedClient();

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
                VimeoClient client = CreateAuthenticatedClient();
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
            VimeoClient client = CreateAuthenticatedClient();

            // act
            User account = client.GetAccountInformation();

            // assert
            account.ShouldNotBeNull();
        }

		[Fact]
		public void Integration_VimeoClient_UpdateAccountInformation_UpdatesCurrentAccountInfo()
		{
			// first, ensure we can retrieve the current user...
			VimeoClient client = CreateAuthenticatedClient();
			User original = client.GetAccountInformation();
			original.ShouldNotBeNull();

			// next, update the user record with some new values...
			string testName = "King Henry VIII";
			string testBio = "";
			string testLocation = "England";

			User updated = client.UpdateAccountInformation(new EditUserParameters
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
			User final = client.UpdateAccountInformation(new Parameters.EditUserParameters
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
            VimeoClient client = CreateAuthenticatedClient();

            // act
            User user = client.GetUserInformation(vimeoSettings.UserId);

            // assert
            user.ShouldNotBeNull();
            user.id.Value.ShouldEqual(vimeoSettings.UserId);
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountVideos_RetrievesCurrentAccountVideos()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Paginated<Video> videos = client.GetUserVideos(vimeoSettings.UserId); 

            // assert
            videos.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideos_SecondPage()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Paginated<Video> videos = await client.GetVideosAsync(page: 2, perPage: 5);

            // assert
            videos.ShouldNotBeNull();
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountVideo_RetrievesVideo()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Video video = client.GetVideo(vimeoSettings.VideoId);

            // assert
            video.ShouldNotBeNull();
            video.pictures.Any(a => a.uri != null).ShouldBeTrue();
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountAlbumVideos_RetrievesCurrentAccountAlbumVideos()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Paginated<Video> videos = client.GetAlbumVideos(vimeoSettings.AlbumId, 1, null);

            // assert
            videos.ShouldNotBeNull();
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void Integration_VimeoClient_GetAccountAlbumVideo_RetrievesVideo()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Video video = client.GetAlbumVideo(vimeoSettings.AlbumId, vimeoSettings.VideoId);

            // assert
            video.ShouldNotBeNull();
        }

        [Fact]
        public void Integration_VimeoClient_GetUserAlbumVideos_RetrievesUserAlbumVideos()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Paginated<Video> videos = client.GetUserAlbumVideos(vimeoSettings.UserId, vimeoSettings.AlbumId);

            // assert
            videos.ShouldNotBeNull();;
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void Integration_VimeoClient_GetUserAlbumVideo_RetrievesVideo()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Video video = client.GetUserAlbumVideo(vimeoSettings.UserId, vimeoSettings.AlbumId, vimeoSettings.VideoId);

            // assert
            video.ShouldNotBeNull();
        }

		[Fact]
		public void Integration_VimeoClient_GetAccountAlbums_NotNull()
		{
			// arrange
			VimeoClient client = CreateAuthenticatedClient();

			// act
			Paginated<Album> albums = client.GetAlbums();

			// assert
			albums.ShouldNotBeNull();
		}

		[Fact]
		public void Integration_VimeoClient_AlbumVideoManagement()
		{
			VimeoClient client = CreateAuthenticatedClient();

			// assume this album and video are configured in the current account...
			long albumId = vimeoSettings.AlbumId;
			long videoId = vimeoSettings.VideoId;

			// then remove it...
			bool isRemoved = client.RemoveFromAlbum(albumId, videoId);
			Video removedVideo = client.GetAlbumVideo(albumId, videoId);
			bool isAbsent = removedVideo == null;

			isRemoved.ShouldBeTrue("RemoveFromAlbum failed.");
			isRemoved.ShouldEqual(isAbsent, "Returned value does not match actual abscence of video.");

		    // add it...
		    bool isAdded = client.AddToAlbum(albumId, videoId);
		    Video addedVideo = client.GetAlbumVideo(albumId, videoId);
		    bool isPresent = addedVideo != null;

		    isAdded.ShouldBeTrue("AddToAlbum failed.");
		    isAdded.ShouldEqual(isPresent, "Returned value does not match actual presence of video.");
		}

		[Fact]
		public void Integration_VimeoClient_AlbumManagement()
		{
			VimeoClient client = CreateAuthenticatedClient();
		
			// create a new album...
			string originalName = "Unit Test Album";
			string originalDesc = "This album was created via an automated test, and should be deleted momentarily...";

			Album newAlbum = client.CreateAlbum(new EditAlbumParameters()
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
			Paginated<Album> albums = client.GetAlbums();

			albums.total.ShouldBeGreaterThan(0);

			// update the album...
			string updatedName = "Unit Test Album (Updated)";
			Album updatedAlbum = client.UpdateAlbum(newAlbum.GetAlbumId().Value, new EditAlbumParameters()
			{
				Name = updatedName,
				Privacy = EditAlbumPrivacyOption.Anybody
			});

			updatedAlbum.name.ShouldEqual(updatedName);

			// delete the album...
			bool isDeleted = client.DeleteAlbum(updatedAlbum.GetAlbumId().Value);

			isDeleted.ShouldBeTrue();


		}

		[Fact]
		public void Integration_VimeoClient_GetUserAlbums_NotNull()
		{
			// arrange
			VimeoClient client = CreateAuthenticatedClient();

			// act
			Paginated<Album> albums = client.GetAlbums(vimeoSettings.UserId);

			// assert
			albums.ShouldNotBeNull();
		}

		[TestMethod]
		public async Task Integration_VimeoClient_GetTextTracksAsync()
		{
			// arrange
			VimeoClient client = CreateAuthenticatedClient();

			// act
			var texttracks = await client.GetTextTracksAsync(vimeoSettings.VideoId);
			
			// assert
			Assert.IsNotNull(texttracks);
		}

		[TestMethod]
		public async Task Integration_VimeoClient_GetTextTrackAsync()
		{
			// arrange
			VimeoClient client = CreateAuthenticatedClient();

			// act
			var texttrack = await client.GetTextTrackAsync(vimeoSettings.VideoId, vimeoSettings.TextTrackId);

			// assert
			Assert.IsNotNull(texttrack);
		}

		[TestMethod]
		public async Task Integration_VimeoClient_UpdateTextTrackAsync()
		{
			// arrange
			VimeoClient client = CreateAuthenticatedClient();
			var original = await client.GetTextTrackAsync(vimeoSettings.VideoId, vimeoSettings.TextTrackId);

			Assert.IsNotNull(original);

			// act
			// update the text track record with some new values...
			var testName = "NewTrackName";
			var testType = "subtitles";
			var testLanguage = "fr";
			var testActive = false;

			var updated = await client.UpdateTextTrackAsync(
									vimeoSettings.VideoId,
									vimeoSettings.TextTrackId,
									new TextTrack
									{
										name = testName,
										type = testType,
										language = testLanguage,
										active = testActive
									});

			// inspect the result and ensure the values match what we expect...
			// assert
			Assert.AreEqual(testName, updated.name);
			Assert.AreEqual(testType, updated.type);
			Assert.AreEqual(testLanguage, updated.language);
			Assert.AreEqual(testActive, updated.active);

			// restore the original values...
			var final = await client.UpdateTextTrackAsync(
									vimeoSettings.VideoId,
									vimeoSettings.TextTrackId,
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
				Assert.IsNull(final.name);
			}
			else
			{
				Assert.AreEqual(original.name, final.name);
			}

			if (string.IsNullOrEmpty(original.type))
			{
				Assert.IsNull(final.type);
			}
			else
			{
				Assert.AreEqual(original.type, final.type);
			}

			if (string.IsNullOrEmpty(original.language))
			{
				Assert.IsNull(final.language);
			}
			else
			{
				Assert.AreEqual(original.language, final.language);
			}

			Assert.AreEqual(original.active, final.active);
		}

		[TestMethod]
		public async Task Integration_VimeoClient_UploadTextTrackFileAsync()
		{
			// arrange
			VimeoClient client = CreateAuthenticatedClient();
			TextTrack completedRequest;
			using (var file = new BinaryContent(GetFullPath(TESTTEXTTRACKFILEPATH)))
			{
				// act
				completedRequest = await client.UploadTextTrackFileAsync(
								file,
								vimeoSettings.VideoId,
								new TextTrack
								{
									active = false,
									name = "UploadTest",
									language = "en",
									type = "captions"
								});
			}

			// assert
			Assert.IsNotNull(completedRequest);
			Assert.IsNotNull(completedRequest.uri);
		}

		[TestMethod]
		public async Task Integration_VimeoClient_DeleteTextTrack()
		{
			// arrange
			TextTrack completedRequest;
			VimeoClient client = CreateAuthenticatedClient();
			using (var file = new BinaryContent(GetFullPath(TESTTEXTTRACKFILEPATH)))
			{
				completedRequest = await client.UploadTextTrackFileAsync(
								file,
								vimeoSettings.VideoId,
								new TextTrack
								{
									active = false,
									name = "DeleteTest",
									language = "en",
									type = "captions"
								});
			}
			Assert.IsNotNull(completedRequest);
			Assert.IsNotNull(completedRequest.uri);
			var uri = completedRequest.uri;
			var trackId = System.Convert.ToInt64(uri.Substring(uri.LastIndexOf('/') + 1));
			// act
			await client.DeleteTextTrackAsync(vimeoSettings.VideoId, trackId);

			//assert
			var texttrack = await client.GetTextTrackAsync(vimeoSettings.VideoId, trackId);
			Assert.IsNull(texttrack);
		}

		private VimeoClient CreateUnauthenticatedClient()
        {
            return new VimeoClient(vimeoSettings.ClientId, vimeoSettings.ClientSecret);
        }

        private VimeoClient CreateAuthenticatedClient()
        {
            return new VimeoClient(vimeoSettings.AccessToken);
        }

        private string GetFullPath(string relativePath)
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory()); // /bin/debug
            return Path.Combine(dir.Parent.Parent.FullName, relativePath);
        }
    }
}