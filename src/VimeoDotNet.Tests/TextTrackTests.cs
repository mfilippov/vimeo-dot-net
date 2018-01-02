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
        public async Task ShouldCorrectlyGetTextTracks()
        {
            var client = CreateAuthenticatedClient();
            var textTracks = await client.GetTextTracksAsync(VimeoSettings.VideoId);
            textTracks.ShouldNotBeNull();
            textTracks.data.Count.ShouldBe(1);
            var textTrack = textTracks.data[0];
            textTrack.ShouldNotBeNull();
            textTrack.active.ShouldBeTrue();
            textTrack.name.ShouldBe("test.vtt");
            textTrack.type.ShouldBe(TextTrackType.subtitles);
            textTrack.language.ShouldBe("en");
            textTrack.uri.ShouldBe("/videos/236281380/texttracks/5303777");
            textTrack.link.ShouldNotBeEmpty();
        }
       
        [Fact]
        public async Task ShouldCorrectlyGetTextTrack()
        {
            var client = CreateAuthenticatedClient();
            var textTrack = await client.GetTextTrackAsync(VimeoSettings.VideoId, VimeoSettings.TextTrackId);
            textTrack.ShouldNotBeNull();
            textTrack.active.ShouldBeTrue();
            textTrack.name.ShouldBe("test.vtt");
            textTrack.type.ShouldBe(TextTrackType.subtitles);
            textTrack.language.ShouldBe("en");
            textTrack.uri.ShouldBe("/videos/236281380/texttracks/5303777");
            textTrack.link.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ShouldCorrectlyUploadAndDeleteTextTrackFile()
        {
            var client = CreateAuthenticatedClient();
            TextTrack textTrack;
            const string textTrackName = "UploadtTest.vtt";
            const string textTrackLanguage = "en";
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestTextTrackFilePath), "application/octet-stream"))
            {
                textTrack = await client.UploadTextTrackFileAsync(
                    file,
                    VimeoSettings.VideoId,
                    new TextTrack
                    {
                        active = false,
                        name = textTrackName,
                        language = textTrackLanguage,
                        type = TextTrackType.captions
                    });
            }
            textTrack.ShouldNotBeNull();
            textTrack.name.ShouldBe(textTrackName);
            textTrack.active.ShouldBeFalse();
            textTrack.language.ShouldBe(textTrackLanguage);
            textTrack.uri.ShouldNotBeEmpty();
            textTrack.link.ShouldNotBeEmpty();
            
            var uri = textTrack.uri;
            var trackId = Convert.ToInt64(uri.Substring(uri.LastIndexOf('/') + 1));
            await client.DeleteTextTrackAsync(VimeoSettings.VideoId, trackId);
            
            var texttrack = await client.GetTextTrackAsync(VimeoSettings.VideoId, trackId);
            texttrack.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyUpdateTextTrackAsync()
        {
            var client = CreateAuthenticatedClient();
            var original = await client.GetTextTrackAsync(VimeoSettings.VideoId, VimeoSettings.TextTrackId);

            original.ShouldNotBeNull();

            // update the text track record with some new values...
            const string testName = "NewTrackName";
            const TextTrackType testType = TextTrackType.metadata;
            const string testLanguage = "fr";
            const bool testActive = false;

            var updated = await client.UpdateTextTrackAsync(
                                    VimeoSettings.VideoId,
                                    VimeoSettings.TextTrackId,
                                    new TextTrack
                                    {
                                        name = testName,
                                        type = testType,
                                        language = testLanguage,
                                        active = testActive
                                    });

            // inspect the result and ensure the values match what we expect...
            updated.name.ShouldBe(testName);
            updated.type.ShouldNotBeNull();
            updated.type.ShouldBe(testType);
            updated.language.ShouldBe(testLanguage);
            updated.active.ShouldBeFalse();

            // restore the original values...
            var final = await client.UpdateTextTrackAsync(
                                    VimeoSettings.VideoId,
                                    VimeoSettings.TextTrackId,
                                    new TextTrack
                                    {
                                        name = original.name,
                                        type = original.type,
                                        language = original.language,
                                        active = original.active
                                    });

            // inspect the result and ensure the values match our originals...
            final.name.ShouldBe(original.name);
            final.type.ShouldBe(original.type);
            final.language.ShouldBe(original.language);
            final.active.ShouldBe(original.active);
        }
    }
}