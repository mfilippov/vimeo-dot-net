using System.Collections.Generic;
using System.Threading.Tasks;
using VimeoDotNet.Models;

namespace VimeoDotNet.Authorization
{
    /// <summary>
    /// IAuthorizationClient
    /// Additional info https://developer.vimeo.com/api/authentication
    /// </summary>
    public interface IAuthorizationClient
    {
        /// <summary>
        /// GetAccessToken
        /// </summary>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <returns>Access token response</returns>
        /// [Obsolete("Use async API instead sync wrapper")]
        AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUri);

        /// <summary>
        /// GetAccessTokenAsync
        /// </summary>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <returns>Access token response</returns>
        Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUri);

        /// <summary>
        /// VerifyAccessToken
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <returns>true if access token works, false otherwise</returns>
        /// [Obsolete("Use async API instead sync wrapper")]
        bool VerifyAccessToken(string accessToken);

        /// <summary>
        /// VerifyAccessToken
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <returns>true if access token works, false otherwise</returns>
        /// [Obsolete("Use async API instead sync wrapper")]
        Task<bool> VerifyAccessTokenAsync(string accessToken);

        /// <summary>
        /// Return unauthenticated token
        /// </summary>
        /// <returns>Access token response</returns>
        Task<AccessTokenResponse> GetUnauthenticatedTokenAsync();

        /// <summary>
        /// GetAuthorizationEndpoint
        /// </summary>
        /// <param name="redirectUri">RedirectUri</param>
        /// <param name="scope">Scope</param>
        /// <param name="state">State</param>
        /// <returns>Authorization endpoint</returns>
        string GetAuthorizationEndpoint(string redirectUri, IEnumerable<string> scope, string state);
    }
}