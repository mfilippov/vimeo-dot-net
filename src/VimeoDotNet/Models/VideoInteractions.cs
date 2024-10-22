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
        /// <value>The watch later.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "watchlater")]
        public WatchLater WatchLater { get; set; }

        /// <summary>
        /// Gets or sets the rent.
        /// </summary>
        /// <value>The rent.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "rent")]
        public Rent Rent { get; set; }
    }
}