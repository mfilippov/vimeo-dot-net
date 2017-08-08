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

namespace VimeoDotNet.Authorization
{
    /// <summary>
    /// Authorization client
    /// </summary>
    public class AuthorizationClient : IAuthorizationClient
    {
        #region Private Properties

        /// <summary>
        ///
        /// </summary>
        protected string ClientId { get; set; }
        /// <summary>
        ///
        /// </summary>
        protected string ClientSecret { get; set; }

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

        /// <summary>
        /// Get authorization endpoint
        /// </summary>
        /// <param name="redirectUri">RedirectUri</param>
        /// <param name="scope">Scope</param>
        /// <param name="state">State</param>
        /// <returns>Authorization endpoint</returns>
        /// <exception cref="InvalidOperationException">Empty ClientId</exception>
        /// <exception cref="ArgumentException">Empty redirectUri</exception>
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

            string queryString = BuildAuthorizeQueryString(redirectUri, scope, state);
            return BuildUrl(Endpoints.Authorize, queryString);
        }

        /// <summary>
        /// GetAccessToken
        /// </summary>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <returns>Access token response</returns>
        public AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUri)
        {
            return GetAccessTokenAsync(authorizationCode, redirectUri).Result;
        }

        /// <summary>
        /// Get access token asynchronously
        /// </summary>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <returns>Access token response</returns>
        public async Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUri)
        {
            ApiRequest request = BuildAccessTokenRequest(authorizationCode, redirectUri);
            IApiResponse<AccessTokenResponse> result = await request.ExecuteRequestAsync<AccessTokenResponse>();
            return result.Content;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Build access token request
        /// </summary>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <returns>Access token request</returns>
        /// <exception cref="InvalidOperationException">Empty ClientId</exception>
        /// <exception cref="ArgumentException">Empty ClientSecret</exception>
        protected ApiRequest BuildAccessTokenRequest(string authorizationCode, string redirectUri)
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

            var request = new ApiRequest(ClientId, ClientSecret);
            request.Method = HttpMethod.Post;
            request.Path = Endpoints.AccessToken;
            SetAccessTokenQueryParams(request, authorizationCode, redirectUri);

            return request;
        }

        /// <summary>
        /// Build url
        /// </summary>
        /// <param name="route">Route</param>
        /// <param name="queryString">QueryString</param>
        /// <returns>URL</returns>
        protected string BuildUrl(string route, string queryString)
        {
            return string.Format("{0}://{1}{2}{3}", Request.DefaultProtocol, Request.DefaultHostName, route, queryString);
        }

        /// <summary>
        /// Set access token query params
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        protected void SetAccessTokenQueryParams(ApiRequest request, string authorizationCode, string redirectUri)
        {
            request.Query.Add("grant_type", "authorization_code");
            request.Query.Add("code", authorizationCode);
            request.Query.Add("redirect_uri", redirectUri);
        }

        /// <summary>
        /// Build authorize query string
        /// </summary>
        /// <param name="redirectUri">RedirectUri</param>
        /// <param name="scope">Scope</param>
        /// <param name="state">State</param>
        /// <returns>Authorize query string</returns>
        protected string BuildAuthorizeQueryString(string redirectUri, IEnumerable<string> scope, string state)
        {
            var qsParams = new Dictionary<string, string>
            {
                {"response_type", "code"},
                {"client_id", ClientId},
                {"redirect_uri", redirectUri}
            };

            if (scope == null)
            {
                qsParams.Add("scope", Scopes.Public);
            }
            else
            {
                qsParams.Add("scope", string.Join(" ", scope.ToArray()));
            }

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
        protected string GetQueryString(IDictionary<string, string> queryParams)
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
    }
}