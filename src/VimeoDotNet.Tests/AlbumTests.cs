using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Models;
using VimeoDotNet.Parameters;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class AlbumTests : BaseTest
    {
        [Fact]
        public async Task GetAlbumsShouldCorrectlyWorkForMe()
        {
            MockHttpRequest("/me/albums", "GET",string.Empty, GetJson("Album.albums.json"));
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync(UserId.Me);
            albums.Total.ShouldBe(1);
            albums.PerPage.ShouldBe(25);
            albums.Data.Count.ShouldBe(1);
            albums.Data[0].Name.ShouldBe("Unit Test Album");
            albums.Paging.Next.ShouldBeNull();
            albums.Paging.Previous.ShouldBeNull();
            albums.Paging.First.ShouldBe("/me/albums?page=1");
            albums.Paging.Last.ShouldBe("/me/albums?page=1");
        }

        [Fact]
        public async Task GetAlbumsShouldCorrectlyWorkForUserId()
        {
            MockHttpRequest("/users/115220313/albums", "GET",string.Empty, GetJson("Album.albums-115220313.json"));
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync(VimeoSettings.PublicUserId);
            albums.Total.ShouldBe(1);
            albums.PerPage.ShouldBe(25);
            albums.Data.Count.ShouldBe(1);
            albums.Paging.Next.ShouldBeNull();
            albums.Paging.Previous.ShouldBeNull();
            albums.Paging.First.ShouldBe($"/users/{VimeoSettings.PublicUserId}/albums?page=1");
            albums.Paging.Last.ShouldBe($"/users/{VimeoSettings.PublicUserId}/albums?page=1");
            var album = albums.Data[0];
            album.Name.ShouldBe("UnitTestAlbum");
            album.Description.ShouldBe("Simple album for testing purpose");
        }

        [Fact]
        public async Task AlbumManagementShouldWorkCorrectlyForMe()
        {
            // create a new album...
            const string originalName = "Unit Test Album";
            const string originalDesc =
                "This album was created via an automated test, and should be deleted momentarily...";
            const string password = "test";
            MockHttpRequest("/me/albums", "POST",
                "privacy=password" +
                "&sort=newest" +
                $"&name={originalName.Replace(" ", "+")}" +
                $"&description={originalDesc.Replace(" ", "+").Replace(",", "%2C")}" +
                $"&password={password}", GetJson("Album.create-album.json"));
            var client = CreateAuthenticatedClient();

            var newAlbum = await client.CreateAlbumAsync(UserId.Me, new EditAlbumParameters
            {
                Name = originalName,
                Description = originalDesc,
                Sort = EditAlbumSortOption.Newest,
                Privacy = EditAlbumPrivacyOption.Password,
                Password = password
            });

            newAlbum.ShouldNotBeNull();
            newAlbum.Name.ShouldBe(originalName);

            newAlbum.Description.ShouldBe(originalDesc);

            MockHttpRequest("/me/albums", "GET", string.Empty, GetJson("Album.albums.json"));

            // retrieve albums for the current user...there should be at least one now...
            var albums = await client.GetAlbumsAsync(UserId.Me);

            albums.Total.ShouldBeGreaterThan(0);

            // update the album...
            const string updatedName = "Unit Test Album (Updated)";
            MockHttpRequest("/me/albums/10303859", "PATCH", 
                "privacy=anybody&name=Unit+Test+Album+%28Updated%29",
                GetJson("Album.patched-album.json"));
            var albumId = newAlbum.GetAlbumId();
            albumId.ShouldNotBeNull();
            var updatedAlbum = await client.UpdateAlbumAsync(UserId.Me, albumId.Value, new EditAlbumParameters
            {
                Name = updatedName,
                Privacy = EditAlbumPrivacyOption.Anybody
            });

            updatedAlbum.Name.ShouldBe(updatedName);

            // delete the album...
            MockHttpRequest("/me/albums/10303859", "DELETE", string.Empty, string.Empty);
            albumId = updatedAlbum.GetAlbumId();
            albumId.ShouldNotBeNull();
            var isDeleted = await client.DeleteAlbumAsync(UserId.Me, albumId.Value);

            isDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task AlbumManagementShouldWorkCorrectlyForUserId()
        {
            // create a new album...
            const string originalName = "Unit Test Album";
            const string originalDesc =
                "This album was created via an automated test, and should be deleted momentarily...";
            const string password = "test";
            MockHttpRequest("/users/2433258/albums", "POST",
                "privacy=password" +
                "&sort=newest" +
                $"&name={originalName.Replace(" ", "+")}" +
                $"&description={originalDesc.Replace(" ", "+").Replace(",", "%2C")}" +
                $"&password={password}", GetJson("Album.create-album.json"));

            var newAlbum = await AuthenticatedClient.CreateAlbumAsync(VimeoSettings.UserId, new EditAlbumParameters
            {
                Name = originalName,
                Description = originalDesc,
                Sort = EditAlbumSortOption.Newest,
                Privacy = EditAlbumPrivacyOption.Password,
                Password = "test"
            });

            newAlbum.ShouldNotBeNull();
            newAlbum.Name.ShouldBe(originalName);

            newAlbum.Description.ShouldBe(originalDesc);

            // retrieve albums for the user...there should be at least one now...
            MockHttpRequest("/users/2433258/albums", "GET", string.Empty, GetJson("Album.albums.json"));
            var albums = await AuthenticatedClient.GetAlbumsAsync(VimeoSettings.UserId);

            albums.Total.ShouldBeGreaterThan(0);

            // update the album...
            const string updatedName = "Unit Test Album (Updated)";
            MockHttpRequest("/users/2433258/albums/10303859", "PATCH", 
                "privacy=anybody&name=Unit+Test+Album+%28Updated%29",
                GetJson("Album.patched-album.json"));
            var albumId = newAlbum.GetAlbumId();
            albumId.ShouldNotBeNull();
            var updatedAlbum = await AuthenticatedClient.UpdateAlbumAsync(VimeoSettings.UserId, albumId.Value,
                new EditAlbumParameters
                {
                    Name = updatedName,
                    Privacy = EditAlbumPrivacyOption.Anybody
                });

            updatedAlbum.Name.ShouldBe(updatedName);

            // delete the album...
            MockHttpRequest("/users/2433258/albums/10303859", "DELETE", string.Empty, string.Empty);
            albumId = updatedAlbum.GetAlbumId();
            albumId.ShouldNotBeNull();
            var isDeleted = await AuthenticatedClient.DeleteAlbumAsync(VimeoSettings.UserId, albumId.Value);

            isDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task GetAlbumsShouldCorrectlyWorkWithParameters()
        {
            MockHttpRequest("/me/albums?per_page=50", "GET", string.Empty, GetJson("Album.album-with-params.json"));
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync(UserId.Me, new GetAlbumsParameters {PerPage = 50});
            albums.ShouldNotBeNull();
            albums.PerPage.ShouldBe(50);
        }
    }
}