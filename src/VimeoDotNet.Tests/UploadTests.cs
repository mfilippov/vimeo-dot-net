using System;
using System.Collections.Generic;
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
            const int userId = 2433258;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/videos",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = """{"upload": { "approach": "tus", "size": "1000"}}""",
                StatusCode = 201,
                ResponseJsonFile = "Upload.new-upload-ticket.json",
                RequestHeaders = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json; charset=utf-8" },
                    { "Accept", "application/vnd.vimeo.*+json; version=3.4, application/json" }
                }
            });
            var ticket = await AuthenticatedClient.GetUploadTicketAsync(1000);
            ticket.ShouldNotBeNull();
            ticket.Upload.Status.ShouldBe("in_progress");
            ticket.Upload.Approach.ShouldBe("tus");
            ticket.Upload.Size.ShouldBe(1000);
            ticket.Upload.UploadLink.ShouldBe(
                "https://asia-files.tus.vimeo.com/files/vimeo-prod-src-tus-asia/61ee816a73b61ae72d6b4806c374d020");
            ticket.User.Id.ShouldBe(userId);
            ticket.User.UploadQuota.Space.Free.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyGenerateReplaceUploadTicket()
        {
            const string fileName = "test.mp4";
            const long size = 1000;
            const int clipId = 530969457;
            const int userId = 2433258;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/versions",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody =
                    $$$"""{ "file_name": "{{{fileName}}}", "upload": { "status": "in_progress", "size": "{{{size}}}", "approach": "tus"}}""",
                StatusCode = 201,
                ResponseJsonFile = "Upload.new-upload-ticket.json",
                RequestHeaders = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json; charset=utf-8" },
                    { "Accept", "application/vnd.vimeo.*+json; version=3.4, application/json" }
                }
            });
            var ticket = await AuthenticatedClient.GetReplaceVideoUploadTicketAsync(clipId, fileName, size);
            ticket.ShouldNotBeNull();
            ticket.Upload.Status.ShouldBe("in_progress");
            ticket.Upload.Approach.ShouldBe("tus");
            ticket.Upload.Size.ShouldBe(1000);
            ticket.Upload.UploadLink.ShouldBe(
                "https://asia-files.tus.vimeo.com/files/vimeo-prod-src-tus-asia/61ee816a73b61ae72d6b4806c374d020");
            ticket.User.Id.ShouldBe(userId);
            ticket.User.UploadQuota.Space.Free.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCorrectlyUploadFileByPath()
        {
            long length;
            IUploadRequest completedRequest;
            const long chunkSize = 1024000;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/videos",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = """{"upload": { "approach": "tus", "size": "5510872"}}""",
                StatusCode = 201,
                ResponseJsonFile = "Upload.new-upload-ticket.json",
                RequestHeaders = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json; charset=utf-8" },
                    { "Accept", "application/vnd.vimeo.*+json; version=3.4, application/json" }
                }
            });

            var videoFs = GetFileFromEmbeddedResources(TestVideoFilePath);
            var chunkCount = (int)(videoFs.Length / chunkSize + 1);
            for (var i = 0; i < chunkCount; i++)
            {
                var chunk = new byte[Math.Min(videoFs.Length - chunkSize * i, chunkSize)];
                var read = await videoFs.ReadAsync(chunk, 0, chunk.Length);
                read.ShouldBe(chunk.Length);
                MockHttpRequest(new RequestSettings
                {
                    AuthBypass = true,
                    UrlSuffix = "/files/vimeo-prod-src-tus-asia/61ee816a73b61ae72d6b4806c374d020",
                    Method = RequestSettings.HttpMethod.Patch,
                    RequestBinaryBody = chunk,
                    StatusCode = 204,
                    RequestHeaders = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/offset+octet-stream" },
                        { "Content-Length", $"{chunk.Length}" },
                        { "Tus-Resumable", "1.0.0" },
                        { "Upload-Offset", $"{chunkSize * i}" },
                        { "Accept", "application/vnd.vimeo.*+json; version=3.4, application/json" }
                    },
                    CustomResponseHeaders = new Dictionary<string, string>
                    {
                        { "Tus-Resumable", "1.0.0" },
                        { "Upload-Offset", $"{chunkSize * (i + 1)}" }
                    }
                });
            }

            MockHttpRequest(new RequestSettings
            {
                AuthBypass = true,
                UrlSuffix = "/files/vimeo-prod-src-tus-asia/61ee816a73b61ae72d6b4806c374d020",
                Method = RequestSettings.HttpMethod.Head,
                CustomResponseHeaders = new Dictionary<string, string>
                {
                    { "Tus-Resumable", "1.0.0" },
                    { "Upload-Length", "5510872" },
                    { "Upload-Offset", "5510872" },
                    {
                        "Upload-Metadata", "app_id ZmJiMjhlNTEtNGZlMS00ZjQ1LWEzYTktODYwZWQwYjNhOWY3,filename VW50aXR" +
                                           "sZWQ=,notify aHR0cHM6Ly92aW1lby5jb20vdXBsb2FkL190dXM=,signature UWdLZlBk" +
                                           "R25iUVliQi9uOHpFUnZGNlYwczgxUXhTZktIcGlmQXVKV3FWVT0=,upload_attempt_id N" +
                                           "zg4NDg3MTYy,user MjQzMzI1OA==,user_region Z3MtYXNpYQ==,vimeo_app_id NTUyNjQ="
                    }
                }
            });

            var tempFilePath = Path.GetTempFileName() + ".mp4";
            // ReSharper disable once UseAwaitUsing
            using (var fs = new FileStream(tempFilePath, FileMode.CreateNew))
            {
                await GetFileFromEmbeddedResources(TestVideoFilePath).CopyToAsync(fs);
            }

            using (var file = new BinaryContent(tempFilePath))
            {
                file.ContentType.ShouldBe("video/mp4");
                length = file.Data.Length;
                completedRequest = await AuthenticatedClient.UploadEntireFileAsync(file, chunkSize);
                completedRequest.ClipId.ShouldNotBeNull();
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
            const long chunkSize = 1024000;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/videos",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = """{"upload": { "approach": "tus", "size": "5510872"}}""",
                StatusCode = 201,
                ResponseJsonFile = "Upload.new-upload-ticket.json",
                RequestHeaders = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json; charset=utf-8" },
                    { "Accept", "application/vnd.vimeo.*+json; version=3.4, application/json" }
                }
            });

            var videoFs = GetFileFromEmbeddedResources(TestVideoFilePath);
            var chunkCount = (int)(videoFs.Length / chunkSize + 1);
            for (var i = 0; i < chunkCount; i++)
            {
                var chunk = new byte[Math.Min(videoFs.Length - chunkSize * i, chunkSize)];
                var read = await videoFs.ReadAsync(chunk, 0, chunk.Length);
                read.ShouldBe(chunk.Length);
                MockHttpRequest(new RequestSettings
                {
                    AuthBypass = true,
                    UrlSuffix = "/files/vimeo-prod-src-tus-asia/61ee816a73b61ae72d6b4806c374d020",
                    Method = RequestSettings.HttpMethod.Patch,
                    RequestBinaryBody = chunk,
                    StatusCode = 204,
                    RequestHeaders = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/offset+octet-stream" },
                        { "Content-Length", $"{chunk.Length}" },
                        { "Tus-Resumable", "1.0.0" },
                        { "Upload-Offset", $"{chunkSize * i}" },
                        { "Accept", "application/vnd.vimeo.*+json; version=3.4, application/json" }
                    },
                    CustomResponseHeaders = new Dictionary<string, string>
                    {
                        { "Tus-Resumable", "1.0.0" },
                        { "Upload-Offset", $"{chunkSize * (i + 1)}" }
                    }
                });
            }

            MockHttpRequest(new RequestSettings
            {
                AuthBypass = true,
                UrlSuffix = "/files/vimeo-prod-src-tus-asia/61ee816a73b61ae72d6b4806c374d020",
                Method = RequestSettings.HttpMethod.Head,
                CustomResponseHeaders = new Dictionary<string, string>
                {
                    { "Tus-Resumable", "1.0.0" },
                    { "Upload-Length", "5510872" },
                    { "Upload-Offset", "5510872" },
                    {
                        "Upload-Metadata", "app_id ZmJiMjhlNTEtNGZlMS00ZjQ1LWEzYTktODYwZWQwYjNhOWY3,filename VW50aXR" +
                                           "sZWQ=,notify aHR0cHM6Ly92aW1lby5jb20vdXBsb2FkL190dXM=,signature UWdLZlBk" +
                                           "R25iUVliQi9uOHpFUnZGNlYwczgxUXhTZktIcGlmQXVKV3FWVT0=,upload_attempt_id N" +
                                           "zg4NDg3MTYy,user MjQzMzI1OA==,user_region Z3MtYXNpYQ==,vimeo_app_id NTUyNjQ="
                    }
                }
            });

            using (var file = new BinaryContent(GetFileFromEmbeddedResources(TestVideoFilePath), "video/mp4"))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file, chunkSize);
                completedRequest.ClipId.ShouldNotBeNull();
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
            const long chunkSize = 1024000;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/videos",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = """{"upload": { "approach": "tus", "size": "5510872"}}""",
                StatusCode = 201,
                ResponseJsonFile = "Upload.new-upload-ticket.json",
                RequestHeaders = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json; charset=utf-8" },
                    { "Accept", "application/vnd.vimeo.*+json; version=3.4, application/json" }
                }
            });

            var videoFs = GetFileFromEmbeddedResources(TestVideoFilePath);
            var chunkCount = (int)(videoFs.Length / chunkSize + 1);
            for (var i = 0; i < chunkCount; i++)
            {
                var chunk = new byte[Math.Min(videoFs.Length - chunkSize * i, chunkSize)];
                var read = await videoFs.ReadAsync(chunk, 0, chunk.Length);
                read.ShouldBe(chunk.Length);
                MockHttpRequest(new RequestSettings
                {
                    AuthBypass = true,
                    UrlSuffix = "/files/vimeo-prod-src-tus-asia/61ee816a73b61ae72d6b4806c374d020",
                    Method = RequestSettings.HttpMethod.Patch,
                    RequestBinaryBody = chunk,
                    StatusCode = 204,
                    RequestHeaders = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/offset+octet-stream" },
                        { "Content-Length", $"{chunk.Length}" },
                        { "Tus-Resumable", "1.0.0" },
                        { "Upload-Offset", $"{chunkSize * i}" },
                        { "Accept", "application/vnd.vimeo.*+json; version=3.4, application/json" }
                    },
                    CustomResponseHeaders = new Dictionary<string, string>
                    {
                        { "Tus-Resumable", "1.0.0" },
                        { "Upload-Offset", $"{chunkSize * (i + 1)}" }
                    }
                });
            }

            MockHttpRequest(new RequestSettings
            {
                AuthBypass = true,
                UrlSuffix = "/files/vimeo-prod-src-tus-asia/61ee816a73b61ae72d6b4806c374d020",
                Method = RequestSettings.HttpMethod.Head,
                CustomResponseHeaders = new Dictionary<string, string>
                {
                    { "Tus-Resumable", "1.0.0" },
                    { "Upload-Length", "5510872" },
                    { "Upload-Offset", "5510872" },
                    {
                        "Upload-Metadata", "app_id ZmJiMjhlNTEtNGZlMS00ZjQ1LWEzYTktODYwZWQwYjNhOWY3,filename VW50aXR" +
                                           "sZWQ=,notify aHR0cHM6Ly92aW1lby5jb20vdXBsb2FkL190dXM=,signature UWdLZlBk" +
                                           "R25iUVliQi9uOHpFUnZGNlYwczgxUXhTZktIcGlmQXVKV3FWVT0=,upload_attempt_id N" +
                                           "zg4NDg3MTYy,user MjQzMzI1OA==,user_region Z3MtYXNpYQ==,vimeo_app_id NTUyNjQ="
                    }
                }
            });
            var stream = GetFileFromEmbeddedResources(TestVideoFilePath);
            var buffer = new byte[stream.Length];
            (await stream.ReadAsync(buffer, 0, (int)stream.Length)).ShouldBe((int)stream.Length);
            using (var file = new BinaryContent(buffer, "video/mp4"))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();
                completedRequest = await client.UploadEntireFileAsync(file, chunkSize);
                completedRequest.ClipId.ShouldNotBeNull();
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
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/me/videos",
                Method = RequestSettings.HttpMethod.Post,
                RequestTextBody = "upload.approach=pull&upload.link=https%3A%2F%2Fclips.vorwaerts-gmbh." +
                                  "de%2Fbig_buck_bunny.mp4",
                ResponseJsonFile = "Upload.pull-upload.json",
            });
            var client = CreateAuthenticatedClient();
            var video = await client.UploadPullLinkAsync("https://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4");
            video.ShouldNotBeNull();
            video.Id.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCorrectlyUploadThumbnail()
        {
            const int clipId = 530969457;
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}",
                Method = RequestSettings.HttpMethod.Get,
                ResponseJsonFile = "Video.video-530969457.json"
            });
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = $"/videos/{clipId}/pictures",
                Method = RequestSettings.HttpMethod.Post,
                ResponseJsonFile = "Upload.post-thumbnail.json"
            });
            MockHttpRequest(new RequestSettings
                {
                    AuthBypass = true,
                    UrlSuffix = "/video/1651994954?expires=1681054562&sig=af2947525ad44d22f2a3cc9e0b9309a7194ccd82",
                    Method = RequestSettings.HttpMethod.Put,
                    RequestBinaryFile = TextThumbnailFilePath,
                    ResponseJsonFile = "Upload.put-thumbnail.json"
                }
            );
            MockHttpRequest(new RequestSettings
            {
                UrlSuffix = "/videos/530969457/pictures/1651994954",
                Method = RequestSettings.HttpMethod.Patch,
                RequestTextBody = "active=true"
            });
            using var file = new BinaryContent(GetFileFromEmbeddedResources(TextThumbnailFilePath), "image/png");
            var picture = await AuthenticatedClient.UploadThumbnailAsync(clipId, file);
            picture.ShouldNotBeNull();
        }
    }
}