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
            var folders = await client.GetUserFolders(null);
            folders.Total.ShouldBeGreaterThan(1);
            folders.Total.ShouldBe(TotalFoldersCount);
            folders.PerPage.ShouldBe(25);
            folders.Data.Count.ShouldBeGreaterThan(1);
            folders.Data.Count.ShouldBeLessThanOrEqualTo(TotalFoldersCount);
            AssertPagingNext(folders);
            folders.Paging.Previous.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetRootFolderList()
        {
            var client = CreateAuthenticatedClient();
            var folders = await client.GetUserRootFolders(VimeoSettings.UserId);
            folders.Total.ShouldBeGreaterThan(1);
            folders.Total.ShouldBeLessThan(7);
            folders.PerPage.ShouldBe(25);
            folders.Data.Count.ShouldBe(RootFoldersCount);
            AssertPagingNext(folders);
            folders.Paging.Previous.ShouldBeNull();
        }

        private static void AssertPagingNext(Paginated<Folder> folders)
        {
            if (folders.Total > folders.PerPage)
                folders.Paging.Next.ShouldNotBeNull();
            else
            {
                folders.Paging.Next.ShouldBeNull();
            }
        }
    }
}