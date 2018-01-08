using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    public class TagMetadataConnections
    {
        /// <summary>
        /// Video connections
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "videos")]
        public VideoConnectionsEntry Videos { get; set; }
    }
}