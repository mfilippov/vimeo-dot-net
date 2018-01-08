using JetBrains.Annotations;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video update metadata model
    /// </summary>
    public class VideoUpdateMetadata
    {
        /// <summary>
        /// The new title for the video
        /// </summary>
        [PublicAPI]
        public string Name { get; set; }

        /// <summary>
        /// The new description for the video
        /// </summary>
        [PublicAPI]
        public string Description { get; set; }

        /// <summary>
        /// The new privacy setting for the video.
        /// Content-type application/json is the only valid type for type "users",
        /// basic users can not set privacy to unlisted.
        /// </summary>
        [PublicAPI]
        public VideoPrivacyEnum? Privacy { get; set; }

        /// <summary>
        /// The videos new embed settings. Whitelist allows you to define all valid embed domains.
        ///  Check out our docs for adding and removing domains.
        /// </summary>
        [PublicAPI]
        public VideoEmbedPrivacyEnum? EmbedPrivacy { get; set; }

        /// <summary>
        /// Enable or disable the review page
        /// </summary>
        [PublicAPI]
        public bool? ReviewLinkEnabled { get; set; }

        /// <summary>
        /// When you set privacy to password, you must provide the password as an additional parameter
        /// </summary>
        [PublicAPI]
        public string Password { get; set; }

        /// <summary>
        /// The privacy for who can comment on the video.
        /// </summary>
        [PublicAPI]
        public VideoCommentsEnum? Comments { get; set; }

        /// <summary>
        /// Enable or disable the ability for anyone to add the video to an album, channel, or group.
        /// </summary>
        [PublicAPI]
        public bool? AllowAddToAlbumChannelGroup { get; set; }

        /// <summary>
        /// Enable or disable the ability for anyone to download video.
        /// </summary>
        [PublicAPI]
        public bool? AllowDownloadVideo { get; set; }
    }
}