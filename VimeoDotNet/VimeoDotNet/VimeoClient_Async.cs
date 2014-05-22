using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;
using VimeoDotNet.Authorization;
using VimeoDotNet.Constants;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient : IVimeoClient
    {
        #region Constants

        internal const int DEFAULT_UPLOAD_CHUNK_SIZE = 1048576; // 1MB

        protected static readonly Regex _rangeRegex = new Regex(@"bytes\s*=\s*(?<start>\d+)-(?<end>\d+)",
            RegexOptions.IgnoreCase);

        #endregion

        #region Fields

        protected IApiRequestFactory _apiRequestFactory;
        protected IAuthorizationClientFactory _authClientFactory;

        #endregion

        #region Properties

        protected string ClientId { get; set; }
        protected string ClientSecret { get; set; }
        protected string AccessToken { get; set; }

        protected IAuthorizationClient OAuth2Client { get; set; }

        #endregion

        #region Constructors

        protected VimeoClient()
        {
            _authClientFactory = new AuthorizationClientFactory();
            _apiRequestFactory = new ApiRequestFactory();
        }

        /// <summary>
        ///     Multi-user application constructor, using user-level OAuth2
        /// </summary>
        /// <param name="accessToken">Your Vimeo API Access Token</param>
        public VimeoClient(string clientId, string clientSecret)
            : this()
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            OAuth2Client = new AuthorizationClient(ClientId, ClientSecret);
        }

        /// <summary>
        ///     Single-user application constructor, using account OAuth2 access token
        /// </summary>
        /// <param name="accessToken">Your Vimeo API Access Token</param>
        public VimeoClient(string accessToken)
            : this()
        {
            AccessToken = accessToken;
        }

        /// <summary>
        ///     Multi-user Constructor for use with IVimeoClientFactory
        /// </summary>
        /// <param name="authClientFactory">The IAuthorizationClientFactory</param>
        /// <param name="apiRequestFactory">The IApiRequestFactory</param>
        internal VimeoClient(IAuthorizationClientFactory authClientFactory, IApiRequestFactory apiRequestFactory,
            string clientId, string clientSecret)
            : this(clientId, clientSecret)
        {
            _authClientFactory = authClientFactory;
            _apiRequestFactory = apiRequestFactory;
        }

        /// <summary>
        ///     Single-user Constructor for use with IVimeoClientFactory
        /// </summary>
        /// <param name="authClientFactory">The IAuthorizationClientFactory</param>
        /// <param name="apiRequestFactory">The IApiRequestFactory</param>
        internal VimeoClient(IAuthorizationClientFactory authClientFactory, IApiRequestFactory apiRequestFactory,
            string accessToken)
            : this(accessToken)
        {
            _authClientFactory = authClientFactory;
            _apiRequestFactory = apiRequestFactory;
        }

        #endregion

        #region Authorization

        public string GetOauthUrl(string redirectUri, IEnumerable<string> scope, string state)
        {
            PrepAuthorizationClient();
            return OAuth2Client.GetAuthorizationEndpoint(redirectUri, scope, state);
        }

        public async Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUrl)
        {
            PrepAuthorizationClient();
            return await OAuth2Client.GetAccessTokenAsync(authorizationCode, redirectUrl);
        }

        private void PrepAuthorizationClient()
        {
            if (OAuth2Client == null)
            {
                OAuth2Client = _authClientFactory.GetAuthorizationClient(ClientId, ClientSecret);
            }
        }

        #endregion

        #region Account

        public async Task<User> GetAccountInformationAsync()
        {
            try
            {
                IApiRequest request = GenerateUserInformationRequest();
                IRestResponse<User> response = await request.ExecuteRequestAsync<User>();
                CheckStatusCodeError(response, "Error retrieving account information.");

                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving account information.", ex);
            }
        }

        public async Task<User> GetUserInformationAsync(long userId)
        {
            try
            {
                IApiRequest request = GenerateUserInformationRequest(userId);
                IRestResponse<User> response = await request.ExecuteRequestAsync<User>();
                CheckStatusCodeError(response, "Error retrieving user information.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving user information.", ex);
            }
        }

        private IApiRequest GenerateUserInformationRequest(long? userId = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.GET;
            request.Path = userId.HasValue ? Endpoints.User : Endpoints.GetCurrentUserEndpoint(Endpoints.User);
            if (userId.HasValue)
            {
                request.UrlSegments.Add("userId", userId.ToString());
            }
            return request;
        }

        #endregion

        #region Videos

        public async Task<Paginated<Video>> GetAccountVideosAsync()
        {
            try
            {
                IApiRequest request = GenerateVideosRequest();
                IRestResponse<Paginated<Video>> response = await request.ExecuteRequestAsync<Paginated<Video>>();
                CheckStatusCodeError(response, "Error retrieving account videos.");

                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving account videos.", ex);
            }
        }

        public async Task<Video> GetAccountVideoAsync(long clipId)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(clipId: clipId);
                IRestResponse<Video> response = await request.ExecuteRequestAsync<Video>();
                CheckStatusCodeError(response, "Error retrieving account video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving account video.", ex);
            }
        }

        public async Task<Paginated<Video>> GetUserVideosAsync(long userId)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(userId);
                IRestResponse<Paginated<Video>> response = await request.ExecuteRequestAsync<Paginated<Video>>();
                CheckStatusCodeError(response, "Error retrieving user videos.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new Paginated<Video>
                    {
                        data = new List<Video>(),
                        page = 0,
                        total = 0
                    };
                }
                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving user videos.", ex);
            }
        }

        public async Task<Video> GetUserVideoAsync(long userId, long clipId)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(userId, clipId);
                IRestResponse<Video> response = await request.ExecuteRequestAsync<Video>();
                CheckStatusCodeError(response, "Error retrieving user video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving user video.", ex);
            }
        }

        public async Task UpdateVideoMetadataAsync(long clipId, VideoUpdateMetadata metaData)
        {
            try
            {
                IApiRequest request = GenerateVideoPatchRequest(clipId, metaData);
                IRestResponse response = await request.ExecuteRequestAsync();
                CheckStatusCodeError(response, "Error updating user video metadata.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error updating user video metadata.", ex);
            }
        }

        private IApiRequest GenerateVideosRequest(long? userId = null, long? clipId = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = clipId.HasValue ? Endpoints.UserVideo : Endpoints.UserVideos;
            request.Method = Method.GET;
            request.Path = userId.HasValue ? endpoint : Endpoints.GetCurrentUserEndpoint(endpoint);

            if (userId.HasValue)
            {
                request.UrlSegments.Add("userId", userId.ToString());
            }
            if (clipId.HasValue)
            {
                request.UrlSegments.Add("clipId", clipId.ToString());
            }

            return request;
        }

        private IApiRequest GenerateVideoPatchRequest(long clipId, VideoUpdateMetadata metaData)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.PATCH;
            request.Path = Endpoints.Video;

            request.UrlSegments.Add("clipId", clipId.ToString());
            if (metaData.Name != null)
            {
                request.Query.Add("name", metaData.Name.Trim());
            }
            if (metaData.Description != null)
            {
                request.Query.Add("description", metaData.Description.Trim());
            }
            if (metaData.Privacy != VideoPrivacyEnum.Unknown)
            {
                request.Query.Add("privacy.view", metaData.Privacy.ToString().ToLower());
            }
            request.Query.Add("review_link", metaData.ReviewLinkEnabled.ToString().ToLower());

            return request;
        }

        #endregion

        #region Upload

        public async Task<IUploadRequest> UploadEntireFileAsync(IBinaryContent fileContent,
            int chunkSize = DEFAULT_UPLOAD_CHUNK_SIZE)
        {
            IUploadRequest uploadRequest = await StartUploadFileAsync(fileContent, chunkSize);

            VerifyUploadResponse uploadStatus = null;
            while (!uploadRequest.IsVerifiedComplete)
            {
                uploadStatus = await ContinueUploadFileAsync(uploadRequest);
                uploadRequest.BytesWritten = uploadStatus.BytesWritten;

                if (uploadRequest.AllBytesWritten)
                {
                    // We presumably wrote all the bytes in the file, so verify with Vimeo that it is completed
                    uploadStatus = await VerifyUploadFileAsync(uploadRequest);
                    if (uploadStatus.Status == UploadStatusEnum.Completed)
                    {
                        // If completed, mark file as complete
                        await CompleteFileUploadAsync(uploadRequest);
                        uploadRequest.IsVerifiedComplete = true;
                    }
                    else if (uploadStatus.BytesWritten == uploadRequest.FileLength)
                    {
                        // Supposedly all bytes are written, but Vimeo doesn't think so, so just bail out
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

        public async Task<IUploadRequest> StartUploadFileAsync(IBinaryContent fileContent,
            int chunkSize = DEFAULT_UPLOAD_CHUNK_SIZE)
        {
            if (!fileContent.Data.CanRead)
            {
                throw new ArgumentException("fileContent should be readable");
            }
            if (fileContent.Data.CanSeek && fileContent.Data.Position > 0)
            {
                fileContent.Data.Position = 0;
            }

            UploadTicket ticket = await GetUploadTicketAsync();

            var uploadRequest = new UploadRequest
            {
                Ticket = ticket,
                File = fileContent,
                ChunkSize = chunkSize
            };

            VerifyUploadResponse uploadStatus = await ContinueUploadFileAsync(uploadRequest);
            uploadRequest.BytesWritten = uploadStatus.BytesWritten;

            return uploadRequest;
        }

        public async Task<UploadTicket> GetUploadTicketAsync()
        {
            try
            {
                IApiRequest request = GenerateUploadTicketRequest();
                IRestResponse<UploadTicket> response = await request.ExecuteRequestAsync<UploadTicket>();
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

        public async Task<VerifyUploadResponse> VerifyUploadFileAsync(IUploadRequest uploadRequest)
        {
            try
            {
                IApiRequest request =
                    await GenerateFileStreamRequest(uploadRequest.File, uploadRequest.Ticket, verifyOnly: true);
                IRestResponse response = await request.ExecuteRequestAsync();
                var verify = new VerifyUploadResponse();
                CheckStatusCodeError(uploadRequest, response, "Error verifying file upload.", (HttpStatusCode) 308);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    verify.BytesWritten = uploadRequest.FileLength;
                    verify.Status = UploadStatusEnum.Completed;
                }
                else if (response.StatusCode == (HttpStatusCode) 308)
                {
                    verify.Status = UploadStatusEnum.InProgress;
                    int startIndex = 0;
                    int endIndex = 0;
                    Parameter rangeHeader =
                        response.Headers.FirstOrDefault(h => string.Compare(h.Name, "Range", true) == 0);
                    if (rangeHeader != null && rangeHeader.Value != null)
                    {
                        Match match = _rangeRegex.Match(rangeHeader.Value as string);
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

        private IApiRequest GenerateUploadTicketRequest()
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.POST;
            request.Path = Endpoints.UploadTicket;
            request.Query.Add("type", "streaming");
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

            IApiRequest request = _apiRequestFactory.GetApiRequest();
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

        private IApiRequest GenerateCompleteUploadRequest(UploadTicket ticket)
        {
            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.DELETE;
            request.Path = ticket.complete_uri;
            return request;
        }

        #endregion

        #region Helper Functions

        private void ThrowIfUnauthorized()
        {
            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                throw new InvalidOperationException("Please authenticate via OAuth to obtain an access token.");
            }
        }

        private void CheckStatusCodeError(IUploadRequest request, IRestResponse response, string message,
            params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null &&
                !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoUploadException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine, message, response.StatusCode, response.Content),
                    request);
            }
        }

        private void CheckStatusCodeError(IRestResponse response, string message,
            params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null &&
                !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoApiException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine, message, response.StatusCode, response.Content));
            }
        }

        private bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            var code = (int) statusCode;
            return code >= 200 && code < 300;
        }

        #endregion
    }
}
