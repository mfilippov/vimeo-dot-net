using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    public class TagMetadata
    {
        /// <summary>
        /// Tag connections
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public TagMetadataConnections Connections { get; set; }
    }
}