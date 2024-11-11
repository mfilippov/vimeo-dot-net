using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class TagMetadataConnections.
    /// </summary>
    public class TagMetadataConnections
    {
        /// <summary>
        /// Video connections
        /// </summary>
        /// <value>The videos.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public VideoConnectionsEntry Videos { get; set; }
    }
}