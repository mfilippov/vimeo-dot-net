using System.IO;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Net;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class UploadTests : BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyGenerateNewUploadTicket()
        {
            var client = CreateAuthenticatedClient();
            var ticket = await client.GetUploadTicketAsync();
            ticket.ShouldNotBeNull();
            ticket.CompleteUri.ShouldNotBeEmpty();
            ticket.TicketId.ShouldNotBeEmpty();
            ticket.UploadLink.ShouldNotBeEmpty();
            ticket.UploadLinkSecure.ShouldNotBeEmpty();
            ticket.Uri.ShouldNotBeEmpty();
            ticket.User.Id.ShouldBe(VimeoSettings.UserId);
        }

        [Fact]
        public async Task ShouldCorrectlyGenerateReplaceUploadTicket()
        {
            var client = CreateAuthenticatedClient();
            var ticket = await client.GetReplaceVideoUploadTicketAsync(VimeoSettings.VideoId);
            ticket.ShouldNotBeNull();
            ticket.CompleteUri.ShouldNotBeEmpty();
            ticket.TicketId.ShouldNotBeEmpty();
            ticket.UploadLink.ShouldNotBeEmpty();
            ticket.UploadLinkSecure.ShouldNotBeEmpty();
            ticket.Uri.ShouldNotBeEmpty();
            ticket.User.Id.ShouldBe(VimeoSettings.UserId);
        }

        [Fact]
        public async Task ShouldCorretlyUploadFileByPath()
        {
            long length;
            IUploadRequest completedRequest;
            var tempFilePath = Path.GetTempFileName() + ".mp4";
            using (var fs = new FileStream(tempFilePath, FileMode.CreateNew))
            {
                GetFileFromEmbeddedResources(TestFilePath).CopyTo(fs);
            }

            using (var file = new BinaryContent(tempFilePath))
            {
                file.ContentType.ShouldBe("video/mp4");
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file);
                completedRequest.ClipId.ShouldNotBeNull();
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }

            completedRequest.ShouldNotBeNull();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        [Fact]
        public async Task ShouldCorretlyUploadFileByStream()
        {
            long length;
            IUploadRequest completedRequest;

            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestFilePath), "video/mp4"))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file);
                completedRequest.ClipId.ShouldNotBeNull();
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }

            completedRequest.ShouldNotBeNull();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorretlyUploadFileByByteArray()
        {
            long length;
            IUploadRequest completedRequest;
            var stream = GetFileFromEmbeddedResources(TestFilePath);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int) stream.Length);
            using (var file = new BinaryContent(buffer, "video/mp4"))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file);
                completedRequest.ClipId.ShouldNotBeNull();
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }

            completedRequest.ShouldNotBeNull();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorretlyUploadFileByPullLink()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.UploadPullLinkAsync("http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4");
            video.ShouldNotBeNull();
            video.Id.ShouldNotBeNull();
            await client.DeleteVideoAsync(video.Id.Value);
            video = await client.GetVideoAsync(video.Id.Value);
            video.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldCorretlyUploadThumbnail()
        {
            var client = CreateAuthenticatedClient();
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TextThumbnailFilePath), "image/png"))
            {
                var picture = await client.UploadThumbnailAsync(VimeoSettings.VideoId, file);
                picture.ShouldNotBeNull();
            }
        }
    }
}