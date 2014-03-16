using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class VideoMetadata
    {
        public VideoConnections connections { get; set; }
        public VideoInteractions interactions { get; set; }
    }
}