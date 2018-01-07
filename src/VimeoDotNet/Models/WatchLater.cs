using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Watch later
    /// </summary>
    public class WatchLater
    {
        /// <summary>
        /// Added
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "added")]
        public bool Added { get; set; }

        /// <summary>
        /// Options
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "options")]
        public List<string> Options { get; set; }

        /// <summary>
        /// Added time
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "added_time")]
        public DateTime? AddedTime { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}