using System.Collections.Generic;
using System.Threading.Tasks;
using VimeoDotNet.Models;

namespace VimeoDotNet.Authorization
{
    public interface IAuthorizationClient
    {
        AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUri);
        Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUri);
        string GetAuthorizationEndpoint(string redirectUri, IEnumerable<string> scope, string state);
    }
}