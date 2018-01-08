using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Picture
    /// </summary>
    public class Picture
    {
        /// <summary>
        /// Active
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Sizes
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "sizes")]
        public List<Size> Sizes { get; set; }

        ///
        /// Link
        ///
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        ///
        /// Resource_key
        ///
        [PublicAPI]
        [JsonProperty(PropertyName = "resource_key")]
        public string ResourceKey { get; set; }
    }
}