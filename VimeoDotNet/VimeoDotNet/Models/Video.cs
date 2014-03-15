using System;
using System.Collections.Generic;
using System.Linq;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Video
    {
        private static readonly IDictionary<string, string> _statusMappings = new Dictionary<string, string>()
        {
            { "uploading_error", "UploadError" }
        };

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
            get { return ModelHelpers.GetEnumValue<VideoStatusEnum>(status, _statusMappings); }
            set { status = ModelHelpers.GetEnumString(value, _statusMappings); }
        }

        public string MobileVideoLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Mobile, false); }
        }
        public string MobileVideoSecureLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Mobile, true); }
        }

        public string StandardVideoLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Standard, false); }
        }
        public string StandardVideoSecureLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Standard, true); }
        }

        public string HighDefinitionVideoLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.HighDefinition, false); }
        }
        public string HighDefinitionVideoSecureLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.HighDefinition, true); }
        }

        public string StreamingVideoLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Streaming, false); }
        }
        public string StreamingVideoSecureLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Streaming, true); }
        }

        private string GetFileQualityUrl(FileQualityEnum quality, bool secureLink)
        {
            if (files == null || files.Count == 0) { return null; }
            var match = files.FirstOrDefault(f => f.FileQuality == quality);
            if (match == null) { return null; }
            return secureLink ? match.link_secure : match.link;
        }
    }
}
