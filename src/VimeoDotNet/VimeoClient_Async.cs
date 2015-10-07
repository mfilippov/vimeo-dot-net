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

		#region Albums

		public async Task<Paginated<Album>> GetUserAlbumsAsync(long userId)
		{
			try
			{
				IApiRequest request = GetUserAlbumsRequest(userId);
				IRestResponse<Paginated<Album>> response = await request.ExecuteRequestAsync<Paginated<Album>>();
				CheckStatusCodeError(response, "Error retrieving user albums information.", HttpStatusCode.NotFound);

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
				throw new VimeoApiException("Error retrieving user albums information.", ex);
			}
		}

		public async Task<Paginated<Album>> GetAccountAlbumsAsync()
		{
			try
			{
				IApiRequest request = GetAccountAlbumsRequest();
				IRestResponse<Paginated<Album>> response = await request.ExecuteRequestAsync<Paginated<Album>>();
				CheckStatusCodeError(response, "Error retrieving account albums information.", HttpStatusCode.NotFound);

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
				throw new VimeoApiException("Error retrieving account albums information.", ex);
			}
		}

		private IApiRequest GetUserAlbumsRequest(long userId)
		{
			ThrowIfUnauthorized();

			IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
			request.Method = Method.GET;
			request.Path = Endpoints.UserAlbums;
			
			request.UrlSegments.Add("userId", userId.ToString());

			return request;
		}

		private IApiRequest GetAccountAlbumsRequest()
		{
			ThrowIfUnauthorized();

			IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
			request.Method = Method.GET;
			request.Path = Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbums);

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
            var code = (int)statusCode;
            return code >= 200 && code < 300;
        }

        #endregion
    }
}
