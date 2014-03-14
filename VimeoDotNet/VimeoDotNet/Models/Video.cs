using System;
using System.Collections.Generic;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Video
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
        public string review_link { get; set; }
        public string status { get; set; }
        public EmbedPresets embed_presets { get; set; }
        public string content_rating { get; set; }
        public int duration { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public DateTime created_time { get; set; }
        public DateTime modified_time { get; set; }
        public Privacy privacy { get; set; }
        public List<Picture> pictures { get; set; }
        public List<File> files { get; set; }
        public List<Download> download { get; set; }
        public List<Tag> tags { get; set; }
        public VideoStats stats { get; set; }
        public VideoMetadata metadata { get; set; }

        public VideoStatusEnum VideoStatus
        {
            get { return ModelHelpers.GetEnumValue<VideoStatusEnum>(status); }
            set { status = ModelHelpers.GetEnumString(value); }
        }
    }
}
