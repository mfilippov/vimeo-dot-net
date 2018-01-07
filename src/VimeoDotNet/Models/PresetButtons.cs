using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Preset buttons
    /// </summary>
    public class PresetButtons
    {
        /// <summary>
        /// Like
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "like")]
        public bool Like { get; set; }

        /// <summary>
        /// Watch later
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "watchlater")]
        public bool WatchLater { get; set; }

        /// <summary>
        /// Share
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "share")]
        public bool Share { get; set; }

        /// <summary>
        /// Embed
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed")]
        public bool Embed { get; set; }

        /// <summary>
        /// Vote
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "vote")]
        public bool Vote { get; set; }

        /// <summary>
        /// HD
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "hd")]
        public bool Hd { get; set; }
    }
}