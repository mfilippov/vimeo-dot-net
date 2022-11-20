using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Enums;
using VimeoDotNet.Models;
using VimeoDotNet.Parameters;
using Xunit;

namespace VimeoDotNet.Tests
{

    public class FolderTest : BaseTest
    {
        private const int TotalFoldersCount = 7;
        private const int RootFoldersCount = 2;

        [Fact]
        public async Task ShouldCorrectlyGetFolderList()
        {
            var client = CreateAuthenticatedClient();
            Paginated<Folder> folders = await client.GetUserFolders(null);
            folders.Total.ShouldBeGreaterThan(1);
            folders.Total.ShouldBe(TotalFoldersCount);
            folders.PerPage.ShouldBe(25);
            folders.Data.Count.ShouldBeGreaterThan(1);
            folders.Data.Count.ShouldBeLessThanOrEqualTo(TotalFoldersCount);
            AssertPagingNext(folders);
            folders.Paging.Previous.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetRootItemsList()
        {
            var client = CreateAuthenticatedClient();
            Paginated<Item> items = await client.GetUserRootItems(VimeoSettings.UserId);
            
            items.Total.ShouldBeGreaterThan(1);
            items.Total.ShouldBeLessThan(7);
            items.PerPage.ShouldBe(25);
            items.Data.Where(x=>x.IsVideo).Count().ShouldBe(1);
            items.Data.Where(x => x.IsFolder).Count().ShouldBe(1);
            items.Data.Count.ShouldBe(RootFoldersCount);
            AssertPagingNext<Item>(items);
            items.Paging.Previous.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetFolderItemsList()
        {
            var client = CreateAuthenticatedClient();
            Paginated<Item> rootItems = await client.GetUserRootItems(VimeoSettings.UserId);
            var folder = rootItems.Data.First(x => x.IsFolder);
            Paginated<Item> folderItems = await client.GetFolderItems(VimeoSettings.UserId, folder.Folder.Id.Value);
            folderItems.Total.ShouldBe(7);
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