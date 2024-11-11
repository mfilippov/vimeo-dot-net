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
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Html
        /// </summary>
        /// <value>The HTML.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }
    }
}
