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
        /// <value>The domain.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Whether HD quality is allowed
        /// </summary>
        /// <value><c>true</c> if [allow high definition]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "allow_hd")]
        public bool AllowHighDefinition { get; set; }

        /// <summary>
        /// URI of this resource
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}
