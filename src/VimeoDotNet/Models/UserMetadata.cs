
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User  metadata
    /// </summary>
    public class UserMetadata
    {
        /// <summary>
        /// Connections
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public UserConnections Connections { get; set; }

        /// <summary>
        /// Follower
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "follower")]
        public Follower Follower { get; set; }
    }
}