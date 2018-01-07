
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video stats
    /// </summary>
    public class VideoStats
    {
        /// <summary>
        /// Plays
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "plays")]
        public int? Plays { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "likes")]
        public int? Likes { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "comments")]
        public int? Comments { get; set; }
    }
}