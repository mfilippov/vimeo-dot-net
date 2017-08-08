using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Text tracks
    /// </summary>
    [Serializable]
    public class TextTracks
    {
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public List<string> options { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public List<TextTrack> data { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public string total { get; set; }
    }
}