using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Space
    {
        public long max { get; set; }
        public long free { get; set; }
        public long used { get; set; }
    }
}
