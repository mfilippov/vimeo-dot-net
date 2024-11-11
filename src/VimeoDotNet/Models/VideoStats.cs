
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
        /// <value>The plays.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "plays")]
        public int? Plays { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        /// <value>The likes.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "likes")]
        public int? Likes { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        /// <value>The comments.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "comments")]
        public int? Comments { get; set; }
    }
}