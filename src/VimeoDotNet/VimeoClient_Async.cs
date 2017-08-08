using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VimeoDotNet.Authorization;
using VimeoDotNet.Constants;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    public partial class VimeoClient: IVimeoClient
    {
        #region Constants

        internal const int DEFAULT_UPLOAD_CHUNK_SIZE = 1048576; // 1MB
        /// <summary>
        /// Range regex
        /// </summary>
        protected static readonly Regex RangeRegex = new Regex(@"bytes\s*=\s*(?<start>\d+)-(?<end>\d+)",
            RegexOptions.IgnoreCase);

        #endregion

        #region Fields
        /// <summary>
        /// Api request factory
        /// </summary>
        protected IApiRequestFactory ApiRequestFactory;
        /// <summary>
        /// Auth client factory
        /// </summary>
        protected IAuthorizationClientFactory AuthClientFactory;

        #endregion

        #region Properties

        /// <summary>
        /// ClientId
        /// </summary>
        protected string ClientId { get; set; }
        /// <summary>
        /// ClientSecret
        /// </summary>
        protected string ClientSecret { get; set; }
        /// <summary>
        /// AccessToken
        /// </summary>
        protected string AccessToken { get; set; }

        /// <summary>
        /// OAuth2Client
        /// </summary>
        protected IAuthorizationClient OAuth2Client { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        protected VimeoClient()
        {
            AuthClientFactory = new AuthorizationClientFactory();
            ApiRequestFactory = new ApiRequestFactory();
        }

        /// <summary>
        /// Multi-user application constructor, using user-level OAuth2
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">ClientSecret</param>
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

        /// <summary>
        ///     Multi-user Constructor for use with IVimeoClientFactory
        /// </summary>
        /// <param name="authClientFactory">The IAuthorizationClientFactory</param>
        /// <param name="apiRequestFactory">The IApiRequestFactory</param>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">ClientSecret</param>
        internal VimeoClient(IAuthorizationClientFactory authClientFactory, IApiRequestFactory apiRequestFactory,
            string clientId, string clientSecret)
            : this(clientId, clientSecret)
        {
            AuthClientFactory = authClientFactory;
            ApiRequestFactory = apiRequestFactory;
        }

        /// <summary>
        ///     Single-user Constructor for use with IVimeoClientFactory
        /// </summary>
        /// <param name="authClientFactory">The IAuthorizationClientFactory</param>
        /// <param name="apiRequestFactory">The IApiRequestFactory</param>
        /// <param name="accessToken">AccessToken</param>
        internal VimeoClient(IAuthorizationClientFactory authClientFactory, IApiRequestFactory apiRequestFactory,
            string accessToken)
            : this(accessToken)
        {
            AuthClientFactory = authClientFactory;
            ApiRequestFactory = apiRequestFactory;
        }

        #endregion

        #region Authorization

        /// <summary>
        /// Return authorztion URL
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <param name="scope">Defaults to "public" and "private"; this is a space-separated list of <a href="#supported-scopes">scopes</a> you want to access</param>
        /// <param name="state">A unique value which the client will return alongside access tokens</param>
        /// <returns>Authorization URL</returns>
        public string GetOauthUrl(string redirectUri, IEnumerable<string> scope, string state)
        {
            PrepAuthorizationClient();
            return OAuth2Client.GetAuthorizationEndpoint(redirectUri, scope, state);
        }

        /// <summary>
        /// Exchange the code for an access token asynchronously
        /// </summary>
        /// <param name="authorizationCode">A string token you must exchange for your access token</param>
        /// <param name="redirectUrl">This field is required, and must match one of your application’s
        /// redirect URI’s</param>
        /// <returns></returns>
        public async Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUrl)
        {
            PrepAuthorizationClient();
            return await OAuth2Client.GetAccessTokenAsync(authorizationCode, redirectUrl);
        }

        private void PrepAuthorizationClient()
        {
            if (OAuth2Client == null)
            {
                OAuth2Client = AuthClientFactory.GetAuthorizationClient(ClientId, ClientSecret);
            }
        }

        #endregion

        #region Account

        /// <summary>
        /// Get user information asynchronously
        /// </summary>
        /// <returns>User information</returns>
        public async Task<User> GetAccountInformationAsync()
        {
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Get,
				Endpoints.GetCurrentUserEndpoint(Endpoints.User)
			);

			return await ExecuteApiRequest<User>(request);
        }

        /// <summary>
        /// Update user information asynchronously
        /// </summary>
        /// <param name="parameters">User parameters</param>
        /// <returns>User information</returns>
        public async Task<User> UpdateAccountInformationAsync(EditUserParameters parameters)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				new HttpMethod("PATCH"),
				Endpoints.GetCurrentUserEndpoint(Endpoints.User),
				null,
				parameters
			);

			return await ExecuteApiRequest<User>(request);
		}


        /// <summary>
        /// Get user information async
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>User information object</returns>
        public async Task<User> GetUserInformationAsync(long userId)
        {
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Get,
				Endpoints.User,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() }
				}
			);

			return await ExecuteApiRequest<User>(request);
        }

		#endregion

		#region Albums

        /// <summary>
        /// Get album by parameters asynchronously
        /// </summary>
        /// <param name="parameters">GetAlbumsParameters</param>
        /// <returns>Paginated albums</returns>
        public async Task<Paginated<Album>> GetAlbumsAsync(GetAlbumsParameters parameters = null)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Get,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbums),
				null,
				parameters
			);

			return await ExecuteApiRequest<Paginated<Album>>(request);
		}

        /// <summary>
        /// Get album by UserId and parameters asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="parameters">GetAlbumsParameters</param>
        /// <returns>Paginated albums</returns>
        public async Task<Paginated<Album>> GetAlbumsAsync(long userId, GetAlbumsParameters parameters = null)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Get,
				Endpoints.UserAlbums,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() }
				},
				parameters
			);

			return await ExecuteApiRequest<Paginated<Album>>(request);
		}

        /// <summary>
        /// Get album by AlbumId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Album</returns>
        public async Task<Album> GetAlbumAsync(long albumId)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Get,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() }
				},
				null
			);

			return await ExecuteApiRequest<Album>(request);
		}

        /// <summary>
        /// Get album by AlbumId and UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Album</returns>
        public async Task<Album> GetAlbumAsync(long userId, long albumId)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Get,
				Endpoints.UserAlbum,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() },
					{ "albumId", albumId.ToString() }
				},
				null
			);

			return await ExecuteApiRequest<Album>(request);
		}

        /// <summary>
        /// Create new album asynchronously
        /// </summary>
        /// <param name="parameters">Creation parameters</param>
        /// <returns>Album</returns>
        public async Task<Album> CreateAlbumAsync(EditAlbumParameters parameters = null)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Post,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbums),
				null,
				parameters
			);

			return await ExecuteApiRequest<Album>(request);
		}

        /// <summary>
        /// Update album asynchronously
        /// </summary>
        /// <param name="albumId">Albumid</param>
        /// <param name="parameters">Album parameters</param>
        /// <returns>Album</returns>
        public async Task<Album> UpdateAlbumAsync(long albumId, EditAlbumParameters parameters = null)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				new HttpMethod("PATCH"),
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() }
				},
				parameters
			);

			return await ExecuteApiRequest<Album>(request);
		}

        /// <summary>
        /// Delete album asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Deletion result</returns>
        public async Task<bool> DeleteAlbumAsync(long albumId)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Delete,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}

        /// <summary>
        /// Add video to album by AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Adding result</returns>
        public async Task<bool> AddToAlbumAsync(long albumId, long clipId)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Put,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbumVideo),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() },
					{ "clipId", clipId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}

        /// <summary>
        /// Add video to album by UserId and AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Adding result</returns>
        public async Task<bool> AddToAlbumAsync(long userId, long albumId, long clipId)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Put,
				Endpoints.UserAlbumVideo,
				new Dictionary<string, string>(){
					{ "userId", userId.ToString() },
					{ "albumId", albumId.ToString() },
					{ "clipId", clipId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}

        /// <summary>
        /// Remove video from album by AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Removing result</returns>
        public async Task<bool> RemoveFromAlbumAsync(long albumId, long clipId)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Delete,
				Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbumVideo),
				new Dictionary<string, string>(){
					{ "albumId", albumId.ToString() },
					{ "clipId", clipId.ToString() }
				}
			);

			return await ExecuteApiRequest(request);
		}

        /// <summary>
        /// Remove video from album by AlbumId and ClipId and UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Removing result</returns>
        public async Task<bool> RemoveFromAlbumAsync(long userId, long albumId, long clipId)
		{
			var request = ApiRequestFactory.AuthorizedRequest(
				AccessToken,
				HttpMethod.Delete,
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
				var response = await request.ExecuteRequestAsync<T>();
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
					response.StatusCode
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
				var response = await request.ExecuteRequestAsync();
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
					response.StatusCode
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

        private void CheckStatusCodeError(IUploadRequest request, IApiResponse response, string message,
            params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null &&
                !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoUploadException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine, message, response.StatusCode),
                    request);
            }
        }

        private void CheckStatusCodeError(IApiResponse response, string message,
            params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null &&
                !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoApiException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine, message, response.StatusCode));
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
