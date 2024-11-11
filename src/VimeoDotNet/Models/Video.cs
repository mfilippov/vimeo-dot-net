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
        /// <summary>
        /// The status mappings
        /// </summary>
        private static readonly IDictionary<string, string> StatusMappings = new Dictionary<string, string>
        {
            {"uploading_error", "UploadError"}
        };

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The identifier.</value>
        public long? Id => ModelHelpers.ParseModelUriId(Uri);

        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// User
        /// </summary>
        /// <value>The user.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        /// <value>The description.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the description HTML.
        /// </summary>
        /// <value>The description HTML.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "description_html")]
        public string DescriptionHtml { get; set; }

        /// <summary>
        /// Gets or sets the description rich.
        /// </summary>
        /// <value>The description rich.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "description_rich")]
        public string DescriptionRich { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// The video's player embed URL
        /// </summary>
        /// <value>The player embed URL.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "player_embed_url")]
        public string Player_Embed_Url { get; set; }

        /// <summary>
        /// Review link
        /// </summary>
        /// <value>The review link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "review_link")]
        public string ReviewLink { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <value>The status.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        /// <value>The type.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Embed presets
        /// </summary>
        /// <value>The embed presets.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed_presets")]
        public EmbedPresets EmbedPresets { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        /// <value>The duration.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        /// <value>The width.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        /// <value>The height.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        /// <value>The created time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Modified time
        /// </summary>
        /// <value>The modified time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "modified_time")]
        public DateTime ModifiedTime { get; set; }

        /// <summary>
        /// Modified time
        /// </summary>
        /// <value>The release time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "release_time")]
        public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        /// <value>The privacy.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        /// <value>The pictures.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Files
        /// </summary>
        /// <value>The files.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "files")]
        public List<File> Files { get; set; }

        /// <summary>
        /// Download
        /// </summary>
        /// <value>The download.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "download")]
        public List<Download> Download { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        /// <value>The tags.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "tags")]
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// Stats
        /// </summary>
        /// <value>The stats.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "stats")]
        public VideoStats Stats { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        /// <value>The metadata.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public VideoMetadata Metadata { get; set; }

        /// <summary>
        /// Embed
        /// </summary>
        /// <value>The embed.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed")]
        public Embed Embed { get; set; }

        /// <summary>
        /// The video's 360 spatial data
        /// </summary>
        /// <value>The spatial.</value>
        [PublicAPI]
        [CanBeNull]
        [JsonProperty(PropertyName = "spatial")]
        public Spatial Spatial { get; set; }

        /// <summary>
        /// Video status
        /// </summary>
        /// <value>The video status.</value>
        [PublicAPI]
        public VideoStatusEnum VideoStatus
        {
            get => ModelHelpers.GetEnumValue<VideoStatusEnum>(Status, StatusMappings);
            set => Status = ModelHelpers.GetEnumString(value, StatusMappings);
        }



        /// <summary>
        /// Mobile video link
        /// </summary>
        /// <value>The mobile video link.</value>
        [PublicAPI]
        public string MobileVideoLink => GetFileQualityUrl(FileQualityEnum.Mobile, false);

        /// <summary>
        /// Mobile video secure link
        /// </summary>
        /// <value>The mobile video secure link.</value>
        [PublicAPI]
        public string MobileVideoSecureLink => GetFileQualityUrl(FileQualityEnum.Mobile, true);

        /// <summary>
        /// Standard video link
        /// </summary>
        /// <value>The standard video link.</value>
        [PublicAPI]
        public string StandardVideoLink => GetFileQualityUrl(FileQualityEnum.Standard, false);

        /// <summary>
        /// Standard video secure link
        /// </summary>
        /// <value>The standard video secure link.</value>
        [PublicAPI]
        public string StandardVideoSecureLink => GetFileQualityUrl(FileQualityEnum.Standard, true);

        /// <summary>
        /// High definition video link
        /// </summary>
        /// <value>The high definition video link.</value>
        [PublicAPI]
        public string HighDefinitionVideoLink => GetFileQualityUrl(FileQualityEnum.HighDefinition, false);

        /// <summary>
        /// High definition video secure link
        /// </summary>
        /// <value>The high definition video secure link.</value>
        [PublicAPI]
        public string HighDefinitionVideoSecureLink => GetFileQualityUrl(FileQualityEnum.HighDefinition, true);

        /// <summary>
        /// Streaming video link
        /// </summary>
        /// <value>The streaming video link.</value>
        [PublicAPI]
        public string StreamingVideoLink => GetFileQualityUrl(FileQualityEnum.Streaming, false);

        /// <summary>
        /// Streaming video secure link
        /// </summary>
        /// <value>The streaming video secure link.</value>
        [PublicAPI]
        public string StreamingVideoSecureLink => GetFileQualityUrl(FileQualityEnum.Streaming, true);

        /// <summary>
        /// Gets the file quality URL.
        /// </summary>
        /// <param name="quality">The quality.</param>
        /// <param name="secureLink">if set to <c>true</c> [secure link].</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Gets or sets the transcoding status.
        /// </summary>
        /// <value>The transcoding status.</value>
        [PublicAPI]
        [CanBeNull]
        [JsonProperty(PropertyName = "transcode")]
        public Transcode Transcode { get; set; }


        ///// <summary>
        ///// Gets or sets the upload.
        ///// </summary>
        ///// <value>The upload.</value>
        //[PublicAPI]
        //[CanBeNull]
        //[JsonProperty(PropertyName = "upload")]
        //public Upload Upload { get; set; }


        /// <summary>
        /// Embed
        /// </summary>
        /// <value><c>true</c> if this instance is playable; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "is_playable")]
        public bool IsPlayable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is free.
        /// </summary>
        /// <value><c>true</c> if this instance is free; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "is_free")]
        public bool IsFree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can move to project.
        /// </summary>
        /// <value><c>true</c> if this instance can move to project; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "can_move_to_project")]
        public bool CanMoveToProject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has audio.
        /// </summary>
        /// <value><c>true</c> if this instance has audio; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "has_audio")]
        public bool HasAudio { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has audio tracks.
        /// </summary>
        /// <value><c>true</c> if this instance has audio tracks; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "has_audio_tracks")]
        public bool HasAudioTracks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has chapters.
        /// </summary>
        /// <value><c>true</c> if this instance has chapters; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "has_chapters")]
        public bool HasChapters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has interactive.
        /// </summary>
        /// <value><c>true</c> if this instance has interactive; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "has_interactive")]
        public bool HasInteractive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has viewer home page settings.
        /// </summary>
        /// <value><c>true</c> if this instance has viewer home page settings; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "has_viewer_home_page_settings")]
        public bool HasViewerHomePageSettings { get; set; }

        /// <summary>
        /// Gets or sets the content rating.
        /// </summary>
        /// <value>The content rating.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "content_rating")]
        public List<string> ContentRating { get; set; }

        /// <summary>
        /// Gets or sets the content rating class.
        /// </summary>
        /// <value>The content rating class.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "content_rating_class")]
        public string ContentRatingClass { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "context")]
        public Context Context { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is copyright restricted.
        /// </summary>
        /// <value><c>true</c> if this instance is copyright restricted; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "is_copyright_restricted")]
        public bool IsCopyrightRestricted { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "language")]
        [CanBeNull]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has text tracks.
        /// </summary>
        /// <value><c>true</c> if this instance has text tracks; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "has_text_tracks")]
        public bool HasTextTracks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [origin variable frame resolution].
        /// </summary>
        /// <value><c>true</c> if [origin variable frame resolution]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "origin_variable_frame_resolution")]
        public bool OriginVariableFrameResolution { get; set; }

        /// <summary>
        /// Gets or sets the last user action event date.
        /// </summary>
        /// <value>The last user action event date.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "last_user_action_event_date")]
        public DateTime? LastUserActionEventDate { get; set; }

        /// <summary>
        /// Gets or sets the allowed privacies.
        /// </summary>
        /// <value>The allowed privacies.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "allowed_privacies")]
        [CanBeNull]
        public List<string> AllowedPrivacies { get; set; }

        /// <summary>
        /// Gets or sets the custom URL.
        /// </summary>
        /// <value>The custom URL.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "custom_url")]
        [CanBeNull]
        public string CustomUrl { get; set; }

        /// <summary>
        /// Gets or sets the license.
        /// </summary>
        /// <value>The license.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        /// <summary>
        /// Gets or sets the manage link.
        /// </summary>
        /// <value>The manage link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "manage_link")]
        public string ManageLink { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>The page.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "page")]
        public VideoPageSettings Page { get; set; }

        /// <summary>
        /// Gets or sets the parent folder.
        /// </summary>
        /// <value>The parent folder.</value>
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