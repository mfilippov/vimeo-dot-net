using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Like
    /// </summary>
    public class Like
    {
        /// <summary>
        /// Added
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "added")]
        public bool Added { get; set; }

        /// <summary>
        /// Added time
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "added_time")]
        public DateTime AddedTime { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}