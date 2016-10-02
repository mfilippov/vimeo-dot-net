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
        /// This is the full user response for the authenticated user.
        /// If you generated your token using the client credentials grant, this value is left out.
        /// </summary>
        public User user { get; set; }
    }
}