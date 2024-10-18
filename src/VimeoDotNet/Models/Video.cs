using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video
    /// </summary>
    // https://developer.vimeo.com/api/reference/response/video
    public class Video
    {
        private static readonly IDictionary<string, string> StatusMappings = new Dictionary<string, string>
        {
            {"uploading_error", "UploadError"}
        };

        /// <summary>
        /// Id
        /// </summary>
        public long? Id => ModelHelpers.ParseModelUriId(Uri);

        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "description_html")]
        public string DescriptionHtml { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "description_rich")]
        public string DescriptionRich { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// The video's player embed URL
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "player_embed_url")]
        public string Player_Embed_Url { get; set; }

        /// <summary>
        /// Review link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "review_link")]
        public string ReviewLink { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Embed presets
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed_presets")]
        public EmbedPresets EmbedPresets { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Modified time
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "modified_time")]
        public DateTime ModifiedTime { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Files
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "files")]
        public List<File> Files { get; set; }

        /// <summary>
        /// Download
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "download")]
        public List<Download> Download { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "tags")]
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// Stats
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "stats")]
        public VideoStats Stats { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public VideoMetadata Metadata { get; set; }

        /// <summary>
        /// Embed
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed")]
        public Embed Embed { get; set; }

        /// <summary>
        /// The video's 360 spatial data
        /// </summary>
        [PublicAPI]
        [CanBeNull]
        [JsonProperty(PropertyName = "spatial")]
        public Spatial Spatial { get; set; }

        /// <summary>
        /// Video status
        /// </summary>
        [PublicAPI]
        public VideoStatusEnum VideoStatus
        {
            get => ModelHelpers.GetEnumValue<VideoStatusEnum>(Status, StatusMappings);
            set => Status = ModelHelpers.GetEnumString(value, StatusMappings);
        }



        /// <summary>
        /// Mobile video link
        /// </summary>
        [PublicAPI]
        public string MobileVideoLink => GetFileQualityUrl(FileQualityEnum.Mobile, false);

        /// <summary>
        /// Mobile video secure link
        /// </summary>
        [PublicAPI]
        public string MobileVideoSecureLink => GetFileQualityUrl(FileQualityEnum.Mobile, true);

        /// <summary>
        /// Standard video link
        /// </summary>
        [PublicAPI]
        public string StandardVideoLink => GetFileQualityUrl(FileQualityEnum.Standard, false);

        /// <summary>
        /// Standard video secure link
        /// </summary>
        [PublicAPI]
        public string StandardVideoSecureLink => GetFileQualityUrl(FileQualityEnum.Standard, true);

        /// <summary>
        /// High definition video link
        /// </summary>
        [PublicAPI]
        public string HighDefinitionVideoLink => GetFileQualityUrl(FileQualityEnum.HighDefinition, false);

        /// <summary>
        /// High definition video secure link
        /// </summary>
        [PublicAPI]
        public string HighDefinitionVideoSecureLink => GetFileQualityUrl(FileQualityEnum.HighDefinition, true);

        /// <summary>
        /// Streaming video link
        /// </summary>
        [PublicAPI]
        public string StreamingVideoLink => GetFileQualityUrl(FileQualityEnum.Streaming, false);

        /// <summary>
        /// Streaming video secure link
        /// </summary>
        [PublicAPI]
        public string StreamingVideoSecureLink => GetFileQualityUrl(FileQualityEnum.Streaming, true);

        private string GetFileQualityUrl(FileQualityEnum quality, bool secureLink)
        {
            if (Files == null || Files.Count == 0)
            {
                return null;
            }

            var match = Files.FirstOrDefault(f => f.FileQuality == quality);
            if (match == null)
            {
                return null;
            }

            return secureLink ? match.LinkSecure : match.Link;
        }

        [PublicAPI]
        [CanBeNull]
        [JsonProperty(PropertyName = "transcode")]
        public Transcode Transcode { get; set; }

        /// <summary>
        /// Embed
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "is_playable")]
        public bool IsPlayable { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "is_free")]
        public bool IsFree { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "can_move_to_project")]
        public bool CanMoveToProject { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "has_audio")]
        public bool HasAudio { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "has_audio_tracks")]
        public bool HasAudioTracks { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "has_chapters")]
        public bool HasChapters { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "has_interactive")]
        public bool HasInteractive { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "has_viewer_home_page_settings")]
        public bool HasViewerHomePageSettings { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "content_rating")]
        public List<string> ContentRating { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "content_rating_class")]
        public string ContentRatingClass { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "context")]
        public Context Context { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "is_copyright_restricted")]
        public bool IsCopyrightRestricted { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "language")]
        [CanBeNull]
        public string Language { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "has_text_tracks")]
        public bool HasTextTracks { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "origin_variable_frame_resolution")]
        public bool OriginVariableFrameResolution { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "last_user_action_event_date")]
        public DateTime? LastUserActionEventDate { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "allowed_privacies")]
        [CanBeNull]
        public List<string> AllowedPrivacies { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "custom_url")]
        [CanBeNull]
        public string CustomUrl { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "manage_link")]
        public string ManageLink { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "page")]
        public VideoPageSettings Page { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "parent_folder")]
        public Project ParentFolder { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        //edit_session, files_size,
    }
}