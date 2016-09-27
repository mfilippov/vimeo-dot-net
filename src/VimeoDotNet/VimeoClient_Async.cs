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
using VimeoDotNet.Parameters;

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
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.GET,
				Endpoints.GetCurrentUserEndpoint(Endpoints.User)
			);

			return await ExecuteApiRequest<User>(request);
        }

		public async Task<User> UpdateAccountInformationAsync(EditUserParameters parameters)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.PATCH,
				Endpoints.GetCurrentUserEndpoint(Endpoints.User),
				null,
				parameters
			);

			return await ExecuteApiRequest<User>(request);
		}


        public async Task<User> GetUserInformationAsync(long userId)
        {
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.GET,
				Endpoints.User,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() }
				}
			);

			return await ExecuteApiRequest<User>(request);
        }

		#endregion

		#region Albums

		public async Task<Paginated<Album>> GetAlbumsAsync(GetAlbumsParameters parameters = null)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.GET,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbums),
				null,
				parameters
			);

			return await ExecuteApiRequest<Paginated<Album>>(request);
		}

		public async Task<Paginated<Album>> GetAlbumsAsync(long userId, GetAlbumsParameters parameters = null)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.GET,
				Endpoints.UserAlbums,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() }
				},
				parameters
			);

			return await ExecuteApiRequest<Paginated<Album>>(request);
		}

		public async Task<Album> GetAlbumAsync(long albumId)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.GET,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() }
				},
				null
			);

			return await ExecuteApiRequest<Album>(request);
		}

		public async Task<Album> GetAlbumAsync(long userId, long albumId)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.GET,
				Endpoints.UserAlbum,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() },
					{ "albumId", albumId.ToString() }
				},
				null
			);

			return await ExecuteApiRequest<Album>(request);
		}

		public async Task<Album> CreateAlbumAsync(EditAlbumParameters parameters = null)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.POST,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbums),
				null,
				parameters
			);

			return await ExecuteApiRequest<Album>(request);
		}

		public async Task<Album> UpdateAlbumAsync(long albumId, EditAlbumParameters parameters = null)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.PATCH,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() }
				},
				parameters
			);

			return await ExecuteApiRequest<Album>(request);
		}

		public async Task<bool> DeleteAlbumAsync(long albumId)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.DELETE,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}

		public async Task<bool> AddToAlbumAsync(long albumId, long clipId)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.PUT,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbumVideo),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() },
					{ "clipId", clipId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}

		public async Task<bool> AddToAlbumAsync(long userId, long albumId, long clipId)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.PUT,
				Endpoints.UserAlbumVideo,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() },
					{ "albumId", albumId.ToString() },
					{ "clipId", clipId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}

		public async Task<bool> RemoveFromAlbumAsync(long albumId, long clipId)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.DELETE,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbumVideo),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() },
					{ "clipId", clipId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}

		public async Task<bool> RemoveFromAlbumAsync(long userId, long albumId, long clipId)
		{
			IApiRequest request = _apiRequestFactory.AuthorizedRequest(
				AccessToken,
				Method.DELETE,
				Endpoints.UserAlbumVideo,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() },
					{ "albumId", albumId.ToString() },
					{ "clipId", clipId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}
		
		

		#endregion

		#region Utility

		/// <summary>
		/// Utility method for calling ExecuteApiRequest with the most common use case (returning
		/// null for NotFound responses).
		/// </summary>
		/// <typeparam name="T">Type of the expected response data.</typeparam>
		/// <param name="request"></param>
		/// <returns></returns>
		private async Task<T> ExecuteApiRequest<T>(IApiRequest request) where T : new()
		{
			return await ExecuteApiRequest<T>(request, (statusCode) => default(T), new []{ HttpStatusCode.NotFound } );
		}

		/// <summary>
		/// Utility method for performing API requests that retrieve data in a consistent manner.  
		/// 
		/// The given request will be performed, and if the response is an outright success then
		/// the response data will be unwrapped and returned.  
		/// 
		/// If the call is not an outright success, but the status code is among the other acceptable 
		/// results (provided via validStatusCodes), the getValueForStatusCode method will be called
		/// to generate a return value. This allows the caller to return null or an empty list as 
		/// desired.
		/// 
		/// If neither of the above is possible, an exception will be thrown.
		/// </summary>
		/// <typeparam name="T">Type of the expected response data.</typeparam>
		/// <param name="request"></param>
		/// <param name="getValueForStatusCode"></param>
		/// <param name="validStatusCodes"></param>
		/// <returns></returns>
		private async Task<T> ExecuteApiRequest<T>(IApiRequest request, Func<HttpStatusCode, T> getValueForStatusCode, params HttpStatusCode[] validStatusCodes) where T : new()
		{			
			try
			{
				IRestResponse<T> response = await request.ExecuteRequestAsync<T>();
				UpdateRateLimit(response);

				// if request was successful, return immediately...
				if (IsSuccessStatusCode(response.StatusCode))
				{
					return response.Data;
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
					response.Content
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

		private async Task<bool> ExecuteApiRequest(IApiRequest request, params HttpStatusCode[] validStatusCodes)
		{
			try
			{
				IRestResponse response = await request.ExecuteRequestAsync();
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
					response.Content
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
