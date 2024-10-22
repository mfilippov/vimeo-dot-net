using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// File model
    /// </summary>
    public class File
    {
        private static readonly IDictionary<string, string> QualityMappings = new Dictionary<string, string>
        {
            {"mobile", "Mobile"},
            {"hd", "HighDefinition"},
            {"sd", "Standard"},
            {"hls", "Streaming"}
        };

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

        /// <summary>
        /// FileQuality
        /// </summary>
        [PublicAPI]
        public FileQualityEnum FileQuality
        {
            get => ModelHelpers.GetEnumValue<FileQualityEnum>(Quality, QualityMappings);
            set => Quality = ModelHelpers.GetEnumString(value, QualityMappings);
        }
    }
}