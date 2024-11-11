using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album connections
    /// </summary>
    public class ChannelConnections
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
    }
}