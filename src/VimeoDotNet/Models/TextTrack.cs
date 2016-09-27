using System;
using System.Collections.Generic;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class TextTrack
    {
        public string uri { get; set; }
        public bool active { get; set; }
        public string type { get; set; }
        public string language { get; set; }
        public string link { get; set; }
        public string hls_link { get; set; }
        public string name { get; set; }
    }
}