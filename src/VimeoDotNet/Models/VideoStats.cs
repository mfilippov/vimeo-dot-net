using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video stats
    /// </summary>
    [Serializable]
    public class VideoStats
    {
        /// <summary>
        /// Plays
        /// </summary>
        public int? plays { get; set; }
        /// <summary>
        /// Likes
        /// </summary>
        public int? likes { get; set; }
        /// <summary>
        /// Comments
        /// </summary>
        public int? comments { get; set; }
    }
}