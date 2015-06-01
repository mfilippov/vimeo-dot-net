using System;
using System.Collections.Generic;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class File
    {
        private static readonly IDictionary<string, string> _qualityMappings = new Dictionary<string, string>
        {
            {"mobile", "Mobile"},
            {"hd", "HighDefinition"},
            {"sd", "Standard"},
            {"hls", "Streaming"}
        };

        public string quality { get; set; }
        public string type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public DateTime? expires { get; set; }
        public string link { get; set; }
        public string link_secure { get; set; }

        public FileQualityEnum FileQuality
        {
            get { return ModelHelpers.GetEnumValue<FileQualityEnum>(quality, _qualityMappings); }
            set { quality = ModelHelpers.GetEnumString(value, _qualityMappings); }
        }
    }
}