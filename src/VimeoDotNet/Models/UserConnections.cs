using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User connections entry
    /// </summary>
    public class UserConnections
    {
        /// <summary>
        /// Activities
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "activities")]
        public UserConnectionsEntry Activities { get; set; }

        /// <summary>
        /// Albums
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "albums")]
        public UserConnectionsEntry Albums { get; set; }

        /// <summary>
        /// Channels
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "channels")]
        public UserConnectionsEntry Channels { get; set; }

        /// <summary>
        /// Feed
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "feed")]
        public UserConnectionsEntry Feed { get; set; }

        /// <summary>
        /// Followers
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "followers")]
        public UserConnectionsEntry Followers { get; set; }

        /// <summary>
        /// Following
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "following")]
        public UserConnectionsEntry Following { get; set; }

        /// <summary>
        /// Groups
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "groups")]
        public UserConnectionsEntry Groups { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "likes")]
        public UserConnectionsEntry Likes { get; set; }

        /// <summary>
        /// Portfolios
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "portfolios")]
        public UserConnectionsEntry Portfolios { get; set; }

        /// <summary>
        /// Videos
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public UserConnectionsEntry Videos { get; set; }

        /// <summary>
        /// Watch later
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "watchlater")]
        public UserConnectionsEntry Watchlater { get; set; }
    }
}