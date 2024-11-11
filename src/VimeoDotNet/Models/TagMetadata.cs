using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class TagMetadata.
    /// </summary>
    public class TagMetadata
    {
        /// <summary>
        /// Tag connections
        /// </summary>
        /// <value>The connections.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public TagMetadataConnections Connections { get; set; }
    }
}