using System.Diagnostics;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;
using Xunit;


namespace VimeoDotNet.Tests
{
    public class VimeoClientAsyncTests : BaseTest
    {
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
            var user = await client.GetUserInformationAsync(VimeoSettings.UserId);
            user.ShouldNotBeNull();
            user.id.ShouldNotBeNull();
            Debug.Assert(user.id != null, "user.id != null");
            user.id.Value.ShouldBe(VimeoSettings.UserId);
        }
    }
}