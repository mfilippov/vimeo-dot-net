using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album connections entry
    /// </summary>
    public class ChannelInteractionsEntry
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
    }
}