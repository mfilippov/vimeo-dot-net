using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class VideoInteractions
    {
        public Like like { get; set; }
        public WatchLater watchlater { get; set; }
    }
}
