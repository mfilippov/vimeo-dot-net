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
        [PublicAPI]
        [JsonProperty(PropertyName = "tag")]
        public string Id { get; set; }


        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Canonical
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "canonical")]
        public string Canonical { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public TagMetadata Metadata { get; set; }
    }
}