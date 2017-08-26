using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album connections
    /// </summary>
    [Serializable]
    public class AlbumConnections
    {
        /// <summary>
        /// Videos
        /// </summary>
        public AlbumConnectionsEntry videos { get; set; }
    }
}