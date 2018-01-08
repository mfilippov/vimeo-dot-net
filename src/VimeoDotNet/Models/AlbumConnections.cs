using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album connections
    /// </summary>
    public class AlbumConnections
    {
        /// <summary>
        /// Videos
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public AlbumConnectionsEntry Videos { get; set; }
    }
}