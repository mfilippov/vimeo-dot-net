using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class UserStats
    {
        public int videos { get; set; }
        public int contacts { get; set; }
        public int likes { get; set; }
        public int albums { get; set; }
        public int channels { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
    }
}