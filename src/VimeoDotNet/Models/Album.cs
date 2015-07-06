using System;
using System.Collections.Generic;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Album
    {
        public long? id
        {
            get { return ModelHelpers.ParseModelUriId(uri); }
        }

        public string uri { get; set; }
        public User user { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public int duration { get; set; }
        public string created_time { get; set; }
        public List<Picture> pictures { get; set; }
        public Privacy privacy { get; set; }
        public AlbumStats stats { get; set; }
        public AlbumMetadata metadata { get; set; }
    }
}