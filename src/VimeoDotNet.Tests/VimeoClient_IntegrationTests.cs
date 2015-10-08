using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Tests.Settings;
using VimeoDotNet.Parameters;

namespace VimeoDotNet.Tests
{
    [TestClass]
    //[Ignore] // Comment this line to run integration tests.
    public class VimeoClient_IntegrationTests
    {
        private VimeoApiTestSettings vimeoSettings;

        private const string TESTFILEPATH = @"Resources\test.mp4";
            // http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        [TestInitialize]
        public void SetupTest()
        {
            // Load the settings from a file that is not under version control for security
            // The settings loader will create this file in the bin/ folder if it doesn't exist
            vimeoSettings = Settings.SettingsLoader.LoadSettings(); 
        }


        [TestMethod]
        public void Integration_VimeoClient_GetUploadTicket_CanGenerateStreamingTicket()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            UploadTicket ticket = client.GetUploadTicket();

            // assert
            Assert.IsNotNull(ticket);
        }

        [TestMethod]
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
            }

            // assert
            Assert.IsNotNull(completedRequest);
            Assert.IsTrue(completedRequest.AllBytesWritten);
            Assert.IsTrue(completedRequest.IsVerifiedComplete);
            Assert.AreEqual(length, completedRequest.BytesWritten);
            Assert.IsNotNull(completedRequest.ClipUri);
            Assert.IsTrue(completedRequest.ClipId > 0);
        }

        [TestMethod]
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
                Assert.IsTrue(completedRequest.AllBytesWritten);
                Assert.IsNotNull(completedRequest);
                Assert.IsTrue(completedRequest.IsVerifiedComplete);
                Assert.AreEqual(length, completedRequest.BytesWritten);
                Assert.IsNotNull(completedRequest.ClipUri);
                Assert.IsTrue(completedRequest.ClipId.HasValue);
                client.DeleteVideo(completedRequest.ClipId.Value);
                Assert.IsNull(client.GetVideo(completedRequest.ClipId.Value));
            }
            // assert            
        }

        [TestMethod]
        public void Integration_VimeoClient_GetAccountInformation_RetrievesCurrentAccountInfo()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            User account = client.GetAccountInformation();

            // assert
            Assert.IsNotNull(account);
        }

		[TestMethod]
		public void Integration_VimeoClient_UpdateAccountInformation_UpdatesCurrentAccountInfo()
		{
			// first, ensure we can retrieve the current user...
			VimeoClient client = CreateAuthenticatedClient();
			User original = client.GetAccountInformation();
			Assert.IsNotNull(original);

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
				Assert.IsNull(updated.name);
			else
				Assert.AreEqual(testName, updated.name);

			if (string.IsNullOrEmpty(testBio))
				Assert.IsNull(updated.bio);
			else
				Assert.AreEqual(testBio, updated.bio);

			if (string.IsNullOrEmpty(testLocation))
				Assert.IsNull(updated.location);
			else
				Assert.AreEqual(testLocation, updated.location);

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
				Assert.IsNull(final.name);
			}
			else
			{
				Assert.AreEqual(original.name, final.name);
			}
				
			if (string.IsNullOrEmpty(original.bio))
			{
				Assert.IsNull(final.bio);
			}
			else
			{
				Assert.AreEqual(original.bio, final.bio);
			}
				
			if (string.IsNullOrEmpty(original.location))
			{
				Assert.IsNull(final.location);
			} 
			else
			{
				Assert.AreEqual(original.location, final.location);
			}			
		}


        [TestMethod]
        public void Integration_VimeoClient_GetUserInformation_RetrievesUserInfo()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            User user = client.GetUserInformation(vimeoSettings.UserId);

            // assert
            Assert.IsNotNull(user);
            Assert.AreEqual(vimeoSettings.UserId, user.id.Value);
        }

        [TestMethod]
        public void Integration_VimeoClient_GetAccountVideos_RetrievesCurrentAccountVideos()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Paginated<Video> videos = client.GetUserVideos(vimeoSettings.UserId); 

            // assert
            Assert.IsNotNull(videos);
        }

        [TestMethod]
        public async Task Integration_VimeoClient_GetAccountVideos_SecondPage()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Paginated<Video> videos = await client.GetVideosAsync(page: 2, perPage: 5);

            // assert
            Assert.IsNotNull(videos);
        }

        [TestMethod]
        public void Integration_VimeoClient_GetAccountVideo_RetrievesVideo()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Video video = client.GetVideo(vimeoSettings.VideoId);

            // assert
            Assert.IsNotNull(video);
            Assert.IsTrue(video.pictures.Any(a => a.uri != null));
        }

        [TestMethod]
        public void Integration_VimeoClient_GetAccountAlbumVideos_RetrievesCurrentAccountAlbumVideos()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Paginated<Video> videos = client.GetAlbumVideos(vimeoSettings.AlbumId, 1, null);

            // assert
            Assert.IsNotNull(videos);
            Assert.AreNotEqual(videos.data.Count, 0);
        }

        [TestMethod]
        public void Integration_VimeoClient_GetAccountAlbumVideo_RetrievesVideo()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Video video = client.GetAlbumVideo(vimeoSettings.AlbumId, vimeoSettings.VideoId);

            // assert
            Assert.IsNotNull(video);
        }

        [TestMethod]
        public void Integration_VimeoClient_GetUserAlbumVideos_RetrievesUserAlbumVideos()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Paginated<Video> videos = client.GetUserAlbumVideos(vimeoSettings.UserId, vimeoSettings.AlbumId);

            // assert
            Assert.IsNotNull(videos);
            Assert.AreNotEqual(videos.data.Count, 0);
        }

        [TestMethod]
        public void Integration_VimeoClient_GetUserAlbumVideo_RetrievesVideo()
        {
            // arrange
            VimeoClient client = CreateAuthenticatedClient();

            // act
            Video video = client.GetUserAlbumVideo(vimeoSettings.UserId, vimeoSettings.AlbumId, vimeoSettings.VideoId);

            // assert
            Assert.IsNotNull(video);
        }

		[TestMethod]
		public void Integration_VimeoClient_GetAccountAlbums_NotNull()
		{
			// arrange
			VimeoClient client = CreateAuthenticatedClient();

			// act
			Paginated<Album> albums = client.GetAccountAlbums();

			// assert
			Assert.IsNotNull(albums);
		}

		[TestMethod]
		public void Integration_VimeoClient_GetUserAlbums_NotNull()
		{
			// arrange
			VimeoClient client = CreateAuthenticatedClient();

			// act
			Paginated<Album> albums = client.GetUserAlbums(vimeoSettings.UserId);

			// assert
			Assert.IsNotNull(albums);
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
            var dir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)); // /bin/debug
            return Path.Combine(dir.Parent.Parent.FullName, relativePath);
        }
    }
}