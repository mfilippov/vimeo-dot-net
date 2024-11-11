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
        /// <value>The buttons.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "buttons")]
        public PresetButtons Buttons { get; set; }

        /// <summary>
        /// Logos
        /// </summary>
        /// <value>The logos.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "logos")]
        public PresetLogos Logos { get; set; }

        /// <summary>
        /// Outro
        /// </summary>
        /// <value>The outro.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "outro")]
        public string Outro { get; set; }

        /// <summary>
        /// Portrait
        /// </summary>
        /// <value>The portrait.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "portrait")]
        public string Portrait { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Byline
        /// </summary>
        /// <value>The byline.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "byline")]
        public string Byline { get; set; }

        /// <summary>
        /// Badge
        /// </summary>
        /// <value><c>true</c> if badge; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "badge")]
        public bool Badge { get; set; }

        /// <summary>
        /// Byline badge
        /// </summary>
        /// <value><c>true</c> if [byline badge]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "byline_badge")]
        public bool BylineBadge { get; set; }

        /// <summary>
        /// Playbar
        /// </summary>
        /// <value><c>true</c> if [play bar]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "playbar")]
        public bool PlayBar { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        /// <value><c>true</c> if volume; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "volume")]
        public bool Volume { get; set; }

        /// <summary>
        /// Fullscreen button
        /// </summary>
        /// <value><c>true</c> if [fullscreen button]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "fullscreen_button")]
        public bool FullscreenButton { get; set; }

        /// <summary>
        /// Scaling button
        /// </summary>
        /// <value><c>true</c> if [scaling button]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "scaling_button")]
        public bool ScalingButton { get; set; }

        /// <summary>
        /// Autoplay
        /// </summary>
        /// <value><c>true</c> if [automatic play]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "autoplay")]
        public bool AutoPlay { get; set; }

        /// <summary>
        /// Autopause
        /// </summary>
        /// <value><c>true</c> if [automatic pause]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "autopause")]
        public bool AutoPause { get; set; }

        /// <summary>
        /// Loop
        /// </summary>
        /// <value><c>true</c> if loop; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "loop")]
        public bool Loop { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        /// <value>The color.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        /// <value><c>true</c> if link; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public bool Link { get; set; }
    }
}