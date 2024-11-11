using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album connections
    /// </summary>
    public class CategoriesConnections
    {
        /// <summary>
        /// Videos
        /// </summary>
        /// <value>The videos.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public AlbumConnectionsEntry Videos { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "users")]
        public AlbumConnectionsEntry Users { get; set; }

        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        /// <value>The channels.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "channels")]
        public AlbumConnectionsEntry Channels { get; set; }

        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        /// <value>The groups.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "groups")]
        public AlbumConnectionsEntry Groups { get; set; }
    }
}