using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Follow
    {
        public bool added { get; set; }
        public DateTime added_time { get; set; }
        public string uri { get; set; }
    }
}
