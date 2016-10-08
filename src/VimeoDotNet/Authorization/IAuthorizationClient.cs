using System.Collections.Generic;
using System.Threading.Tasks;
using VimeoDotNet.Models;

namespace VimeoDotNet.Authorization
{
    /// <summary>
    /// IAuthorizationClient
    /// </summary>
    public interface IAuthorizationClient
    {
        /// <summary>
        /// GetAccessToken
        /// </summary>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <returns>Access token response</returns>
        AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUri);
        /// <summary>
        /// GetAccessTokenAsync
        /// </summary>
        /// <param name="authorizationCode">AuthorizationCode</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <returns>Access token response</returns>
        Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUri);
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