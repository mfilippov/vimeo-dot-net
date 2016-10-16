using System;

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
        public string comments { get; set; }
        /// <summary>
        /// Credits
        /// </summary>
        public string credits { get; set; }
        /// <summary>
        /// Files
        /// </summary>
        public string files { get; set; }
        /// <summary>
        /// Likes
        /// </summary>
        public string likes { get; set; }
        /// <summary>
        /// Presets
        /// </summary>
        public string presets { get; set; }
        /// <summary>
        /// Text tracks
        /// </summary>
        public TextTracks texttracks { get; set; }
        /// <summary>
        /// Upload tickets
        /// </summary>
        public string upload_tickets { get; set; }
    }
}