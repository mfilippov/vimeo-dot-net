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
            const int clipId = 530969457;
            var client = CreateAuthenticatedClient();
            TextTrack newTextTrack;
            const string textTrackName = "UploadTest.vtt";
            const string textTrackLanguage = "en";

            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/videos/530969457/texttracks/",
                RequestTextBody = "active=false&name=UploadTest.vtt&language=en&type=captions",
                Method = RequestSettings.HttpMethod.Post,
                ResponseJsonFile = "TextTrack.new-text-track.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/captions/84654997?expires=1680975493&sig=8e259a1376e90c4112beb66e7f28b37aa8491f7c",
                Method = RequestSettings.HttpMethod.Put,
                RequestBinaryFile = TestTextTrackFilePath,
                AuthBypass = true,
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/videos/530969457/texttracks/84654997",
                ResponseJsonFile = "TextTrack.get-text-track-84654997.json"
            });
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestTextTrackFilePath),
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
            textTrack.Name.ShouldBe("UploadTest.vtt");
            textTrack.Type.ShouldBe(TextTrackType.Captions);
            textTrack.Language.ShouldBe("en");
            textTrack.Uri.ShouldNotBeEmpty();
            textTrack.Link.ShouldNotBeEmpty();

            const string testName = "NewTrackName";
            const TextTrackType testType = TextTrackType.Metadata;
            const string testLanguage = "fr";
            const bool testActive = false;
            
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/videos/530969457/texttracks/84654997",
                Method = RequestSettings.HttpMethod.Patch,
                RequestTextBody = "active=false&name=NewTrackName&language=fr&type=metadata",
                ResponseJsonFile = "TextTrack.patch-text-track-84654997.json"
            });

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

            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/videos/530969457/texttracks/84654997",
                Method = RequestSettings.HttpMethod.Delete
            });
            await client.DeleteTextTrackAsync(clipId, trackId);
        }
    }
}