using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Text tracks
    /// </summary>
    public class TextTracks
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
        /// Content
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "data")]
        public List<TextTrack> Data { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "total")]
        public string Total { get; set; }
    }
}