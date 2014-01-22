using System;
using System.Collections.Generic;
using VimeoDotNet.Authorization;
using VimeoDotNet.Models;

namespace VimeoDotNet
{
    public class VimeoClient
    {
        #region Constants


        #endregion

        #region Private Fields


        #endregion

        #region Properties

        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string AccessToken { get; set; }
        private AuthorizationClient OAuth2Client { get; set; }

        #endregion

        #region Constructors

        public VimeoClient(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            OAuth2Client = new AuthorizationClient(ClientId, ClientSecret);
        }

        public VimeoClient(string accessToken)
        {
            AccessToken = accessToken;
        }

        public VimeoClient(string clientId, string clientSecret, string accessToken)
            :this(clientId, clientSecret)
        {
            AccessToken = accessToken;
        }

        #endregion

        #region Authorization

        public string GetOauthUrl(string redirectUri, IEnumerable<string> scope, string state)
        {
            PrepAuthorizationClient();
            return OAuth2Client.GetAuthorizationEndpoint(redirectUri, scope, state);
        }

        public AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl)
        {
            PrepAuthorizationClient();
            return OAuth2Client.GetAccessToken(authorizationCode, redirectUrl);
        }

        private void PrepAuthorizationClient() {
            if (OAuth2Client == null) {
                OAuth2Client = new AuthorizationClient(ClientId, ClientSecret);
            }
        }

        #endregion

        #region Videos

        #endregion

    }
}
