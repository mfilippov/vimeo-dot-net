using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Tag
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value>The identifier.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "tag")]
        public string Id { get; set; }


        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Canonical
        /// </summary>
        /// <value>The canonical.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "canonical")]
        public string Canonical { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        /// <value>The metadata.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public TagMetadata Metadata { get; set; }
    }
}