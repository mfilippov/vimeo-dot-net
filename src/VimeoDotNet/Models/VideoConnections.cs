using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class VideoConnections
    {
        public string comments { get; set; }
        public string credits { get; set; }
        public string files { get; set; }
        public string likes { get; set; }
        public string presets { get; set; }
        public string upload_tickets { get; set; }
    }
}