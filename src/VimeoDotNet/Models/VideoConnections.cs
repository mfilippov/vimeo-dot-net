using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video connections
    /// </summary>
    public class VideoConnections
    {
        /// <summary>
        /// Comments
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "comments")]
        public VideoConnectionsEntry Comments { get; set; }

        /// <summary>
        /// Credits
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "credits")]
        public VideoConnectionsEntry Credits { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "likes")]
        public VideoConnectionsEntry Likes { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public VideoConnectionsEntry Pictures { get; set; }

        /// <summary>
        /// Text tracks
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "texttracks")]
        public TextTracks Texttracks { get; set; }
    }
}