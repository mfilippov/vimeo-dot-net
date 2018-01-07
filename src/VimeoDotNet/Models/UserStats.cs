using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User stats
    /// </summary>
    public class UserStats
    {
        /// <summary>
        /// Videos
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public int Videos { get; set; }

        /// <summary>
        /// Contacts
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "contacts")]
        public int Contacts { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }

        /// <summary>
        /// Albums
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "albums")]
        public int Albums { get; set; }

        /// <summary>
        /// Channels
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "channels")]
        public int Channels { get; set; }

        /// <summary>
        /// Followers
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "followers")]
        public int Followers { get; set; }

        /// <summary>
        /// Following
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "following")]
        public int Following { get; set; }
    }
}