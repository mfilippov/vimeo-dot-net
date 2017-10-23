using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Response on aceess token request
    /// </summary>
    [Serializable]
    public class AccessTokenResponse
    {
        /// <summary>
        /// The token you use to make an API request
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// The type of token (Vimeo only supports Bearer at the moment)
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// The final scopes the token was granted. This list MAY be different from the scopes you requested. This will be the actual permissions the token has been granted.
        /// </summary>
        public string scope { get; set; }

        /// <summary>
        /// This is the full user response for the authenticated user.
        /// If you generated your token using the client credentials grant, this value is left out.
        /// </summary>
        public User user { get; set; }
    }
}