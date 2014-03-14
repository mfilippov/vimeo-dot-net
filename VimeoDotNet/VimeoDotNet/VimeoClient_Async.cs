using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VimeoDotNet.Authorization;
using VimeoDotNet.Constants;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        #region Constants        

        private const int DEFAULT_CHUNK_SIZE = 262144; // 256kb
        private static readonly Regex _rangeRegex = new Regex(@"bytes\s*=\s*(?<start>\d+)-(?<end>\d+)", RegexOptions.IgnoreCase);

        #endregion

        #region Private Fields


        #endregion

        #region Properties

        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string AccessToken { get; set; }
        private AuthorizationClient OAuth2Client { get; set; }

        #endregion

        #region Constructors

        public VimeoClient(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            OAuth2Client = new AuthorizationClient(ClientId, ClientSecret);
        }

        public VimeoClient(string accessToken)
        {
            AccessToken = accessToken;
        }

        public VimeoClient(string clientId, string clientSecret, string accessToken)
            :this(clientId, clientSecret)
        {
            AccessToken = accessToken;
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

        private void PrepAuthorizationClient() {
            if (OAuth2Client == null) {
                OAuth2Client = new AuthorizationClient(ClientId, ClientSecret);
            }
        }

        #endregion

        #region Account

        public async Task<User> GetAccountInformationAsync()
        {
            try
            {
                var request = GenerateAccountInformationRequest();
                var response = await request.ExecuteRequestAsync<User>();
                CheckStatusCodeError(response, "Error retrieving account information.");

                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException) { throw; }
                throw new VimeoApiException("Error retrieving account information.", ex);
            }
        }

        private ApiRequest GenerateAccountInformationRequest()
        {
            ThrowIfUnauthorized();

            var request = new ApiRequest(AccessToken);
            request.Method = Method.GET;
            request.Path = Endpoints.GetCurrentUserEndpoint(Endpoints.User);
            return request;
        }

        #endregion

        #region Videos

        public async Task<Paginated<Video>> GetAccountVideosAsync()
        {
            try {
                var request = GenerateVideosRequest(null);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>();
                CheckStatusCodeError(response, "Error retrieving account videos.");

                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException) { throw; }
                throw new VimeoApiException("Error retrieving account videos.", ex);
            }
        }

        public async Task<Paginated<Video>> GetUserVideosAsync(string userId)
        {
            try {
                var request = GenerateVideosRequest(userId);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>();
                CheckStatusCodeError(response, "Error retrieving user videos.");

                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException) { throw; }
                throw new VimeoApiException("Error retrieving user videos.", ex);
            }
        }

        private ApiRequest GenerateVideosRequest(string userId)
        {
            ThrowIfUnauthorized();

            var request = new ApiRequest(AccessToken);
            request.Method = Method.GET;
            request.Path = Endpoints.UserVideos;

            if (userId != null) {
                request.UrlSegments.Add("userId", userId);
            }
            else {
                request.Path = Endpoints.GetCurrentUserEndpoint(request.Path);
            }

            return request;
        }

        #endregion

        #region Upload

        public async Task<UploadRequest> UploadEntireFileAsync(BinaryContent fileContent, int chunkSize = DEFAULT_CHUNK_SIZE)
        {
            var uploadRequest = await StartUploadFileAsync(fileContent, chunkSize);

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
                    else if (uploadStatus.BytesWritten == uploadRequest.FileLength) {
                        // Supposedly all bytes are written, but Vimeo doesn't think so, so just bail out
                        throw new VimeoUploadException(
                            string.Format("Vimeo failed to mark file as completed, Bytes Written: {0:N0}, Expected: {1:N0}.",
                                uploadStatus.BytesWritten),
                            uploadRequest);
                    }
                }
            }

            return uploadRequest;
        }

        public async Task<UploadRequest> StartUploadFileAsync(BinaryContent fileContent, int chunkSize = DEFAULT_CHUNK_SIZE)
        {
            if (!fileContent.Data.CanRead)
            {
                throw new ArgumentException("fileContent should be readable");
            }
            if (fileContent.Data.CanSeek && fileContent.Data.Position > 0)
            {
                fileContent.Data.Position = 0;
            }

            var ticket = await GetUploadTicketAsync();

            var uploadRequest = new UploadRequest()
            {
                Ticket = ticket,
                File = fileContent,
                ChunkSize = chunkSize
            };

            var uploadStatus = await ContinueUploadFileAsync(uploadRequest);
            uploadRequest.BytesWritten = uploadStatus.BytesWritten;

            return uploadRequest;
        }

        public async Task<UploadTicket> GetUploadTicketAsync()
        {
            try
            {
                var request = GenerateUploadTicketRequest();
                var response = await request.ExecuteRequestAsync<UploadTicket>();
                CheckStatusCodeError(null, response, "Error generating upload ticket.");

                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException) { throw; }
                throw new VimeoUploadException("Error generating upload ticket.", null, ex);
            }
        }

        public async Task<VerifyUploadResponse> ContinueUploadFileAsync(UploadRequest uploadRequest)
        {
            if (uploadRequest.AllBytesWritten) {
                // Already done, there's nothing to do.
                return new VerifyUploadResponse() {
                    Status = UploadStatusEnum.InProgress,
                    BytesWritten = uploadRequest.BytesWritten
                };
            }

            try
            {
                var request = await GenerateFileStreamRequest(uploadRequest.File, uploadRequest.Ticket,
                    chunkSize: uploadRequest.ChunkSize, written: uploadRequest.BytesWritten);
                var response = await request.ExecuteRequestAsync();
                CheckStatusCodeError(uploadRequest, response, "Error uploading file chunk.",
                    new[] { HttpStatusCode.OK, HttpStatusCode.BadRequest });

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // something went wrong, figure out where we need to start over
                    return await VerifyUploadFileAsync(uploadRequest);
                }

                // Success, update total written
                uploadRequest.BytesWritten += uploadRequest.ChunkSize;
                uploadRequest.BytesWritten = Math.Min(uploadRequest.BytesWritten, uploadRequest.FileLength);
                return new VerifyUploadResponse()
                {
                    Status = UploadStatusEnum.InProgress,
                    BytesWritten = uploadRequest.BytesWritten
                };
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException) { throw; }
                throw new VimeoUploadException("Error uploading file chunk", uploadRequest, ex);
            }

        }
        
        public async Task<VerifyUploadResponse> VerifyUploadFileAsync(UploadRequest uploadRequest)
        {
            try
            {
                var request = await GenerateFileStreamRequest(uploadRequest.File, uploadRequest.Ticket, verifyOnly: true);
                var response = await request.ExecuteRequestAsync();
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
                    var rangeHeader = response.Headers.FirstOrDefault(h => string.Compare(h.Name, "Range", true) == 0);
                    if (rangeHeader != null && rangeHeader.Value != null)
                    {
                        var match = _rangeRegex.Match(rangeHeader.Value as string);
                        if (match.Success
                            && int.TryParse(match.Groups["start"].Value, out startIndex)
                            && int.TryParse(match.Groups["end"].Value, out endIndex)) {
                            verify.BytesWritten = endIndex - startIndex;
                            if (verify.BytesWritten == uploadRequest.FileLength) {
                                verify.Status = UploadStatusEnum.Completed;
                            }
                        }
                    }
                }
                else {
                    verify.Status = UploadStatusEnum.NotFound;
                }

                return verify;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException) { throw; }
                throw new VimeoUploadException("Error verifying file upload.", uploadRequest, ex);
            }
        }

        public async Task CompleteFileUploadAsync(UploadRequest uploadRequest)
        {
            ThrowIfUnauthorized();

            try {
                var request = GenerateCompleteUploadRequest(uploadRequest.Ticket);
                var response = await request.ExecuteRequestAsync();
                CheckStatusCodeError(uploadRequest, response, "Error marking file upload as complete.");

                var locationHeader = response.Headers.FirstOrDefault(h => string.Compare(h.Name, "Location", true) == 0);
                if (locationHeader != null && locationHeader.Value != null)
                {
                    uploadRequest.ClipUri = locationHeader.Value as string;
                }
            }
            catch (Exception ex) {
                if (ex is VimeoApiException) { throw; }
                throw new VimeoUploadException("Error marking file upload as complete.", uploadRequest, ex);
            }
        }

        private ApiRequest GenerateUploadTicketRequest()
        {
            ThrowIfUnauthorized();

            var request = new ApiRequest(AccessToken);
            request.Method = Method.POST;
            request.Path = Endpoints.UploadTicket;
            request.Query.Add("type", "streaming");
            return request;
        }

        private async Task<ApiRequest> GenerateFileStreamRequest(BinaryContent fileContent, UploadTicket ticket, long written = 0, int? chunkSize = null, bool verifyOnly = false)
        {
            if (ticket == null || string.IsNullOrWhiteSpace(ticket.ticket_id))
            {
                throw new ArgumentException("Invalid upload ticket.");
            }
            if (fileContent.Data.Length > ticket.user.upload_quota.space.free)
            {
                throw new InvalidOperationException("User does not have enough free space to upload this video. Remaining space: " + ticket.quota.free_space + ".");
            }

            var request = new ApiRequest();
            request.Method = Method.PUT;
            request.ExcludeAuthorizationHeader = true;
            request.Path = ticket.upload_link_secure;
            request.Headers.Add(Request.HeaderContentType, fileContent.ContentType);
            if (verifyOnly) {
                    request.Headers.Add(Request.HeaderContentLength, "0");
                    request.Headers.Add(Request.HeaderContentRange, "bytes */*");
            }
            else {
                if (chunkSize.HasValue)
                {
                    long startIndex = fileContent.Data.CanSeek ? fileContent.Data.Position : written;
                    long endIndex = Math.Min(startIndex + chunkSize.Value, fileContent.Data.Length);
                    request.Headers.Add(Request.HeaderContentLength, (endIndex - startIndex).ToString());
                    request.Headers.Add(Request.HeaderContentRange, string.Format("bytes {0}-{1}/{2}", startIndex, endIndex, fileContent.Data.Length));
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

        private ApiRequest GenerateCompleteUploadRequest(UploadTicket ticket)
        {
            var request = new ApiRequest(AccessToken);
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

        private void CheckStatusCodeError(UploadRequest request, IRestResponse response, string message, params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null && !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoUploadException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine, message, response.StatusCode, response.Content),
                    request);
            }
        }

        private void CheckStatusCodeError(IRestResponse response, string message, params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null && !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoApiException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine, message, response.StatusCode, response.Content));
            }
        }

        private bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            int code = (int)statusCode;
            return code >= 200 && code < 300;
        }

        #endregion
    }
}
