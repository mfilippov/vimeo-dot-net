using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Preset logos
    /// </summary>
    public class PresetLogos
    {
        /// <summary>
        /// Vimeo
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "vimeo")]
        public bool Vimeo { get; set; }

        /// <summary>
        /// Custom
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }

        /// <summary>
        /// Sticky custom
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "sticky_custom")]
        public bool StickyCustom { get; set; }
    }
}