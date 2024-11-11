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
        /// <value>The comments.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "comments")]
        public VideoConnectionsEntry Comments { get; set; }

        /// <summary>
        /// Credits
        /// </summary>
        /// <value>The credits.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "credits")]
        public VideoConnectionsEntry Credits { get; set; }

        /// <summary>
        /// Likes
        /// </summary>
        /// <value>The likes.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "likes")]
        public VideoConnectionsEntry Likes { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        /// <value>The pictures.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public VideoConnectionsEntry Pictures { get; set; }

        /// <summary>
        /// Text tracks
        /// </summary>
        /// <value>The text tracks.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "texttracks")]
        public TextTracks TextTracks { get; set; }
    }
}