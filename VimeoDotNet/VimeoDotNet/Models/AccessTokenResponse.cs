using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class AccessTokenResponse
    {
        public string access_token { get; set; }
        public User user { get; set; }
    }
}
