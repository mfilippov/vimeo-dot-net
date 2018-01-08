using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Preset settings
    /// </summary>
    public class PresetSettings
    {
        /// <summary>
        /// Buttons
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "buttons")]
        public PresetButtons Buttons { get; set; }

        /// <summary>
        /// Logos
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "logos")]
        public PresetLogos Logos { get; set; }

        /// <summary>
        /// Outro
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "outro")]
        public string Outro { get; set; }

        /// <summary>
        /// Portrait
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "portrait")]
        public string Portrait { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Byline
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "byline")]
        public string Byline { get; set; }

        /// <summary>
        /// Badge
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "badge")]
        public bool Badge { get; set; }

        /// <summary>
        /// Byline badge
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "byline_badge")]
        public bool BylineBadge { get; set; }

        /// <summary>
        /// Playbar
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "playbar")]
        public bool PlayBar { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "volume")]
        public bool Volume { get; set; }

        /// <summary>
        /// Fullscreen button
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "fullscreen_button")]
        public bool FullscreenButton { get; set; }

        /// <summary>
        /// Scaling button
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "scaling_button")]
        public bool ScalingButton { get; set; }

        /// <summary>
        /// Autoplay
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "autoplay")]
        public bool AutoPlay { get; set; }

        /// <summary>
        /// Autopause
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "autopause")]
        public bool AutoPause { get; set; }

        /// <summary>
        /// Loop
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "loop")]
        public bool Loop { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public bool Link { get; set; }
    }
}