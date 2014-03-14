using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Follower
    {
        public DateTime added_time { get; set; }
        public string uri { get; set; }
    }
}
