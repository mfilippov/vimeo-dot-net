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
            textTracks.Data.Count.ShouldBe(1);
            var textTrack = textTracks.Data[0];
            textTrack.ShouldNotBeNull();
            textTrack.Active.ShouldBeTrue();
            textTrack.Name.ShouldBe("test.vtt");
            textTrack.Type.ShouldBe(TextTrackType.SubTitles);
            textTrack.Language.ShouldBe("en");
            textTrack.Uri.ShouldBe("/videos/236281380/texttracks/5303777");
            textTrack.Link.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ShouldCorrectlyGetTextTrack()
        {
            var client = CreateAuthenticatedClient();
            var textTrack = await client.GetTextTrackAsync(VimeoSettings.VideoId, VimeoSettings.TextTrackId);
            textTrack.ShouldNotBeNull();
            textTrack.Active.ShouldBeTrue();
            textTrack.Name.ShouldBe("test.vtt");
            textTrack.Type.ShouldBe(TextTrackType.SubTitles);
            textTrack.Language.ShouldBe("en");
            textTrack.Uri.ShouldBe("/videos/236281380/texttracks/5303777");
            textTrack.Link.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ShouldCorrectlyUploadAndDeleteTextTrackFile()
        {
            var client = CreateAuthenticatedClient();
            TextTrack textTrack;
            const string textTrackName = "UploadtTest.vtt";
            const string textTrackLanguage = "en";
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestTextTrackFilePath),
                "application/octet-stream"))
            {
                textTrack = await client.UploadTextTrackFileAsync(
                    file,
                    VimeoSettings.VideoId,
                    new TextTrack
                    {
                        Active = false,
                        Name = textTrackName,
                        Language = textTrackLanguage,
                        Type = TextTrackType.Captions
                    });
            }

            textTrack.ShouldNotBeNull();
            textTrack.Name.ShouldBe(textTrackName);
            textTrack.Active.ShouldBeFalse();
            textTrack.Language.ShouldBe(textTrackLanguage);
            textTrack.Uri.ShouldNotBeEmpty();
            textTrack.Link.ShouldNotBeEmpty();

            var uri = textTrack.Uri;
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
            const TextTrackType testType = TextTrackType.Metadata;
            const string testLanguage = "fr";
            const bool testActive = false;

            var updated = await client.UpdateTextTrackAsync(
                VimeoSettings.VideoId,
                VimeoSettings.TextTrackId,
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

            // restore the original values...
            var final = await client.UpdateTextTrackAsync(
                VimeoSettings.VideoId,
                VimeoSettings.TextTrackId,
                new TextTrack
                {
                    Name = original.Name,
                    Type = original.Type,
                    Language = original.Language,
                    Active = original.Active
                });

            // inspect the result and ensure the values match our originals...
            final.Name.ShouldBe(original.Name);
            final.Type.ShouldBe(original.Type);
            final.Language.ShouldBe(original.Language);
            final.Active.ShouldBe(original.Active);
        }
    }
}