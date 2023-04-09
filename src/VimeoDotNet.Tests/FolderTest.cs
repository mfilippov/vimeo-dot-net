using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Models;
using Xunit;

namespace VimeoDotNet.Tests
{

    public class FolderTest : BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyGetFolderListForMe()
        {
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/folders",
                ResponseJsonFile = "Folder.user-folder-list.json"
            });
            var client = CreateAuthenticatedClient();
            var folders = await client.GetUserFolders(UserId.Me);
            folders.Total.ShouldBeGreaterThan(1);
            folders.Total.ShouldBe(12);
            folders.PerPage.ShouldBe(25);
            folders.Data.Count.ShouldBeGreaterThan(1);
            folders.Data.Count.ShouldBeLessThanOrEqualTo(12);
            AssertPagingNext(folders);
            folders.Paging.Previous.ShouldBeNull();
        }
        
        [Fact]
        public async Task ShouldCorrectlyGetFolderListForUserId()
        {
            const long userId = 2433258;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/users/{userId}/folders",
                ResponseJsonFile = "Folder.user-folder-list.json"
            });
            var client = CreateAuthenticatedClient();
            var folders = await client.GetUserFolders(userId);
            folders.Total.ShouldBeGreaterThan(1);
            folders.Total.ShouldBe(12);
            folders.PerPage.ShouldBe(25);
            folders.Data.Count.ShouldBeGreaterThan(1);
            folders.Data.Count.ShouldBeLessThanOrEqualTo(12);
            AssertPagingNext(folders);
            folders.Paging.Previous.ShouldBeNull();
        }


        [Fact]
        public async Task ShouldCorrectlyGetFolderItemsListForMe()
        {
            const long folderId = 15721523;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/projects/15721523/items",
                ResponseJsonFile = "Folder.folder-15721523-items.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/projects/15721574/items",
                ResponseJsonFile = "Folder.folder-15721574-items.json"
            });
            var client = CreateAuthenticatedClient();
            var rootItems = await client.GetFolderItems(UserId.Me, folderId);
            var folder = rootItems.Data.OrderBy(x => x.IsFolder ? x.Folder.Name : x.Video.Name).First(x => x.IsFolder);
            folder.Folder.Id.ShouldNotBeNull();
            var folderItems = await client.GetFolderItems(UserId.Me, folder.Folder.Id.Value);
            folderItems.Total.ShouldBe(1);
        }
        
        [Fact]
        public async Task ShouldCorrectlyGetFolderItemsListForUserId()
        {
            const long userId = 2433258;
            const long folderId = 15721523;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/users/{userId}/projects/15721523/items",
                ResponseJsonFile = "Folder.folder-15721523-items.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/users/{userId}/projects/15721574/items",
                ResponseJsonFile = "Folder.folder-15721574-items.json"
            });
            var client = CreateAuthenticatedClient();
            var rootItems = await client.GetFolderItems(userId, folderId);
            var folder = rootItems.Data.OrderBy(x => x.IsFolder ? x.Folder.Name : x.Video.Name).First(x => x.IsFolder);
            folder.Folder.Id.ShouldNotBeNull();
            var folderItems = await client.GetFolderItems(userId, folder.Folder.Id.Value);
            folderItems.Total.ShouldBe(1);
        }

        [Fact]
        public async Task ShouldCorrectlyCreateFolder()
        {
            var client = CreateAuthenticatedClient();
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/folders",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = "name=Test+folder",
                ResponseJsonFile = "Folder.post-folder.json"
            });
            await client.CreateFolder(UserId.Me, "Test folder");
        }

        [Fact]
        public async Task ShouldCorrectlyDeleteFolder()
        {
            const long folderId = 15721704;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/me/projects/{folderId}",
                Method = RequestSettings.HttpMethod.Delete,
                StatusCode = 204
            });
            var client = CreateAuthenticatedClient();
            await client.DeleteFolder(UserId.Me, folderId);
        }

        private static void AssertPagingNext<T>(Paginated<T> paginated) where T:class
        {
            if (paginated.Total > paginated.PerPage)
                paginated.Paging.Next.ShouldNotBeNull();
            else
            {
                paginated.Paging.Next.ShouldBeNull();
            }
        }
    }
}