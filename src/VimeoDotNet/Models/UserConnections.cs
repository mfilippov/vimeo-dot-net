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
        /// <value>The activities.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "activities")]
        public UserConnectionsEntry Activities { get; set; }

        /// <summary>
        /// Albums
        /// </summary>
        /// <value>The albums.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "albums")]
        public UserConnectionsEntry Albums { get; set; }

        /// <summary>
        /// Channels
        /// </summary>
        /// <value>The channels.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "channels")]
        public UserConnectionsEntry Channels { get; set; }

        /// <summary>
        /// Feed
        /// </summary>
        /// <value>The feed.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "feed")]
        public UserConnectionsEntry Feed { get; set; }

        /// <summary>
        /// Followers
        /// </summary>
        /// <value>The followers.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "followers")]
        public UserConnectionsEntry Followers { get; set; }

        /// <summary>
        /// Following
        /// </summary>
        /// <value>The following.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "following")]
        public UserConnectionsEntry Following { get; set; }

        /// <summary>
        /// Groups
        /// </summary>
        /// <value>The groups.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "groups")]
        public UserConnectionsEntry Groups { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        /// <value>The likes.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "likes")]
        public UserConnectionsEntry Likes { get; set; }

        /// <summary>
        /// Portfolios
        /// </summary>
        /// <value>The portfolios.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "portfolios")]
        public UserConnectionsEntry Portfolios { get; set; }

        /// <summary>
        /// Videos
        /// </summary>
        /// <value>The videos.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public UserConnectionsEntry Videos { get; set; }

        /// <summary>
        /// Watch later
        /// </summary>
        /// <value>The watchlater.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "watchlater")]
        public UserConnectionsEntry Watchlater { get; set; }
    }
}