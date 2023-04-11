using System;

namespace VimeoDotNet.Enums
{
    /// <summary>
    /// View privacy
    /// </summary>
    public enum VideoPrivacyEnum
    {
        /// <summary>
        /// Nobody - No one except the owner can access the video. This privacy setting appears as Private on the Vimeo front end.
        /// </summary>
        Nobody,

        /// <summary>
        /// Anybody - Anyone can access the video. This privacy setting appears as Public on the Vimeo front end.
        /// </summary>
        Anybody,

        /// <summary>
        /// Contacts - Only those who follow the owner on Vimeo can access the video.
        /// </summary>
        [Obsolete]
        Contacts,

        /// <summary>
        /// Users - Only Vimeo members can access the video.
        /// </summary>
        [Obsolete]
        Users,

        /// <summary>
        /// Password - Only those with the password can access the video.
        /// </summary>
        Password,

        /// <summary>
        /// Disable - The video is embeddable, but it's hidden on Vimeo and can't be played. This privacy setting appears as Hide from Vimeo on the Vimeo front end.
        /// </summary>
        [Obsolete]
        Disable,

        /// <summary>
        /// Unlisted - Only those with the private link can access the video.
        /// </summary>
        Unlisted
    }
}