using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Domain for embedding a video
    /// </summary>
    public class DomainForEmbedding
    {
        /// <summary>
        /// Domain name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Whether HD quality is allowed
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "allow_hd")]
        public bool AllowHighDefinition { get; set; }

        /// <summary>
        /// URI of this resource
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}
