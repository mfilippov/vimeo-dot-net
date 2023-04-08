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
            var ticket = await AuthenticatedClient.GetUploadTicketAsync();
            ticket.ShouldNotBeNull();
            ticket.CompleteUri.ShouldNotBeEmpty();
            ticket.TicketId.ShouldNotBeEmpty();
            ticket.UploadLink.ShouldNotBeEmpty();
            ticket.UploadLinkSecure.ShouldNotBeEmpty();
            ticket.Uri.ShouldNotBeEmpty();
            ticket.User.Id.ShouldBe(VimeoSettings.UserId);
        }

        [Fact]
        public async Task ShouldCorrectlyGenerateNewTusResumableUploadTicket()
        {
            var ticket = await AuthenticatedClient.GetTusResumableUploadTicketAsync(1000);
            ticket.ShouldNotBeNull();
            ticket.Upload.UploadLink.ShouldNotBeEmpty();
            ticket.Id.ShouldNotBeNull();
            ticket.User.Id.ShouldBe(VimeoSettings.UserId);
            await AuthenticatedClient.DeleteVideoAsync(ticket.Id.Value);
        }

        [Fact]
        public async Task ShouldCorrectlyGenerateReplaceUploadTicket()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                var ticket = await AuthenticatedClient.GetReplaceVideoUploadTicketAsync(clipId);
                ticket.ShouldNotBeNull();
                ticket.CompleteUri.ShouldNotBeEmpty();
                ticket.TicketId.ShouldNotBeEmpty();
                ticket.UploadLink.ShouldNotBeEmpty();
                ticket.UploadLinkSecure.ShouldNotBeEmpty();
                ticket.Uri.ShouldNotBeEmpty();
                ticket.User.Id.ShouldBe(VimeoSettings.UserId);
            });
        }

        [Fact]
        public async Task ShouldCorrectlyUploadFileByPath()
        {
            long length;
            IUploadRequest completedRequest;
            var tempFilePath = Path.GetTempFileName() + ".mp4";
            using (var fs = new FileStream(tempFilePath, FileMode.CreateNew))
            {
                await GetFileFromEmbeddedResources(TestVideoFilePath).CopyToAsync(fs);
            }

            using (var file = new BinaryContent(tempFilePath))
            {
                file.ContentType.ShouldBe("video/mp4");
                length = file.Data.Length;
                completedRequest = await AuthenticatedClient.UploadEntireFileAsync(file);
                completedRequest.ClipId.ShouldNotBeNull();
                await AuthenticatedClient.DeleteVideoAsync(completedRequest.ClipId.Value);
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
        public async Task ShouldCorrectlyUploadFileByStream()
        {
            long length;
            IUploadRequest completedRequest;

            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestVideoFilePath), "video/mp4"))
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
        public async Task ShouldCorrectlyUploadFileByByteArray()
        {
            long length;
            IUploadRequest completedRequest;
            var stream = GetFileFromEmbeddedResources(TestVideoFilePath);
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
        public async Task ShouldCorrectlyUploadFileByPullLink()
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
        public async Task ShouldCorrectlyUploadThumbnail()
        {
            await AuthenticatedClient.WithTempVideo(async clipId =>
            {
                using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestVideoFilePath), "image/png"))
                {
                    var picture = await AuthenticatedClient.UploadThumbnailAsync(clipId, file);
                    picture.ShouldNotBeNull();
                }    
            });
            
        }
    }
}
