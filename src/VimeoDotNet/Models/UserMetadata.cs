
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
        /// <value>The connections.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public UserConnections Connections { get; set; }

        /// <summary>
        /// Interactions
        /// </summary>
        /// <value>The interactions.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "interactions")]
        public UserInteractions Interactions { get; set; }

        /// <summary>
        /// Follower
        /// </summary>
        /// <value>The follower.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "follower")]
        public Follower Follower { get; set; }
    }
}