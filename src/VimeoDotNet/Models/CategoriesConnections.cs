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
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public AlbumConnectionsEntry Videos { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "users")]
        public AlbumConnectionsEntry Users { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "channels")]
        public AlbumConnectionsEntry Channels { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "groups")]
        public AlbumConnectionsEntry Groups { get; set; }
    }
}