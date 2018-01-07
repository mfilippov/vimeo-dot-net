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
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Sizes
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "sizes")]
        public List<Size> Sizes { get; set; }

        /// <summary>
        /// Resources key
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "resource_key")]
        public string ResourceKey { get; set; }
    }
}