using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video metadata
    /// </summary>
    public class VideoMetadata
    {
        /// <summary>
        /// Connections
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public VideoConnections Connections { get; set; }

        /// <summary>
        /// Interactions
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "interactions")]
        public VideoInteractions Interactions { get; set; }
    }
}