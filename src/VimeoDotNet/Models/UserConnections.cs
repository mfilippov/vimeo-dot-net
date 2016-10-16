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
        public string activities { get; set; }
        /// <summary>
        /// Albums
        /// </summary>
        public string albums { get; set; }
        /// <summary>
        /// Channels
        /// </summary>
        public string channels { get; set; }
        /// <summary>
        /// Feed
        /// </summary>
        public string feed { get; set; }
        /// <summary>
        /// Followers
        /// </summary>
        public string followers { get; set; }
        /// <summary>
        /// Following
        /// </summary>
        public string following { get; set; }
        /// <summary>
        /// Groups
        /// </summary>
        public string groups { get; set; }
        /// <summary>
        /// Likes
        /// </summary>
        public string likes { get; set; }
        /// <summary>
        /// Portfolios
        /// </summary>
        public string portfolios { get; set; }
        /// <summary>
        /// Videos
        /// </summary>
        public string videos { get; set; }
        /// <summary>
        /// Watch later
        /// </summary>
        public string watchlater { get; set; }
    }
}