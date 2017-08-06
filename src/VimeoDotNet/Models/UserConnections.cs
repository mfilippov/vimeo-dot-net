using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User connections
    /// </summary>
    [Serializable]
    public class UserConnections
    {
        /// <summary>
        /// Activities
        /// </summary>
        public UserConnection activities { get; set; }
        /// <summary>
        /// Albums
        /// </summary>
        public UserConnection albums { get; set; }
        /// <summary>
        /// Channels
        /// </summary>
        public UserConnection channels { get; set; }
        /// <summary>
        /// Feed
        /// </summary>
        public UserConnection feed { get; set; }
        /// <summary>
        /// Followers
        /// </summary>
        public UserConnection followers { get; set; }
        /// <summary>
        /// Following
        /// </summary>
        public UserConnection following { get; set; }
        /// <summary>
        /// Groups
        /// </summary>
        public UserConnection groups { get; set; }
        /// <summary>
        /// Likes
        /// </summary>
        public UserConnection likes { get; set; }
        /// <summary>
        /// Portfolios
        /// </summary>
        public UserConnection portfolios { get; set; }
        /// <summary>
        /// Videos
        /// </summary>
        public UserConnection videos { get; set; }
        /// <summary>
        /// Watch later
        /// </summary>
        public UserConnection watchlater { get; set; }
    }

    /// <summary>
    /// User connection
    /// </summary>
    public class UserConnection
    {
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
        
        /// <summary>
        /// Options
        /// </summary>
        public string[] options { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public int total { get; set; }
    }
}