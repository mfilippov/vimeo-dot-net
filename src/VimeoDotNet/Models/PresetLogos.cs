using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Preset logos
    /// </summary>
    [Serializable]
    public class PresetLogos
    {
        /// <summary>
        /// Vimeo
        /// </summary>
        public bool vimeo { get; set; }
        /// <summary>
        /// Custom
        /// </summary>
        public bool custom { get; set; }
        /// <summary>
        /// Sticky custom
        /// </summary>
        public bool sticky_custom { get; set; }
    }
}