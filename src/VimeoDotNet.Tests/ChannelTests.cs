using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Enums;
using VimeoDotNet.Parameters;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class ChannelTests : BaseTest
    {
        [Fact]
        public async Task ChannelInteractionTest()
        {
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/channels",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = "privacy=moderators&name=test-channel&description=This+channel+" +
                                  "created+from+Vimeo+client+tests.&link=1sMDqioosEKP4P95k36dAQ",
                ResponseJsonFile = "Channel.create-channel.json"
            });
            const string description = "This channel created from Vimeo client tests.";
            const string name = "test-channel";
            const string link = "1sMDqioosEKP4P95k36dAQ";

            var client = CreateAuthenticatedClient();
            var result = await client.CreateChannelAsync(new EditChannelParameters
            {
                Name = name,
                Link = link,
                Description = description,
                Privacy = ChannelPrivacyOption.Moderators
            });

            result.GetChannelId().ShouldNotBeNull();
            // ReSharper disable once PossibleInvalidOperationException
            var channelId = result.GetChannelId().Value;

            result.Name.ShouldBe(name);
            result.Link.ShouldBe($"https://vimeo.com/channels/{link.ToLowerInvariant()}");
            result.Description.ShouldBe(description);
            result.Privacy.View.ShouldBe(ChannelPrivacyOption.Moderators.GetParameterValue());

            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/channels/1837771",
                ResponseJsonFile = "Channel.channel-1837771.json"
            });
            var channel = await client.GetChannelAsync(channelId);

            channel.GetChannelId().ShouldBe(channelId);
            channel.Name.ShouldBe(name);
            channel.Link.ShouldBe($"https://vimeo.com/channels/{link.ToLowerInvariant()}");
            channel.Description.ShouldBe(description);
            channel.Privacy.View.ShouldBe(ChannelPrivacyOption.Moderators.GetParameterValue());

            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/channels/1837771",
                Method = RequestSettings.HttpMethod.Delete
            });
            await client.DeleteChannelAsync(channelId);
        }

        [Fact]
        public async Task ShouldCorrectlyGetChannelList()
        {
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/channels",
                ResponseJsonFile = "Channel.channels.json"
            });
            var client = CreateAuthenticatedClient();
            var channels = await client.GetChannelsAsync();
            channels.Total.ShouldBeGreaterThan(1);
            channels.PerPage.ShouldBe(25);
            channels.Data.Count.ShouldBe(25);
            channels.Paging.Next.ShouldNotBeNull();
            channels.Paging.Previous.ShouldBeNull();
        }

        [Fact]
        public async Task TestUserIdChannelsList()
        {
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/channels",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = "privacy=moderators&name=test-user-channel",
                ResponseJsonFile = "Channel.channel-1837772.json"
            });
            var client = CreateAuthenticatedClient();
            var channelParameters = new EditChannelParameters()
            {
                Name = "test-user-channel",
                Privacy = ChannelPrivacyOption.Moderators
            };
            var channel = await client.CreateChannelAsync(channelParameters);
            var channelId = channel.GetChannelId();
            channelId.ShouldNotBeNull();
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/channels",
                ResponseJsonFile = "Channel.user-channels.json"
            });
            var userChannels = await client.GetUserChannelsAsync();
            var userChannel =
                userChannels.Data.FirstOrDefault(x => x.Name == channelParameters.Name); // test only with 1 channel
            userChannel.ShouldNotBeNull();
        }
    }
}