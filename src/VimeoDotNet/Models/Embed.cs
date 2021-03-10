using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Embed
    /// </summary>
    public class Embed
    {
        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Html
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }
    }
}
