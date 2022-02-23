using System;
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
            const string description = "This channel created from Vimeo client tests.";
            const string name = "test-channel";
            var link = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("=", "")
                .Replace("+", "")
                .Replace("/", "");

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

            var channel = await client.GetChannelAsync(channelId);

            channel.GetChannelId().ShouldBe(channelId);
            channel.Name.ShouldBe(name);
            channel.Link.ShouldBe($"https://vimeo.com/channels/{link.ToLowerInvariant()}");
            channel.Description.ShouldBe(description);
            channel.Privacy.View.ShouldBe(ChannelPrivacyOption.Moderators.GetParameterValue());

            await client.DeleteChannelAsync(channelId);
        }

        [Fact]
        public async Task ShouldCorrectlyGetChannelList()
        {
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
            var client = CreateAuthenticatedClient();
            var channelParameters = new EditChannelParameters()
            {
                Name = "test-user-channel",
                Privacy = ChannelPrivacyOption.Moderators
            };
            var channel = await client.CreateChannelAsync(channelParameters);
            channel.GetChannelId().ShouldNotBeNull();
            var userChannels = await client.GetUserChannelsAsync();
            var userChannel = userChannels.Data.Where(x => x.Name == channelParameters.Name).FirstOrDefault(); // test only with 1 channel
            userChannel.ShouldNotBeNull();
            await client.DeleteChannelAsync(channel.GetChannelId().Value);
        }
    }
}