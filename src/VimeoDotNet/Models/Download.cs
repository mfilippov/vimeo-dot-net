using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Download model
    /// </summary>
    public class Download
    {
        /// <summary>
        /// Quality
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "quality")]
        public string Quality { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        /// <summary>
        /// File size
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "size")]
        public long Size { get; set; }

        /// <summary>
        /// Expires
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "expires")]
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}