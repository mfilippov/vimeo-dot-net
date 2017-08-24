using System;
using System.Collections.Generic;
using System.Linq;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video
    /// </summary>
    [Serializable]
    public class Video
    {
        private static readonly IDictionary<string, string> _statusMappings = new Dictionary<string, string>
        {
            {"uploading_error", "UploadError"}
        };

        /// <summary>
        /// Id
        /// </summary>
        public long? id
        {
            get { return ModelHelpers.ParseModelUriId(uri); }
        }

        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
        /// <summary>
        /// User
        /// </summary>
        public User user { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// Review link
        /// </summary>
        public string review_link { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// Embed presets
        /// </summary>
        public EmbedPresets embed_presets { get; set; }
        /// <summary>
        /// Duration
        /// </summary>
        public int duration { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// Created time
        /// </summary>
        public DateTime created_time { get; set; }
        /// <summary>
        /// Modified time
        /// </summary>
        public DateTime modified_time { get; set; }
        /// <summary>
        /// Privacy
        /// </summary>
        public Privacy privacy { get; set; }
        /// <summary>
        /// Pictures
        /// </summary>
        public Pictures pictures { get; set; }
        /// <summary>
        /// Files
        /// </summary>
        public List<File> files { get; set; }
        /// <summary>
        /// Download
        /// </summary>
        public List<Download> download { get; set; }
        /// <summary>
        /// Tags
        /// </summary>
        public List<Tag> tags { get; set; }
        /// <summary>
        /// Stats
        /// </summary>
        public VideoStats stats { get; set; }
        /// <summary>
        /// Metadata
        /// </summary>
        public VideoMetadata metadata { get; set; }

        /// <summary>
        /// Video status
        /// </summary>
        public VideoStatusEnum VideoStatus
        {
            get { return ModelHelpers.GetEnumValue<VideoStatusEnum>(status, _statusMappings); }
            set { status = ModelHelpers.GetEnumString(value, _statusMappings); }
        }

        /// <summary>
        /// Mobile video link
        /// </summary>
        public string MobileVideoLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Mobile, false); }
        }

        /// <summary>
        /// Mobile video secure link
        /// </summary>
        public string MobileVideoSecureLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Mobile, true); }
        }

        /// <summary>
        /// Standard video link
        /// </summary>
        public string StandardVideoLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Standard, false); }
        }

        /// <summary>
        /// Standard video secure link
        /// </summary>
        public string StandardVideoSecureLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Standard, true); }
        }

        /// <summary>
        /// High definition video link
        /// </summary>
        public string HighDefinitionVideoLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.HighDefinition, false); }
        }

        /// <summary>
        /// High definition video secure link
        /// </summary>
        public string HighDefinitionVideoSecureLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.HighDefinition, true); }
        }

        /// <summary>
        /// Streaming video link
        /// </summary>
        public string StreamingVideoLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Streaming, false); }
        }

        /// <summary>
        /// Streaming video secure link
        /// </summary>
        public string StreamingVideoSecureLink
        {
            get { return GetFileQualityUrl(FileQualityEnum.Streaming, true); }
        }

        private string GetFileQualityUrl(FileQualityEnum quality, bool secureLink)
        {
            if (files == null || files.Count == 0)
            {
                return null;
            }
            File match = files.FirstOrDefault(f => f.FileQuality == quality);
            if (match == null)
            {
                return null;
            }
            return secureLink ? match.link_secure : match.link;
        }
    }
}