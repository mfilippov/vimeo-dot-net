using System;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video update metadata model
    /// </summary>
    [Serializable]
    public class VideoUpdateMetadata
    {
        /// <summary>
        /// The new title for the video
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The new description for the video
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The new privacy setting for the video.
        /// Content-type application/json is the only valid type for type "users",
        /// basic users can not set privacy to unlisted.
        /// </summary>
        public VideoPrivacyEnum Privacy { get; set; }

        /// <summary>
        /// The videos new embed settings. Whitelist allows you to define all valid embed domains.
        ///  Check out our docs for adding and removing domains.
        /// </summary>
        public VideoEmbedPrivacyEnum EmbedPrivacy { get; set; }

        /// <summary>
        /// Enable or disable the review page
        /// </summary>
        public bool ReviewLinkEnabled { get; set; }

        /// <summary>
        /// When you set privacy to password, you must provide the password as an additional parameter
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The privacy for who can comment on the video.
        /// </summary>
        public VideoCommentsEnum Comments { get; set; }

        /// <summary>
        /// Enable or disable the ability for anyone to add the video to an album, channel, or group.
        /// </summary>
        public bool AllowAddToAlbumChannelGroup { get; set; }

        /// <summary>
        /// Enable or disable the ability for anyone to download video.
        /// </summary>
        public bool AllowDownloadVideo { get; set; }
    }
}