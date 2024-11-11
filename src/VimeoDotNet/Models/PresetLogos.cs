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
        /// <value><c>true</c> if vimeo; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "vimeo")]
        public bool Vimeo { get; set; }

        /// <summary>
        /// Custom
        /// </summary>
        /// <value><c>true</c> if custom; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }

        /// <summary>
        /// Sticky custom
        /// </summary>
        /// <value><c>true</c> if [sticky custom]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "sticky_custom")]
        public bool StickyCustom { get; set; }
    }
}