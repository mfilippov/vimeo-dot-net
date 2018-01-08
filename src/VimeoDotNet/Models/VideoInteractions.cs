using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video interactions
    /// </summary>
    public class VideoInteractions
    {
        /// <summary>
        /// Watch later
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "watchlater")]
        public WatchLater WatchLater { get; set; }
    }
}