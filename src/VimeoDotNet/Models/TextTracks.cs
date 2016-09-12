using System;
using System.Collections.Generic;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class TextTracks
    {
        public string uri { get; set; }
        public List<string> options { get; set; }
        public List<TextTrack> data { get; set; }
        public string total { get; set; }
    }
}