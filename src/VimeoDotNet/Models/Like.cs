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
        /// <value><c>true</c> if added; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "added")]
        public bool Added { get; set; }

        /// <summary>
        /// Added time
        /// </summary>
        /// <value>The added time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "added_time")]
        public DateTime AddedTime { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}