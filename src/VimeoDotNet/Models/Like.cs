using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Like
    /// </summary>
    [Serializable]
    public class Like
    {
        /// <summary>
        /// Added
        /// </summary>
        public bool added { get; set; }
        /// <summary>
        /// Added time
        /// </summary>
        public DateTime added_time { get; set; }
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
    }
}