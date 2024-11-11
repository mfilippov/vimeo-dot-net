using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User pictures
    /// </summary>
    public class Pictures
    {
        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        /// <value>The type.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Sizes
        /// </summary>
        /// <value>The sizes.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "sizes")]
        public List<Size> Sizes { get; set; }

        /// <summary>
        /// Resources key
        /// </summary>
        /// <value>The resource key.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "resource_key")]
        public string ResourceKey { get; set; }

        /// <summary>
        /// The upload URL for the picture. This field appears when you create the picture resource for the first time.
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "base_link")]
        public string Link { get; set; }
    }
}
