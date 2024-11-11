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
        /// <value><c>true</c> if added; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "added")]
        public bool Added { get; set; }

        /// <summary>
        /// Options
        /// </summary>
        /// <value>The options.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "options")]
        public List<string> Options { get; set; }

        /// <summary>
        /// Added time
        /// </summary>
        /// <value>The added time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "added_time")]
        public DateTime? AddedTime { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}