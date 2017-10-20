using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Exceptions;

namespace VimeoDotNet.Authorization
{
    /// <inheritdoc />
    /// <summary>
    /// Authorization client
    /// </summary>
    public class AuthorizationClient : IAuthorizationClient
    {
        #region Private Properties

        /// <summary>
        /// Client Id
        /// </summary>
        private string ClientId { get; }
        /// <summary>
        /// Client secret
        /// </summary>
        private string ClientSecret { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Authorization client
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">ClientSecret</param>
        public AuthorizationClient(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        /// <inheritdoc />
        public string GetAuthorizationEndpoint(string redirectUri, IEnumerable<string> scope, string state)
        {
            if (string.IsNullOrWhiteSpace(ClientId))
            {
                throw new InvalidOperationException("Authorization.ClientId should be a non-null, non-whitespace string");
            }
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                throw new ArgumentException("redirectUri should be a valid Uri");
            }

            var queryString = BuildAuthorizeQueryString(redirectUri, scope, state);
            return BuildUrl(Endpoints.Authorize, queryString);
        }

        /// <inheritdoc />
        public AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUri)
        {
            return GetAccessTokenAsync(authorizationCode, redirectUri).Result;
        }

        /// <inheritdoc />
        public async Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUri)
        {
            try
            {
              var request = BuildAccessTokenRequest(authorizationCode, redirectUri);
              var result = await request.ExecuteRequestAsync<AccessTokenResponse>();
              CheckStatusCodeError(result, "Error getting access token.");
              return result.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error getting access token.", ex);
            }
        }

        /// <inheritdoc />
        public Boolean VerifyAccessToken(string accessToken)
        {
            return VerifyAccessTokenAsync(accessToken).Result;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyAccessTokenAsync(string accessToken)
        {
            var request = GenerateVerifyRequest(accessToken);
            var result = await request.ExecuteRequestAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public async Task<AccessTokenResponse> GetUnauthenticatedTokenAsync()
        {
            var request = BuildUnauthenticatedTokenRequest();
            var result = await request.ExecuteRequestAsync<AccessTokenResponse>();
            return result.Content;
        }

        #endregion

        #region Private Methods

        private ApiRequest BuildUnauthenticatedTokenRequest(List<string> scopes = null)
        {
            if (string.IsNullOrWhiteSpace(ClientId))
                throw new InvalidOperationException("Authorization.ClientId should be a non-null, non-whitespace string");
            if (string.IsNullOrWhiteSpace(ClientSecret))
                throw new InvalidOperationException(
                    "Authorization.ClientSecret should be a non-null, non-whitespace string");
            var request = new ApiRequest(ClientId, ClientSecret)
            {
                Method = HttpMethod.Post,
                Path = Endpoints.AuthorizeClient
            };
            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };
            if (scopes != null)
                parameters["scope"] = string.Join(" ", scopes);
            request.Body = new FormUrlEncodedContent(parameters);
            return request;
        }
        /// <summary>
        /// Build access token request
        /// </summary>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <returns>Access token request</returns>
        /// <exception cref="InvalidOperationException">Empty ClientId</exception>
        /// <exception cref="ArgumentException">Empty ClientSecret</exception>
        private ApiRequest BuildAccessTokenRequest(string authorizationCode, string redirectUri)
        {
            if (string.IsNullOrWhiteSpace(ClientId))
            {
                throw new InvalidOperationException("Authorization.ClientId should be a non-null, non-whitespace string");
            }
            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                throw new InvalidOperationException(
                    "Authorization.ClientSecret should be a non-null, non-whitespace string");
            }
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                throw new ArgumentException("authorizationCode should be a non-null, non-whitespace string");
            }
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                throw new ArgumentException("redirectUri should be a valid Uri");
            }

            var request = new ApiRequest(ClientId, ClientSecret)
            {
                Method = HttpMethod.Post,
                Path = Endpoints.AccessToken
            };
            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = authorizationCode,
                ["redirect_uri"] = redirectUri
            };
            request.Body = new FormUrlEncodedContent(parameters);
            return request;
        }

        private IApiRequest GenerateVerifyRequest(string AccessToken)
        {
            IApiRequest request = new ApiRequest(AccessToken);
            string endpoint = Endpoints.Verify;
            request.Method = HttpMethod.Get;
            request.Path = endpoint;

            return request;
        }

        /// <summary>
        /// Build url
        /// </summary>
        /// <param name="route">Route</param>
        /// <param name="queryString">QueryString</param>
        /// <returns>URL</returns>
        private static string BuildUrl(string route, string queryString)
        {
            return $"{Request.DefaultProtocol}://{Request.DefaultHostName}{route}{queryString}";
        }

        /// <summary>
        /// Build authorize query string
        /// </summary>
        /// <param name="redirectUri">RedirectUri</param>
        /// <param name="scope">Scope</param>
        /// <param name="state">State</param>
        /// <returns>Authorize query string</returns>
        private string BuildAuthorizeQueryString(string redirectUri, IEnumerable<string> scope, string state)
        {
            var qsParams = new Dictionary<string, string>
            {
                {"response_type", "code"},
                {"client_id", ClientId},
                {"redirect_uri", redirectUri},
                {"scope", scope == null ? Scopes.Public : string.Join(" ", scope.ToArray())}
            };

            if (!string.IsNullOrWhiteSpace(state))
            {
                qsParams.Add("state", state);
            }

            return GetQueryString(qsParams);
        }

        /// <summary>
        /// Return query string
        /// </summary>
        /// <param name="queryParams">QueryParams</param>
        /// <returns>Query string</returns>
        private static string GetQueryString(IDictionary<string, string> queryParams)
        {
            var sb = new StringBuilder("");
            foreach (var qsParam in queryParams)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                
                sb.AppendFormat("{0}={1}", WebUtility.UrlEncode(qsParam.Key), WebUtility.UrlEncode(qsParam.Value));
            }
            sb.Insert(0, "?");
            return sb.ToString();
        }

        #endregion

        #region Helper Functions

        private void CheckStatusCodeError(IApiResponse response, string message,
            params HttpStatusCode[] validStatusCodes)
        {
            if (!IsSuccessStatusCode(response.StatusCode) && validStatusCodes != null &&
                !validStatusCodes.Contains(response.StatusCode))
            {
                throw new VimeoApiException(string.Format("{1}{0}Code: {2}{0}Message: {3}",
                    Environment.NewLine, message, response.StatusCode, response.Text));
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