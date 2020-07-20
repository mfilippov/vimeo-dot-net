using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        /// <inheritdoc />
        public async Task<UploadTicket> GetUploadTicketAsync()
        {
            try
            {
                var request = GenerateUploadTicketRequest();
                var response = await request.ExecuteRequestAsync<UploadTicket>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(null, response, "Error generating upload ticket.");

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error generating upload ticket.", null, ex);
            }
        }

        public async Task<TusResumableUploadTicket> GetTusResumableUploadTicketAsync(long size, string name = null)
        {
            try
            {
                var request = GenerateTusResumableUploadTicketRequest(size, name);
                var response = await request.ExecuteRequestAsync<TusResumableUploadTicket>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(null, response, "Error generating upload ticket.");

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error generating upload ticket.", null, ex);
            }
        }

        /// <inheritdoc />
        public async Task<UploadTicket> GetReplaceVideoUploadTicketAsync(long videoId)
        {
            try
            {
                var request = GenerateReplaceVideoUploadTicketRequest(videoId);
                var response = await request.ExecuteRequestAsync<UploadTicket>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(null, response, "Error generating upload ticket to replace video.");

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error generating upload ticket to replace video.", null, ex);
            }
        }

        /// <inheritdoc />
        public async Task<IUploadRequest> UploadEntireFileAsync(IBinaryContent fileContent,
            int chunkSize = DefaultUploadChunkSize,
            long? replaceVideoId = null,
            Action<double> statusCallback = null)
        {
            var uploadRequest = await StartUploadFileAsync(fileContent, chunkSize, replaceVideoId).ConfigureAwait(false);

            while (!uploadRequest.IsVerifiedComplete)
            {
                var uploadStatus = await ContinueUploadFileAsync(uploadRequest).ConfigureAwait(false);

                statusCallback?.Invoke(Math.Round(((double)uploadStatus.BytesWritten / uploadRequest.FileLength) * 100));

                if (uploadStatus.Status == UploadStatusEnum.InProgress)
                    continue;
                // We presumably wrote all the bytes in the file, so verify with Vimeo that it
                // is completed
                uploadStatus = await VerifyUploadFileAsync(uploadRequest).ConfigureAwait(false);
                if (uploadStatus.Status == UploadStatusEnum.Completed)
                {
                    // If completed, mark file as complete
                    await CompleteFileUploadAsync(uploadRequest).ConfigureAwait(false);
                    uploadRequest.IsVerifiedComplete = true;
                }
                else if (uploadStatus.BytesWritten == uploadRequest.FileLength)
                {
                    // Supposedly all bytes are written, but Vimeo doesn't think so, so just
                    // bail out
                    throw new VimeoUploadException(
                        $"Vimeo failed to mark file as completed, Bytes Written: {uploadStatus.BytesWritten:N0}, Expected: {uploadRequest.FileLength:N0}.",
                        uploadRequest);
                }
            }

            return uploadRequest;
        }

        /// <inheritdoc />
        public async Task<Video> UploadPullLinkAsync(string link)
        {
            try
            {
                var param = new ParameterDictionary {{"type", "pull"}, {"link", link}};

                var request = _apiRequestFactory.AuthorizedRequest(
                    AccessToken,
                    HttpMethod.Post,
                    Endpoints.UploadTicket,
                    null,
                    param
                );

                return await ExecuteApiRequest<Video>(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error generating upload ticket.", null, ex);
            }
        }

        /// <inheritdoc />
        public async Task<Picture> UploadThumbnailAsync(long clipId, IBinaryContent fileContent)
        {
            try
            {
                var picUri = await UploadPictureAsync(fileContent, clipId).ConfigureAwait(false);
                await SetThumbnailActiveAsync(picUri).ConfigureAwait(false);
                return new Picture { Uri = picUri};
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error generating upload ticket.", null, ex);
            }
        }

        /// <summary>
        /// Start upload file asynchronously
        /// </summary>
        /// <param name="fileContent">FileContent</param>
        /// <param name="chunkSize">ChunkSize</param>
        /// <param name="replaceVideoId">ReplaceVideoId</param>
        /// <returns></returns>
        private async Task<IUploadRequest> StartUploadFileAsync(IBinaryContent fileContent,
            int chunkSize = DefaultUploadChunkSize,
            long? replaceVideoId = null)
        {
            if (!fileContent.Data.CanRead)
            {
                throw new ArgumentException("fileContent should be readable");
            }

            if (fileContent.Data.CanSeek && fileContent.Data.Position > 0)
            {
                fileContent.Data.Position = 0;
            }

            var ticket = replaceVideoId.HasValue
                ? await GetReplaceVideoUploadTicketAsync(replaceVideoId.Value).ConfigureAwait(false)
                : await GetUploadTicketAsync().ConfigureAwait(false);

            var uploadRequest = new UploadRequest
            {
                Ticket = ticket,
                File = fileContent,
                ChunkSize = chunkSize
            };

            return uploadRequest;
        }

        /// <summary>
        /// Continue upload file asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns>Verification upload response</returns>
        private async Task<VerifyUploadResponse> ContinueUploadFileAsync(IUploadRequest uploadRequest)
        {
            if (uploadRequest.FileLength == uploadRequest.BytesWritten)
            {
                // Already done, there's nothing to do.
                return new VerifyUploadResponse
                {
                    Status = UploadStatusEnum.InProgress,
                    BytesWritten = uploadRequest.BytesWritten
                };
            }

            try
            {
                var request = await GenerateFileStreamRequest(uploadRequest.File, uploadRequest.Ticket,
                    chunkSize: uploadRequest.ChunkSize, written: uploadRequest.BytesWritten).ConfigureAwait(false);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                CheckStatusCodeError(uploadRequest, response, "Error uploading file chunk.", HttpStatusCode.BadRequest);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // something went wrong, figure out where we need to start over
                    return await VerifyUploadFileAsync(uploadRequest).ConfigureAwait(false);
                }

                // Success, update total written
                uploadRequest.BytesWritten += uploadRequest.ChunkSize;
                uploadRequest.BytesWritten = Math.Min(uploadRequest.BytesWritten, uploadRequest.FileLength);

                return new VerifyUploadResponse
                {
                    Status = uploadRequest.FileLength == uploadRequest.BytesWritten
                        ? UploadStatusEnum.Completed
                        : UploadStatusEnum.InProgress,
                    BytesWritten = uploadRequest.BytesWritten
                };
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error uploading file chunk", uploadRequest, ex);
            }
        }

        /// <summary>
        /// Verify upload file part asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns>Verification reponse</returns>
        private async Task<VerifyUploadResponse> VerifyUploadFileAsync(IUploadRequest uploadRequest)
        {
            try
            {
                var request =
                    await GenerateFileStreamRequest(uploadRequest.File, uploadRequest.Ticket, verifyOnly: true)
                        .ConfigureAwait(false);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                var verify = new VerifyUploadResponse();
                CheckStatusCodeError(uploadRequest, response, "Error verifying file upload.", (HttpStatusCode) 308);

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    verify.BytesWritten = uploadRequest.FileLength;
                    verify.Status = UploadStatusEnum.Completed;
                }
                else if (response.StatusCode == (HttpStatusCode) 308)
                {
                    verify.Status = UploadStatusEnum.InProgress;
                    if (!response.Headers.Contains("Range"))
                        return verify;
                    var match = RangeRegex.Match(response.Headers.GetValues("Range").First());
                    // ReSharper disable once InvertIf
                    if (match.Success
                        && long.TryParse(match.Groups["start"].Value, out var startIndex)
                        && long.TryParse(match.Groups["end"].Value, out var endIndex))
                    {
                        verify.BytesWritten = endIndex - startIndex;
                        uploadRequest.BytesWritten = verify.BytesWritten;
                        if (verify.BytesWritten == uploadRequest.FileLength)
                        {
                            verify.Status = UploadStatusEnum.Completed;
                        }
                    }
                }
                else
                {
                    verify.Status = UploadStatusEnum.NotFound;
                }

                return verify;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error verifying file upload.", uploadRequest, ex);
            }
        }

        /// <summary>
        /// Complete upload file asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns></returns>
        private async Task CompleteFileUploadAsync(IUploadRequest uploadRequest)
        {
            ThrowIfUnauthorized();

            try
            {
                var request = GenerateCompleteUploadRequest(uploadRequest.Ticket);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                CheckStatusCodeError(uploadRequest, response, "Error marking file upload as complete.");

                if (response.Headers.Location != null)
                {
                    uploadRequest.ClipUri = response.Headers.Location.ToString();
                }
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error marking file upload as complete.", uploadRequest, ex);
            }
        }

        private IApiRequest GenerateCompleteUploadRequest(UploadTicket ticket)
        {
            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Delete;
            request.Path = ticket.CompleteUri;
            return request;
        }

        private async Task<IApiRequest> GenerateFileStreamRequest(IBinaryContent fileContent, UploadTicket ticket,
            long written = 0, int? chunkSize = null, bool verifyOnly = false)
        {
            if (string.IsNullOrWhiteSpace(ticket?.TicketId))
            {
                throw new ArgumentException("Invalid upload ticket.");
            }

            if (fileContent.Data.Length > ticket.User.UploadQuota.Space.Free)
            {
                throw new InvalidOperationException(
                    "User does not have enough free space to upload this video. Remaining space: " +
                    ticket.Quota.FreeSpace + ".");
            }

            var request = _apiRequestFactory.GetApiRequest();
            request.Method = HttpMethod.Put;
            request.ExcludeAuthorizationHeader = true;
            request.Path = ticket.UploadLinkSecure;

            if (verifyOnly)
            {
                var body = new ByteArrayContent(new byte[0]);
                body.Headers.Add("Content-Range", "bytes */*");
                body.Headers.ContentLength = 0;
                request.Body = body;
            }
            else
            {
                if (chunkSize.HasValue)
                {
                    var startIndex = fileContent.Data.CanSeek ? fileContent.Data.Position : written;
                    var endIndex = Math.Min(startIndex + chunkSize.Value, fileContent.Data.Length);
                    var byteArray = await fileContent.ReadAsync(startIndex, endIndex).ConfigureAwait(false);
                    var body = new ByteArrayContent(byteArray, 0, byteArray.Length);
                    body.Headers.Add("Content-Range", $"bytes {startIndex}-{endIndex}/*");
                    body.Headers.ContentLength = endIndex - startIndex;
                    request.Body = body;
                }
                else
                {
                    request.Body = new ByteArrayContent(await fileContent.ReadAllAsync().ConfigureAwait(false));
                }
            }

            return request;
        }

        private IApiRequest GenerateTusResumableUploadTicketRequest(long size, string name = null)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.ApiVersion = ApiVersions.v3_4;
            request.Method = HttpMethod.Post;
            request.Path = Endpoints.UploadTicket;


            var parameters = new Dictionary<string, string>
            {
                ["upload.approach"] = "tus",
                ["upload.size"] = size.ToString()
            };

            if(name != null)
            {
                parameters["name"] = name;
            }

            request.Body = new FormUrlEncodedContent(parameters);
            return request;
        }

        private IApiRequest GenerateUploadTicketRequest(string type = "streaming")
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Post;
            request.Path = Endpoints.UploadTicket;
            request.Query.Add("type", type);
            return request;
        }

        private IApiRequest GenerateReplaceVideoUploadTicketRequest(long clipId)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Put;
            request.Path = Endpoints.VideoReplaceFile;
            request.UrlSegments.Add("clipId", clipId.ToString());
            request.Query.Add("type", "streaming");
            return request;
        }
    }
}
