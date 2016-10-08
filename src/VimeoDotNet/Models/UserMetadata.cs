using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User  metadata
    /// </summary>
    [Serializable]
    public class UserMetadata
    {
        /// <summary>
        /// Connections
        /// </summary>
        public UserConnections connections { get; set; }
        /// <summary>
        /// Interactions
        /// </summary>
        public UserInteractions interactions { get; set; }
        /// <summary>
        /// Follower
        /// </summary>
        public Follower follower { get; set; }
    }
}