using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video metadata
    /// </summary>
    [Serializable]
    public class VideoMetadata
    {
        /// <summary>
        /// Connections
        /// </summary>
        public VideoConnections connections { get; set; }
        /// <summary>
        /// Interactions
        /// </summary>
        public VideoInteractions interactions { get; set; }
    }
}