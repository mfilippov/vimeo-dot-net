using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VimeoDotNet.Authorization;
using VimeoDotNet.Constants;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    /// <summary>
    /// Class VimeoClient.
    /// Implements the <see cref="VimeoDotNet.IVimeoClient" />
    /// </summary>
    /// <seealso cref="VimeoDotNet.IVimeoClient" />
    public partial class VimeoClient : IVimeoClient
    {
        #region Constants

        /// <summary>
        /// The default upload chunk size
        /// </summary>
        internal const long DefaultUploadChunkSize = 134217728; // 128MB

        /// <summary>
        /// Range regex
        /// </summary>
        private static readonly Regex RangeRegex = new Regex(@"bytes\s*=\s*(?<start>\d+)-(?<end>\d+)",
            RegexOptions.IgnoreCase);

        #endregion

        #region Fields

        /// <summary>
        /// Api request factory
        /// </summary>
        private readonly IApiRequestFactory _apiRequestFactory;

        /// <summary>
        /// Auth client factory
        /// </summary>
        private readonly IAuthorizationClientFactory _authClientFactory;

        #endregion

        #region Properties

        /// <summary>
        /// ClientId
        /// </summary>
        /// <value>The client identifier.</value>
        private string ClientId { get; }

        /// <summary>
        /// ClientSecret
        /// </summary>
        /// <value>The client secret.</value>
        private string ClientSecret { get; }

        /// <summary>
        /// AccessToken
        /// </summary>
        /// <value>The access token.</value>
        private string AccessToken { get; }

        /// <summary>
        /// OAuth2Client
        /// </summary>
        /// <value>The o auth2 client.</value>
        private IAuthorizationClient OAuth2Client { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="VimeoClient"/> class from being created.
        /// </summary>
        private VimeoClient()
        {
            _authClientFactory = new AuthorizationClientFactory();
            _apiRequestFactory = new ApiRequestFactory();
            RateLimit = 0;
            RateLimitRemaining = 0;
            RateLimitReset = DateTime.UtcNow;
        }

        /// <summary>
        /// Multi-user application constructor, using user-level OAuth2
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">ClientSecret</param>
        [PublicAPI]
        public VimeoClient(string clientId, string clientSecret)
            : this()
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            OAuth2Client = new AuthorizationClient(ClientId, ClientSecret);
        }

        /// <summary>
        /// Single-user application constructor, using account OAuth2 access token
        /// </summary>
        /// <param name="accessToken">Your Vimeo API Access Token</param>
        public VimeoClient(string accessToken)
            : this()
        {
            AccessToken = accessToken;
        }

        /// <inheritdoc />
        internal VimeoClient(IAuthorizationClientFactory authClientFactory, IApiRequestFactory apiRequestFactory,
            string clientId, string clientSecret)
            : this(clientId, clientSecret)
        {
            _authClientFactory = authClientFactory;
            _apiRequestFactory = apiRequestFactory;
        }

        /// <inheritdoc />
        internal VimeoClient(IAuthorizationClientFactory authClientFactory, IApiRequestFactory apiRequestFactory,
            string accessToken)
            : this(accessToken)
        {
            _authClientFactory = authClientFactory;
            _apiRequestFactory = apiRequestFactory;
        }

        #endregion

        #region Authorization

        /// <inheritdoc />
        public string GetOauthUrl(string redirectUri, IEnumerable<string> scope, string state)
        {
            PrepAuthorizationClient();
            return OAuth2Client.GetAuthorizationEndpoint(redirectUri, scope, state);
        }

        /// <inheritdoc />
        public async Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUrl)
        {
            PrepAuthorizationClient();
            return await OAuth2Client.GetAccessTokenAsync(authorizationCode, redirectUrl).ConfigureAwait(false);
        }

        /// <summary>
        /// Preps the authorization client.
        /// </summary>
        private void PrepAuthorizationClient()
        {
            if (OAuth2Client == null)
            {
                OAuth2Client = _authClientFactory.GetAuthorizationClient(ClientId, ClientSecret);
            }
        }

        #endregion

        #region Account

        /// <inheritdoc />
        public async Task<User> GetAccountInformationAsync()
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                Endpoints.GetCurrentUserEndpoint(Endpoints.User)
            );

            return await ExecuteApiRequest<User>(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<User> UpdateAccountInformationAsync(EditUserParameters parameters)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                new HttpMethod("PATCH"),
                Endpoints.GetCurrentUserEndpoint(Endpoints.User),
                null,
                parameters
            );

            return await ExecuteApiRequest<User>(request).ConfigureAwait(false);
        }


        /// <inheritdoc />
        public async Task<User> GetUserInformationAsync(long userId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                Endpoints.User,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()}
                }
            );

            return await ExecuteApiRequest<User>(request).ConfigureAwait(false);
        }

        #endregion

        #region Utility

        /// <summary>
        /// Utility method for calling ExecuteApiRequest with the most common use case (returning
        /// null for NotFound responses).
        /// </summary>
        /// <typeparam name="T">Type of the expected response data.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>T.</returns>
        private async Task<T> ExecuteApiRequest<T>(IApiRequest request) where T : new()
        {
            return await ExecuteApiRequest(request, statusCode => default(T), HttpStatusCode.NotFound)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Utility method for performing API requests that retrieve data in a consistent manner.
        /// The given request will be performed, and if the response is an outright success then
        /// the response data will be unwrapped and returned.
        /// If the call is not an outright success, but the status code is among the other acceptable
        /// results (provided via validStatusCodes), the getValueForStatusCode method will be called
        /// to generate a return value. This allows the caller to return null or an empty list as
        /// desired.
        /// If neither of the above is possible, an exception will be thrown.
        /// </summary>
        /// <typeparam name="T">Type of the expected response data.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="getValueForStatusCode">The get value for status code.</param>
        /// <param name="validStatusCodes">The valid status codes.</param>
        /// <returns>T.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException"></exception>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error retrieving information from Vimeo API.</exception>
        private async Task<T> ExecuteApiRequest<T>(IApiRequest request, Func<HttpStatusCode, T> getValueForStatusCode,
            params HttpStatusCode[] validStatusCodes) where T : new()
        {
            try
            {
                var response = await request.ExecuteRequestAsync<T>().ConfigureAwait(false);
                UpdateRateLimit(response);

                // if request was successful, return immediately...
                if (IsSuccessStatusCode(response.StatusCode))
                {
                    return response.Content;
                }

                // if request is among other accepted status codes, return the corresponding value...
                if (validStatusCodes != null && validStatusCodes.Contains(response.StatusCode))
                {
                    return getValueForStatusCode(response.StatusCode);
                }

                // at this point, we've eliminated all acceptable responses, throw an exception...
                throw new VimeoApiException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine,
                    "Error retrieving information from Vimeo API.",
                    response.StatusCode,
                    response.Text
                ));
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error retrieving information from Vimeo API.", ex);
            }
        }

        /// <summary>
        /// Executes the API request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="validStatusCodes">The valid status codes.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException"></exception>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error retrieving information from Vimeo API.</exception>
        private async Task<bool> ExecuteApiRequest(IApiRequest request, params HttpStatusCode[] validStatusCodes)
        {
            try
            {
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                UpdateRateLimit(response);
                // if request was successful, return immediately...
                if (IsSuccessStatusCode(response.StatusCode))
                {
                    return true;
                }

                // if request is among other accepted status codes, return the corresponding value...
                if (validStatusCodes != null && validStatusCodes.Contains(response.StatusCode))
                {
                    return true;
                }

                // at this point, we've eliminated all acceptable responses, throw an exception...
                throw new VimeoApiException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine,
                    "Error retrieving information from Vimeo API.",
                    response.StatusCode,
                    response.Text
                ));
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error retrieving information from Vimeo API.", ex);
            }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Throws if unauthorized.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Please authenticate via OAuth to obtain an access token.</exception>
        private void ThrowIfUnauthorized()
        {
            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                throw new InvalidOperationException("Please authenticate via OAuth to obtain an access token.");
            }
        }

        /// <summary>
        /// Checks the status code error.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="message">The message.</param>
        /// <param name="validStatusCodes">The valid status codes.</param>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoUploadException"></exception>
        private static void CheckStatusCodeError(IUploadRequest request, IApiResponse response, string message,
            params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null &&
                !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoUploadException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                        Environment.NewLine, message, response.StatusCode, response.Text),
                    request);
            }
        }

        /// <summary>
        /// Checks the status code error.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="message">The message.</param>
        /// <param name="validStatusCodes">The valid status codes.</param>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException"></exception>
        private static void CheckStatusCodeError(IApiResponse response, string message,
            params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null &&
                !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoApiException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine, message, response.StatusCode, response.Text));
            }
        }

        /// <summary>
        /// Determines whether [is success status code] [the specified status code].
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns><c>true</c> if [is success status code] [the specified status code]; otherwise, <c>false</c>.</returns>
        private static bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            var code = (int) statusCode;
            return code >= 200 && code < 300;
        }

        #endregion
    }
}