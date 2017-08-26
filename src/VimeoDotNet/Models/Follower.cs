using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Follower
    /// </summary>
    [Serializable]
    public class Follower
    {
        /// <summary>
        /// Added time
        /// </summary>
        public DateTime? added_time { get; set; }
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
    }
}