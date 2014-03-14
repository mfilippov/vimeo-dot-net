using System;

namespace VimeoDotNet.Authorization
{
    public interface IAuthorizationClient
    {
        VimeoDotNet.Models.AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUri);
        System.Threading.Tasks.Task<VimeoDotNet.Models.AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUri);
        string GetAuthorizationEndpoint(string redirectUri, System.Collections.Generic.IEnumerable<string> scope, string state);
    }
}
