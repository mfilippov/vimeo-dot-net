using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Preset settings
    /// </summary>
    [Serializable]
    public class PresetSettings
    {
        /// <summary>
        /// Buttons
        /// </summary>
        public PresetButtons buttons { get; set; }
        /// <summary>
        /// Logos
        /// </summary>
        public PresetLogos logos { get; set; }
        /// <summary>
        /// Outro
        /// </summary>
        public string outro { get; set; }
        /// <summary>
        /// Portrait
        /// </summary>
        public string portrait { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// Byline
        /// </summary>
        public string byline { get; set; }
        /// <summary>
        /// Badge
        /// </summary>
        public bool badge { get; set; }
        /// <summary>
        /// Byline badge
        /// </summary>
        public bool byline_badge { get; set; }
        /// <summary>
        /// Playbar
        /// </summary>
        public bool playbar { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        public bool volume { get; set; }
        /// <summary>
        /// Fullscreen button
        /// </summary>
        public bool fullscreen_button { get; set; }
        /// <summary>
        /// Scaling button
        /// </summary>
        public bool scaling_button { get; set; }
        /// <summary>
        /// Autoplay
        /// </summary>
        public bool autoplay { get; set; }
        /// <summary>
        /// Autopause
        /// </summary>
        public bool autopause { get; set; }
        /// <summary>
        /// Loop
        /// </summary>
        public bool loop { get; set; }
        /// <summary>
        /// Color
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// Link
        /// </summary>
        public bool link { get; set; }
    }
}