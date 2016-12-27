using System;
using System.Collections.Generic;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// File model
    /// </summary>
    [Serializable]
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
        public string quality { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// File size
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// Expires
        /// </summary>
        public DateTime? expires { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// LinkSecure
        /// </summary>
        public string link_secure { get; set; }

        /// <summary>
        /// FileQuality
        /// </summary>
        public FileQualityEnum FileQuality
        {
            get { return ModelHelpers.GetEnumValue<FileQualityEnum>(quality, QualityMappings); }
            set { quality = ModelHelpers.GetEnumString(value, QualityMappings); }
        }
    }
}
