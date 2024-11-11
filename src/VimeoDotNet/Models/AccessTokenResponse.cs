using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Response on aceess token request
    /// </summary>
    public class AccessTokenResponse
    {
        /// <summary>
        /// The token you use to make an API request
        /// </summary>
        /// <value>The access token.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The type of token (Vimeo only supports Bearer at the moment)
        /// </summary>
        /// <value>The type of the token.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// The final scopes the token was granted. This list MAY be different from the scopes you requested. This will be the actual permissions the token has been granted.
        /// </summary>
        /// <value>The scope.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        /// <summary>
        /// This is the full user response for the authenticated user.
        /// If you generated your token using the client credentials grant, this value is left out.
        /// </summary>
        /// <value>The user.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
    }
}