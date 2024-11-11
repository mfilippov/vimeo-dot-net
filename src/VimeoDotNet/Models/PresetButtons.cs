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
        /// <value><c>true</c> if like; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "like")]
        public bool Like { get; set; }

        /// <summary>
        /// Watch later
        /// </summary>
        /// <value><c>true</c> if [watch later]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "watchlater")]
        public bool WatchLater { get; set; }

        /// <summary>
        /// Share
        /// </summary>
        /// <value><c>true</c> if share; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "share")]
        public bool Share { get; set; }

        /// <summary>
        /// Embed
        /// </summary>
        /// <value><c>true</c> if embed; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed")]
        public bool Embed { get; set; }

        /// <summary>
        /// Vote
        /// </summary>
        /// <value><c>true</c> if vote; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "vote")]
        public bool Vote { get; set; }

        /// <summary>
        /// HD
        /// </summary>
        /// <value><c>true</c> if hd; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "hd")]
        public bool Hd { get; set; }
    }
}