using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;
using Xunit;
using static System.IO.File;

namespace VimeoDotNet.Tests
{
    public class VimeoClientAsyncTests : BaseTest
    {
        [Fact]
        public async Task Integration_VimeoClient_GetReplaceVideoUploadTicket_CanGenerateStreamingTicket()
        {
            var client = CreateAuthenticatedClient();
            var ticket = await client.GetReplaceVideoUploadTicketAsync(VimeoSettings.VideoId);
            ticket.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUploadTicket_CanGenerateStreamingTicket()
        {
            var client = CreateAuthenticatedClient();
            var ticket = await client.GetUploadTicketAsync();
            ticket.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadEntireFile_UploadsFile_ByPath()
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
                Debug.Assert(completedRequest.ClipId != null, "completedRequest.ClipId != null");
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }
            completedRequest.ShouldNotBeNull();
            completedRequest.AllBytesWritten.ShouldBeTrue();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
            if (Exists(tempFilePath))
            {
                Delete(tempFilePath);
            }
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadEntireFile_UploadsFile_ByStream()
        {
            long length;
            IUploadRequest completedRequest;

            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestFilePath), "video/mp4"))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file);
                Debug.Assert(completedRequest.ClipId != null, "completedRequest.ClipId != null");
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }
            completedRequest.ShouldNotBeNull();
            completedRequest.AllBytesWritten.ShouldBeTrue();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadEntireFile_UploadsFile_ByArray()
        {
            long length;
            IUploadRequest completedRequest;
            var stream = GetFileFromEmbeddedResources(TestFilePath);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);
            using (var file = new BinaryContent(buffer, "video/mp4"))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file);
                Debug.Assert(completedRequest.ClipId != null, "completedRequest.ClipId != null");
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
            }
            completedRequest.ShouldNotBeNull();
            completedRequest.AllBytesWritten.ShouldBeTrue();
            completedRequest.IsVerifiedComplete.ShouldBeTrue();
            completedRequest.BytesWritten.ShouldBe(length);
            completedRequest.ClipUri.ShouldNotBeNull();
            completedRequest.ClipId.ShouldNotBeNull();
            completedRequest.ClipId?.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadEntireFile_UploadsFile_ReadPartOfFile()
        {
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestFilePath), "video/mp4"))
            {
                (await file.ReadAsync(17, 20)).Length.ShouldBe(3);
                (await file.ReadAsync(17000, 17020)).Length.ShouldBe(20);
            }
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadEntireFile_UploadsFile_DoubleRead()
        {
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestFilePath), "video/mp4"))
            {
                (await file.ReadAllAsync()).Length.ShouldBe(5510872);
                (await file.ReadAllAsync()).Length.ShouldBe(5510872);
            }
        }

        [Fact]
        public void Integration_VimeoClient_UploadEntireFile_UploadsFile_DisposedStreamAccess()
        {
            var file = new BinaryContent(GetFileFromEmbeddedResources(TestFilePath), "video/mp4");
            file.Dispose();
            Should.Throw<ObjectDisposedException>(() => file.Dispose());
            Should.Throw<ObjectDisposedException>(() => file.Data.Length.ShouldBe(0));
            Should.ThrowAsync<ObjectDisposedException>(async () => await file.ReadAllAsync());
        }

        [Fact]
        public void Integration_VimeoClient_UploadEntireFile_UploadsFile_InvalidStreams()
        {
            var nonReadablefile = new BinaryContent(new NonReadableStream(), "video/mp4");
            var nonSeekablefile = new BinaryContent(new NonSeekableStream(), "video/mp4");
            Should.ThrowAsync<InvalidOperationException>(async () => await nonReadablefile.ReadAllAsync(), "Content should be a readable Stream");
            Should.ThrowAsync<InvalidOperationException>(async () => await nonSeekablefile.ReadAsync(10, 20), "Content cannot be advanced to the specified start index: 10");
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadPullLink()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.UploadPullLinkAsync("http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4");
            video.ShouldNotBeNull();
            video.id.ShouldNotBeNull();
            await client.DeleteVideoAsync(video.id.Value);
            video = await client.GetVideoAsync(video.id.Value);
            video.ShouldBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_UploadThumbnail()
        {
            var client = CreateAuthenticatedClient();
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TextThumbnailFilePath), "image/png"))
            {
                var picture = await client.UploadThumbnailAsync(VimeoSettings.VideoId, file);
                picture.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task Integration_VimeoClient_DeleteVideo_DeletesVideo()
        {
            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestFilePath), "video/mp4"))
            {
                var length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                var completedRequest = await client.UploadEntireFileAsync(file);
                completedRequest.AllBytesWritten.ShouldBeTrue();
                completedRequest.ShouldNotBeNull();
                completedRequest.IsVerifiedComplete.ShouldBeTrue();
                completedRequest.BytesWritten.ShouldBe(length);
                completedRequest.ClipUri.ShouldNotBeNull();
                completedRequest.ClipId.HasValue.ShouldBeTrue();
                Debug.Assert(completedRequest.ClipId != null, "completedRequest.ClipId != null");
                await client.DeleteVideoAsync(completedRequest.ClipId.Value);
                (await client.GetVideoAsync(completedRequest.ClipId.Value)).ShouldBeNull();
            }
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountInformation_RetrievesCurrentAccountInfo()
        {
            var client = CreateAuthenticatedClient();
            var account = await client.GetAccountInformationAsync();
            account.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_UpdateAccountInformation_UpdatesCurrentAccountInfo()
        {
            // first, ensure we can retrieve the current user...
            var client = CreateAuthenticatedClient();
            var original = await client.GetAccountInformationAsync();
            original.ShouldNotBeNull();

            // next, update the user record with some new values...
            var testName = "King Henry VIII";
            var testBio = "";
            var testLocation = "England";

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
                updated.name.ShouldBeNull();
            else
                updated.name.ShouldBe(testName);
            if (string.IsNullOrEmpty(testBio))
                updated.bio.ShouldBeNull();
            else
                updated.bio.ShouldBe(testBio);

            if (string.IsNullOrEmpty(testLocation))
                updated.location.ShouldBeNull();
            else
                updated.location.ShouldBe(testLocation);

            // restore the original values...
            var final = await client.UpdateAccountInformationAsync(new EditUserParameters
            {
                Name = original.name ?? string.Empty,
                Bio = original.bio ?? string.Empty,
                Location = original.location ?? string.Empty
            });

            // inspect the result and ensure the values match our originals...
            if (string.IsNullOrEmpty(original.name))
            {
                final.name.ShouldBeNull();
            }
            else
            {
                final.name.ShouldBe(original.name);
            }

            if (string.IsNullOrEmpty(original.bio))
            {
                final.bio.ShouldBeNull();
            }
            else
            {
                final.bio.ShouldBe(original.bio);
            }

            if (string.IsNullOrEmpty(original.location))
            {
                final.location.ShouldBeNull();
            }
            else
            {
                final.location.ShouldBe(original.location);
            }
        }


        [Fact]
        public async Task Integration_VimeoClient_GetUserInformation_RetrievesUserInfo()
        {
            var client = CreateAuthenticatedClient();
            var user = await client.GetUserInformationAsync(VimeoSettings.UserId);
            user.ShouldNotBeNull();
            user.id.ShouldNotBeNull();
            Debug.Assert(user.id != null, "user.id != null");
            user.id.Value.ShouldBe(VimeoSettings.UserId);
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideos_RetrievesCurrentAccountVideos()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetUserVideosAsync(VimeoSettings.UserId);
            videos.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideos_SecondPage()
        {
            var client = CreateAuthenticatedClient();

            for (var i = 0; i < 5; i++)
            {
                try
                {
                    var videos = await client.GetVideosAsync(page: 2, perPage: 5);
                    videos.ShouldNotBeNull();
                    return;
                }
                catch (VimeoApiException ex)
                {
                    if (ex.Message.Contains("Please try again."))
                    {
                        continue;
                    }
                    throw;
                }
            }

        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideo_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.ShouldNotBeNull();
            video.pictures.uri.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountVideoWithFields_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.VideoId, new []{"uri", "name"});
            video.ShouldNotBeNull();
            video.uri.ShouldNotBeNull();
            video.name.ShouldNotBeNull();
            video.pictures.ShouldBeNull();
        }

        

        [Fact]
        public async Task Integration_VimeoClient_GetAccountAlbumVideosWithFields_RetrievesCurrentAccountAlbumVideos()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetAlbumVideosAsync(VimeoSettings.AlbumId, 1, null, fields: new[] { "uri", "name" });
            videos.ShouldNotBeNull();
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountAlbumVideo_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetAlbumVideoAsync(VimeoSettings.AlbumId, VimeoSettings.VideoId);
            video.ShouldNotBeNull();
        }
        
        [Fact]
        public async Task Integration_VimeoClient_GetAccountAlbumVideo_GetThumbnails()
        {
            var client = CreateAuthenticatedClient();
            var pictures = await client.GetPicturesAsync(VimeoSettings.VideoId);
            pictures.ShouldNotBeNull();
            pictures.data.Count.ShouldBeGreaterThan(0);
            var uriParts = pictures.data[0].uri.Split('/');
            var pictureId = long.Parse(uriParts[uriParts.Length - 1]);
            var picture = await client.GetPictureAsync(VimeoSettings.VideoId, pictureId);
            picture.ShouldNotBeNull();            
        }

        [Fact]
        public async Task Integration_VimeoClient_GetAccountAlbumVideoWithFields_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetAlbumVideoAsync(VimeoSettings.AlbumId, VimeoSettings.VideoId, new[] { "uri", "name" });
            video.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUserAlbumVideos_RetrievesUserAlbumVideos()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetUserAlbumVideosAsync(VimeoSettings.UserId, VimeoSettings.AlbumId);
            videos.ShouldNotBeNull();
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUserAlbumVideosWithFields_RetrievesUserAlbumVideos()
        {
            var client = CreateAuthenticatedClient();
            var videos = await client.GetUserAlbumVideosAsync(VimeoSettings.UserId, VimeoSettings.AlbumId, new []{"name", "link"});
            videos.ShouldNotBeNull();
            videos.data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUserAlbumVideo_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetUserAlbumVideoAsync(VimeoSettings.UserId, VimeoSettings.AlbumId, VimeoSettings.VideoId);
            video.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUserAlbumVideoWithFields_RetrievesVideo()
        {
            var client = CreateAuthenticatedClient();
            var video = await client.GetUserAlbumVideoAsync(VimeoSettings.UserId, VimeoSettings.AlbumId, VimeoSettings.VideoId, new []{"uri", "name"});
            video.ShouldNotBeNull();
        }

        [Fact]
        public async Task Integration_VimeoClient_GetUserAlbums_NotNull()
        {
            var client = CreateAuthenticatedClient();
            var albums = await client.GetAlbumsAsync(VimeoSettings.UserId);
            albums.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAccountVideoWithUnauthenticatedToken()
        {
            var client = await CreateUnauthenticatedClient();
            var video = await client.GetVideoAsync(VimeoSettings.VideoId);
            video.ShouldNotBeNull();
            video.pictures.uri.ShouldNotBeNull();
        }

        [Fact]
        public async Task CheckRateLimits()
        {
            var client = await CreateUnauthenticatedClient();
            await client.GetVideoAsync(VimeoSettings.VideoId);
            client.RateLimit.ShouldBeGreaterThan(0);
            client.RateLimitRemaining.ShouldBeGreaterThan(0);
            client.RateLimitReset.Kind.ShouldBe(DateTimeKind.Utc);
        }

        private class NonReadableStream : Stream
        {
            public NonReadableStream()
            {
                CanSeek = false;
                CanWrite = false;
                Length = 0;
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override bool CanRead => false;
            public override bool CanSeek { get; }
            public override bool CanWrite { get; }
            public override long Length { get; }
            public override long Position { get; set; }
        }

        private class NonSeekableStream : Stream
        {
            public NonSeekableStream()
            {
                CanWrite = false;
                Length = 0;
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite { get; }
            public override long Length { get; }
            public override long Position { get; set; }
        }
    }
}