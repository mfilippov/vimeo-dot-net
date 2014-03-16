using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Comment
    {
        public string uri { get; set; }
        public User user { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public DateTime created_time { get; set; }
        public List<Comment> comments { get; set; }
    }
}