using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User stats
    /// </summary>
    [Serializable]
    public class UserStats
    {
        /// <summary>
        /// Videos
        /// </summary>
        public int videos { get; set; }
        /// <summary>
        /// Contacts
        /// </summary>
        public int contacts { get; set; }
        /// <summary>
        /// Likes
        /// </summary>
        public int likes { get; set; }
        /// <summary>
        /// Albums
        /// </summary>
        public int albums { get; set; }
        /// <summary>
        /// Channels
        /// </summary>
        public int channels { get; set; }
        /// <summary>
        /// Followers
        /// </summary>
        public int followers { get; set; }
        /// <summary>
        /// Following
        /// </summary>
        public int following { get; set; }
    }
}