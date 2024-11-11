using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User connection
    /// </summary>
    public class UserConnectionsEntry
    {
        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Options
        /// </summary>
        /// <value>The options.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "options")]
        public List<string> Options { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        /// <value>The total.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }
}