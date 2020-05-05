using System;
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
/*
            var channels = await client.GetChannelsAsync();
            channels.Data.Count.ShouldBe(1);
            channels.Total.ShouldBe(1);
            channels.PerPage.ShouldBe(25);
            channels.Data.Count.ShouldBe(1);
            channels.Paging.Next.ShouldBeNull();
            channels.Paging.Previous.ShouldBeNull();

            channels.Data[0].GetChannelId().ShouldBe(channelId);
            channels.Data[0].Name.ShouldBe(name);
            channels.Data[0].Link.ShouldBe($"https://vimeo.com/channels/{link.ToLowerInvariant()}");
            channels.Data[0].Description.ShouldBe(description);
            channels.Data[0].Privacy.View.ShouldBe(ChannelPrivacyOption.Moderators.GetParameterValue());
*/
            var channel = await client.GetChannelAsync(channelId);
            
            channel.GetChannelId().ShouldBe(channelId);
            channel.Name.ShouldBe(name);
            channel.Link.ShouldBe($"https://vimeo.com/channels/{link.ToLowerInvariant()}");
            channel.Description.ShouldBe(description);
            channel.Privacy.View.ShouldBe(ChannelPrivacyOption.Moderators.GetParameterValue());

            await client.DeleteChannelAsync(channelId);
        }
    }
}