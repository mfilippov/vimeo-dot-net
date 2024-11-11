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
        /// <summary>
        /// The quality mappings
        /// </summary>
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

        /// <summary>
        /// LinkSecure
        /// </summary>
        /// <value>The link secure.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link_secure")]
        public string LinkSecure { get; set; }

        /// <summary>
        /// FileQuality
        /// </summary>
        /// <value>The file quality.</value>
        [PublicAPI]
        public FileQualityEnum FileQuality
        {
            get => ModelHelpers.GetEnumValue<FileQualityEnum>(Quality, QualityMappings);
            set => Quality = ModelHelpers.GetEnumString(value, QualityMappings);
        }
    }
}