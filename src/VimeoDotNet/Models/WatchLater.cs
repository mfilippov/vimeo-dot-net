using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Watch later
    /// </summary>
    [Serializable]
    public class WatchLater
    {
        /// <summary>
        /// Added
        /// </summary>
        public bool added { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public List<string> options { get; set; }
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