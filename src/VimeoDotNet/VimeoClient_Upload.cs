using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        /// <summary>
        /// Complete upload file asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns></returns>
        public async Task CompleteFileUploadAsync(IUploadRequest uploadRequest)
        {
            ThrowIfUnauthorized();

            try
            {
                IApiRequest request = GenerateCompleteUploadRequest(uploadRequest.Ticket);
                IRestResponse response = await request.ExecuteRequestAsync();
                CheckStatusCodeError(uploadRequest, response, "Error marking file upload as complete.");

                Parameter locationHeader =
                    response.Headers.FirstOrDefault(h => string.Compare(h.Name, "Location", true) == 0);
                if (locationHeader != null && locationHeader.Value != null)
                {
                    uploadRequest.ClipUri = locationHeader.Value as string;
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

        /// <summary>
        /// Continue upload file asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns>Verification upload response</returns>
        public async Task<VerifyUploadResponse> ContinueUploadFileAsync(IUploadRequest uploadRequest)
        {
            if (uploadRequest.AllBytesWritten)
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
                IApiRequest request = await GenerateFileStreamRequest(uploadRequest.File, uploadRequest.Ticket,
                    chunkSize: uploadRequest.ChunkSize, written: uploadRequest.BytesWritten);
                IRestResponse response = await request.ExecuteRequestAsync();
                CheckStatusCodeError(uploadRequest, response, "Error uploading file chunk.", HttpStatusCode.BadRequest);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // something went wrong, figure out where we need to start over
                    return await VerifyUploadFileAsync(uploadRequest);
                }

                // Success, update total written
                uploadRequest.BytesWritten += uploadRequest.ChunkSize;
                uploadRequest.BytesWritten = Math.Min(uploadRequest.BytesWritten, uploadRequest.FileLength);
                return new VerifyUploadResponse
                {
                    Status = UploadStatusEnum.InProgress,
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
        /// Create new upload ticket asynchronously
        /// </summary>
        /// <returns>Upload ticket</returns>
        public async Task<UploadTicket> GetUploadTicketAsync()
        {
            try
            {
                IApiRequest request = GenerateUploadTicketRequest();
                IRestResponse<UploadTicket> response = await request.ExecuteRequestAsync<UploadTicket>();
                UpdateRateLimit(response);
                CheckStatusCodeError(null, response, "Error generating upload ticket.");

                return response.Data;
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
        /// Create new upload ticket for replace video asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <returns>Upload ticket</returns>
        public async Task<UploadTicket> GetReplaceVideoUploadTicketAsync(long videoId)
        {
            try
            {
                IApiRequest request = GenerateReplaceVideoUploadTicketRequest(videoId);
                IRestResponse<UploadTicket> response = await request.ExecuteRequestAsync<UploadTicket>();
                UpdateRateLimit(response);
                CheckStatusCodeError(null, response, "Error generating upload ticket to replace video.");

                return response.Data;
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

        /// <summary>
        /// Start upload file asynchronously
        /// </summary>
        /// <param name="fileContent">FileContent</param>
        /// <param name="chunkSize">ChunkSize</param>
        /// <param name="replaceVideoId">ReplaceVideoId</param>
        /// <returns></returns>
        public async Task<IUploadRequest> StartUploadFileAsync(IBinaryContent fileContent,
            int chunkSize = DEFAULT_UPLOAD_CHUNK_SIZE,
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

            UploadTicket ticket = replaceVideoId.HasValue 
                ? await GetReplaceVideoUploadTicketAsync(replaceVideoId.Value)
                : await GetUploadTicketAsync();

            var uploadRequest = new UploadRequest
            {
                Ticket = ticket,
                File = fileContent,
                ChunkSize = chunkSize
            };

            VerifyUploadResponse uploadStatus = await ContinueUploadFileAsync(uploadRequest);
            
            return uploadRequest;
        }

        /// <summary>
        /// Upload file part asynchronously
        /// </summary>
        /// <param name="fileContent">FileContent</param>
        /// <param name="chunkSize">ChunkSize</param>
        /// <param name="replaceVideoId">ReplaceVideoId</param>
        /// <returns>Upload request</returns>
        public async Task<IUploadRequest> UploadEntireFileAsync(IBinaryContent fileContent,
            int chunkSize = DEFAULT_UPLOAD_CHUNK_SIZE,
            long? replaceVideoId = null)
        {
            IUploadRequest uploadRequest = await StartUploadFileAsync(fileContent, chunkSize, replaceVideoId);

            VerifyUploadResponse uploadStatus = null;
            while (!uploadRequest.IsVerifiedComplete)
            {
                uploadStatus = await ContinueUploadFileAsync(uploadRequest);                

                if (uploadRequest.AllBytesWritten)
                {
                    // We presumably wrote all the bytes in the file, so verify with Vimeo that it
                    // is completed
                    uploadStatus = await VerifyUploadFileAsync(uploadRequest);
                    if (uploadStatus.Status == UploadStatusEnum.Completed)
                    {
                        // If completed, mark file as complete
                        await CompleteFileUploadAsync(uploadRequest);
                        uploadRequest.IsVerifiedComplete = true;
                    }
                    else if (uploadStatus.BytesWritten == uploadRequest.FileLength)
                    {
                        // Supposedly all bytes are written, but Vimeo doesn't think so, so just
                        // bail out
                        throw new VimeoUploadException(
                            string.Format(
                                "Vimeo failed to mark file as completed, Bytes Written: {0:N0}, Expected: {1:N0}.",
                                uploadStatus.BytesWritten),
                            uploadRequest);
                    }
                }
            }

            return uploadRequest;
        }

        /// <summary>
        /// Create new upload ticket asynchronously
        /// </summary>
        /// <returns>Upload ticket</returns>
        public async Task<Video> UploadPullLinkAsync(string link)
        {
            try
            {
                IApiRequest request = GenerateUploadTicketRequest("pull");
                request.Query.Add("link", link);
                IRestResponse<Video> response = await request.ExecuteRequestAsync<Video>();
                UpdateRateLimit(response);
                CheckStatusCodeError(null, response, "Error generating upload ticket.");

                return response.Data;
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
        /// Verify upload file part asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns>Verification reponse</returns>
        public async Task<VerifyUploadResponse> VerifyUploadFileAsync(IUploadRequest uploadRequest)
        {
            try
            {
                IApiRequest request =
                    await GenerateFileStreamRequest(uploadRequest.File, uploadRequest.Ticket, verifyOnly: true);
                IRestResponse response = await request.ExecuteRequestAsync();
                var verify = new VerifyUploadResponse();
                CheckStatusCodeError(uploadRequest, response, "Error verifying file upload.", (HttpStatusCode)308);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    verify.BytesWritten = uploadRequest.FileLength;
                    verify.Status = UploadStatusEnum.Completed;
                }
                else if (response.StatusCode == (HttpStatusCode)308)
                {
                    verify.Status = UploadStatusEnum.InProgress;
                    int startIndex = 0;
                    int endIndex = 0;
                    Parameter rangeHeader =
                        response.Headers.FirstOrDefault(h => string.Compare(h.Name, "Range", true) == 0);
                    if (rangeHeader != null && rangeHeader.Value != null)
                    {
                        Match match = RangeRegex.Match(rangeHeader.Value as string);
                        if (match.Success
                            && int.TryParse(match.Groups["start"].Value, out startIndex)
                            && int.TryParse(match.Groups["end"].Value, out endIndex))
                        {
                            verify.BytesWritten = endIndex - startIndex;
                            if (verify.BytesWritten == uploadRequest.FileLength)
                            {
                                verify.Status = UploadStatusEnum.Completed;
                            }
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

        private IApiRequest GenerateCompleteUploadRequest(UploadTicket ticket)
        {
            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.DELETE;
            request.Path = ticket.complete_uri;
            return request;
        }

        private async Task<IApiRequest> GenerateFileStreamRequest(IBinaryContent fileContent, UploadTicket ticket,
            long written = 0, int? chunkSize = null, bool verifyOnly = false)
        {
            if (ticket == null || string.IsNullOrWhiteSpace(ticket.ticket_id))
            {
                throw new ArgumentException("Invalid upload ticket.");
            }
            if (fileContent.Data.Length > ticket.user.upload_quota.space.free)
            {
                throw new InvalidOperationException(
                    "User does not have enough free space to upload this video. Remaining space: " +
                    ticket.quota.free_space + ".");
            }

            IApiRequest request = ApiRequestFactory.GetApiRequest();
            request.Method = Method.PUT;
            request.ExcludeAuthorizationHeader = true;
            request.Path = ticket.upload_link_secure;
            request.Headers.Add(Request.HeaderContentType, fileContent.ContentType);
            if (verifyOnly)
            {
                request.Headers.Add(Request.HeaderContentLength, "0");
                request.Headers.Add(Request.HeaderContentRange, "bytes */*");
            }
            else
            {
                if (chunkSize.HasValue)
                {
                    long startIndex = fileContent.Data.CanSeek ? fileContent.Data.Position : written;
                    long endIndex = Math.Min(startIndex + chunkSize.Value, fileContent.Data.Length);
                    request.Headers.Add(Request.HeaderContentLength, (endIndex - startIndex).ToString());
                    request.Headers.Add(Request.HeaderContentRange,
                        string.Format("bytes {0}-{1}/{2}", startIndex, endIndex, fileContent.Data.Length));
                    request.BinaryContent = await fileContent.ReadAsync(startIndex, endIndex);
                }
                else
                {
                    request.Headers.Add(Request.HeaderContentLength, fileContent.Data.Length.ToString());
                    request.BinaryContent = await fileContent.ReadAllAsync();
                }
            }

            return request;
        }

        private IApiRequest GenerateUploadTicketRequest(string type = "streaming")
        {
            ThrowIfUnauthorized();

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.POST;
            request.Path = Endpoints.UploadTicket;
            request.Query.Add("type", type);
            return request;
        }

        private IApiRequest GenerateReplaceVideoUploadTicketRequest(long clipId)
        {
            ThrowIfUnauthorized();

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.PUT;
            request.Path = Endpoints.VideoReplaceFile;
            request.UrlSegments.Add("clipId", clipId.ToString());
            request.Query.Add("type", "streaming");
            return request;
        }
    }
}