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
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Options
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "options")]
        public List<string> Options { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }
}