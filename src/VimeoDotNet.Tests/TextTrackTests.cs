using System;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class TextTrackTests : BaseTest
    {
        [Fact]
        public async Task TestTrackInteractionTest()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var client = CreateAuthenticatedClient();
                TextTrack newTextTrack;
                const string textTrackName = "UploadtTest.vtt";
                const string textTrackLanguage = "en";
                using (var file = new BinaryContent(TestHelper.GetFileFromEmbeddedResources(TestHelper.TestFilePath),
                    "application/octet-stream"))
                {
                    newTextTrack = await client.UploadTextTrackFileAsync(
                        file,
                        clipId,
                        new TextTrack
                        {
                            Active = false,
                            Name = textTrackName,
                            Language = textTrackLanguage,
                            Type = TextTrackType.Captions
                        });
                }

                newTextTrack.ShouldNotBeNull();
                newTextTrack.Name.ShouldBe(textTrackName);
                newTextTrack.Active.ShouldBeFalse();
                newTextTrack.Language.ShouldBe(textTrackLanguage);
                newTextTrack.Uri.ShouldNotBeEmpty();
                newTextTrack.Link.ShouldNotBeEmpty();
                
                var uri = newTextTrack.Uri;
                var trackId = Convert.ToInt64(uri.Substring(uri.LastIndexOf('/') + 1));
                
                var textTrack = await client.GetTextTrackAsync(clipId, trackId);
                textTrack.ShouldNotBeNull();
                textTrack.Active.ShouldBeFalse();
                textTrack.Name.ShouldBe("UploadtTest.vtt");
                textTrack.Type.ShouldBe(TextTrackType.Captions);
                textTrack.Language.ShouldBe("en");
                textTrack.Uri.ShouldNotBeEmpty();
                textTrack.Link.ShouldNotBeEmpty();
                
                const string testName = "NewTrackName";
                const TextTrackType testType = TextTrackType.Metadata;
                const string testLanguage = "fr";
                const bool testActive = false;

                var updated = await client.UpdateTextTrackAsync(
                    clipId,
                    trackId,
                    new TextTrack
                    {
                        Name = testName,
                        Type = testType,
                        Language = testLanguage,
                        Active = testActive
                    });

                // inspect the result and ensure the values match what we expect...
                updated.Name.ShouldBe(testName);
                updated.Type.ShouldNotBeNull();
                updated.Type.ShouldBe(testType);
                updated.Language.ShouldBe(testLanguage);
                updated.Active.ShouldBeFalse();

                await client.DeleteTextTrackAsync(clipId, trackId);

                var deletedTrack = await client.GetTextTrackAsync(clipId, trackId);
                deletedTrack.ShouldBeNull();
            });
        }
    }
}