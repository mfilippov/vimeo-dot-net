using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Follower
    /// </summary>
    [Serializable]
    public class Follower
    {
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