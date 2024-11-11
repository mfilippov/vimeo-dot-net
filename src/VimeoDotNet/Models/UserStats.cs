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
        /// <value>The videos.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public int Videos { get; set; }

        /// <summary>
        /// Contacts
        /// </summary>
        /// <value>The contacts.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "contacts")]
        public int Contacts { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        /// <value>The likes.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }

        /// <summary>
        /// Albums
        /// </summary>
        /// <value>The albums.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "albums")]
        public int Albums { get; set; }

        /// <summary>
        /// Channels
        /// </summary>
        /// <value>The channels.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "channels")]
        public int Channels { get; set; }

        /// <summary>
        /// Followers
        /// </summary>
        /// <value>The followers.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "followers")]
        public int Followers { get; set; }

        /// <summary>
        /// Following
        /// </summary>
        /// <value>The following.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "following")]
        public int Following { get; set; }
    }
}