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
        /// <value>The quality.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "quality")]
        public string Quality { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        /// <value>The type.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        /// <value>The width.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        /// <value>The height.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        /// <summary>
        /// File size
        /// </summary>
        /// <value>The size.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "size")]
        public long Size { get; set; }

        /// <summary>
        /// Expires
        /// </summary>
        /// <value>The expires.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "expires")]
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}