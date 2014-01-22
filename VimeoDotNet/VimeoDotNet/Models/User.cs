using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    public class User
    {
        public string uri { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string location { get; set; }
        public string bio { get; set; }
        public DateTime created_time { get; set; }
        public string account { get; set; }
        public string content_filter { get; set; }
        public List<Picture> pictures { get; set; }
        public List<Website> websites { get; set; }
        public UserStats stats { get; set; }
        public UserMetadata metadata { get; set; }
        public UserUploadQuota upload_quota { get; set; }
    }
}
