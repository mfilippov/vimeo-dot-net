using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album metadata
    /// </summary>
    [Serializable]
    public class AlbumMetadata
    {
        /// <summary>
        /// Connections
        /// </summary>
        public AlbumConnections connections { get; set; }
    }
}