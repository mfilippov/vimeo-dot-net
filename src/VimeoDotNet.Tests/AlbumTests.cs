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
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync(UserId.Me);
            albums.total.ShouldBe(1);
            albums.per_page.ShouldBe(25);
            albums.data.Count.ShouldBe(1);
            albums.paging.next.ShouldBeNull();
            albums.paging.previous.ShouldBeNull();
            albums.paging.first.ShouldBe("/me/albums?page=1");
            albums.paging.last.ShouldBe("/me/albums?page=1");
        }

        [Fact]
        public async Task GetAlbumsShouldCorrectlyWorkForUserId()
        {
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync(VimeoSettings.UserId);
            albums.total.ShouldBe(1);
            albums.per_page.ShouldBe(25);
            albums.data.Count.ShouldBe(1);
            albums.paging.next.ShouldBeNull();
            albums.paging.previous.ShouldBeNull();
            albums.paging.first.ShouldBe($"/users/{VimeoSettings.UserId}/albums?page=1");
            albums.paging.last.ShouldBe($"/users/{VimeoSettings.UserId}/albums?page=1");
            var album = albums.data[0];
            album.name.ShouldBe("Test album");
            album.description.ShouldBe("Test description");
        }

        [Fact]
        public async Task AlbumManagementShouldWorkCorrectlyForMe()
        {
            var client = CreateAuthenticatedClient();

            // create a new album...
            const string originalName = "Unit Test Album";
            const string originalDesc = "This album was created via an automated test, and should be deleted momentarily...";

            var newAlbum = await client.CreateAlbumAsync(UserId.Me, new EditAlbumParameters
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
            var albums = await client.GetAlbumsAsync(UserId.Me);

            albums.total.ShouldBeGreaterThan(0);

            // update the album...
            const string updatedName = "Unit Test Album (Updated)";
            var albumId = newAlbum.GetAlbumId();
            albumId.ShouldNotBeNull();
            var updatedAlbum = await client.UpdateAlbumAsync(UserId.Me, albumId.Value, new EditAlbumParameters
            {
                Name = updatedName,
                Privacy = EditAlbumPrivacyOption.Anybody
            });

            updatedAlbum.name.ShouldBe(updatedName);

            // delete the album...
            albumId = updatedAlbum.GetAlbumId();
            albumId.ShouldNotBeNull();
            var isDeleted = await client.DeleteAlbumAsync(UserId.Me, albumId.Value);

            isDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task AlbumManagementShouldWorkCorrectlyForUserId()
        {
            var client = CreateAuthenticatedClient();

            // create a new album...
            const string originalName = "Unit Test Album";
            const string originalDesc = "This album was created via an automated test, and should be deleted momentarily...";

            var newAlbum = await client.CreateAlbumAsync(VimeoSettings.PublicUserId, new EditAlbumParameters
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
            var albums = await client.GetAlbumsAsync(VimeoSettings.PublicUserId);

            albums.total.ShouldBeGreaterThan(0);

            // update the album...
            const string updatedName = "Unit Test Album (Updated)";
            var albumId = newAlbum.GetAlbumId();
            albumId.ShouldNotBeNull();
            var updatedAlbum = await client.UpdateAlbumAsync(VimeoSettings.PublicUserId, albumId.Value, new EditAlbumParameters
            {
                Name = updatedName,
                Privacy = EditAlbumPrivacyOption.Anybody
            });

            updatedAlbum.name.ShouldBe(updatedName);

            // delete the album...
            albumId = updatedAlbum.GetAlbumId();
            albumId.ShouldNotBeNull();
            var isDeleted = await client.DeleteAlbumAsync(VimeoSettings.PublicUserId, albumId.Value);

            isDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task GetAlbumsShouldCorrectlyWorkWithParameters()
        {
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync(UserId.Me, new GetAlbumsParameters { PerPage = 50 });
            albums.ShouldNotBeNull();
            albums.per_page.ShouldBe(50);
        }
    }
}