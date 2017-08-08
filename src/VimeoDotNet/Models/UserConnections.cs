using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User connections entry
    /// </summary>
    [Serializable]
    public class UserConnections
    {
        /// <summary>
        /// Activities
        /// </summary>
        public UserConnectionsEntry activities { get; set; }
        /// <summary>
        /// Albums
        /// </summary>
        public UserConnectionsEntry albums { get; set; }
        /// <summary>
        /// Channels
        /// </summary>
        public UserConnectionsEntry channels { get; set; }
        /// <summary>
        /// Feed
        /// </summary>
        public UserConnectionsEntry feed { get; set; }
        /// <summary>
        /// Followers
        /// </summary>
        public UserConnectionsEntry followers { get; set; }
        /// <summary>
        /// Following
        /// </summary>
        public UserConnectionsEntry following { get; set; }
        /// <summary>
        /// Groups
        /// </summary>
        public UserConnectionsEntry groups { get; set; }
        /// <summary>
        /// Likes
        /// </summary>
        public UserConnectionsEntry likes { get; set; }
        /// <summary>
        /// Portfolios
        /// </summary>
        public UserConnectionsEntry portfolios { get; set; }
        /// <summary>
        /// Videos
        /// </summary>
        public UserConnectionsEntry videos { get; set; }
        /// <summary>
        /// Watch later
        /// </summary>
        public UserConnectionsEntry watchlater { get; set; }
    }
}