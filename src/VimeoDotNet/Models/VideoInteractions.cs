using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class VideoInteractions
    {
        /// <summary>
        /// Like
        /// </summary>
        public Like like { get; set; }
        /// <summary>
        /// Watch later
        /// </summary>
        public WatchLater watchlater { get; set; }
    }
}