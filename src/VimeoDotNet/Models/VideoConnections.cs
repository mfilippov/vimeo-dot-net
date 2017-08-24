using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video connections
    /// </summary>
    [Serializable]
    public class VideoConnections
    {
        /// <summary>
        /// Comments
        /// </summary>
        public VideoConnectionsEntry comments { get; set; }
        /// <summary>
        /// Credits
        /// </summary>
        public VideoConnectionsEntry credits { get; set; }
        /// <summary>
        /// Likes
        /// </summary>
        public VideoConnectionsEntry likes { get; set; }
        /// <summary>
        /// Presets
        /// </summary>
        public VideoConnectionsEntry puctires { get; set; }
        /// <summary>
        /// Text tracks
        /// </summary>
        public TextTracks texttracks { get; set; }
    }

    /// <summary>
    /// Video connections entry
    /// </summary>
    public class VideoConnectionsEntry
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
        /// Total
        /// </summary>
        public int total { get; set; }
    }
}