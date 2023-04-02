using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Parameters;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class AccountTests : BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyGetAccountInformation()
        {
            MockHttpRequest("/me", "GET",string.Empty, GetJson("User.user.json"));
            var client = CreateAuthenticatedClient();
            var account = await client.GetAccountInformationAsync();
            account.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyGetUserInformation()
        {
            MockHttpRequest($"/users/{VimeoSettings.UserId}", "GET", string.Empty, GetJson("User.user.json"));
            var client = CreateAuthenticatedClient();
            var user = await client.GetUserInformationAsync(VimeoSettings.UserId);
            user.ShouldNotBeNull();
            user.Id.ShouldNotBeNull();
            user.Id.ShouldNotBeNull();
            user.Id.Value.ShouldBe(VimeoSettings.UserId);
        }

        [Fact]
        public async Task ShouldCorrectlyUpdateAccountInformation()
        {
            const string testName = "Jonh Wayne";
            const string testBio = "Test bio";
            const string testLocation = "England";
            
            MockHttpRequest("/me", "GET",
                string.Empty, GetJson("User.user.json"));
            MockHttpRequest("/me", "PATCH",
                $"name={testName.Replace(" ", "+")}" +
                $"&location={testLocation}" +
                $"&bio={testBio.Replace(" ", "+")}", GetJson("User.user.json"));
            // first, ensure we can retrieve the current user...
            var client = CreateAuthenticatedClient();
            var original = await client.GetAccountInformationAsync();
            original.ShouldNotBeNull();

            // next, update the user record with some new values...
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
                updated.Name.ShouldBeNull();
            else
                updated.Name.ShouldBe(testName);
            if (string.IsNullOrEmpty(testBio))
                updated.Bio.ShouldBeNull();
            else
                updated.Bio.ShouldBe(testBio);

            if (string.IsNullOrEmpty(testLocation))
                updated.Location.ShouldBeNull();
            else
                updated.Location.ShouldBe(testLocation);

            // restore the original values...
            var final = await client.UpdateAccountInformationAsync(new EditUserParameters
            {
                Name = original.Name ?? string.Empty,
                Bio = original.Bio ?? string.Empty,
                Location = original.Location ?? string.Empty
            });

            // inspect the result and ensure the values match our originals...
            if (string.IsNullOrEmpty(original.Name))
            {
                final.Name.ShouldBeNull();
            }
            else
            {
                final.Name.ShouldBe(original.Name);
            }

            if (string.IsNullOrEmpty(original.Bio))
            {
                final.Bio.ShouldBeNull();
            }
            else
            {
                final.Bio.ShouldBe(original.Bio);
            }

            if (string.IsNullOrEmpty(original.Location))
            {
                final.Location.ShouldBeNull();
            }
            else
            {
                final.Location.ShouldBe(original.Location);
            }
        }
    }
}