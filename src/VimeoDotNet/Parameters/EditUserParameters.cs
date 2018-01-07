using System.Collections.Generic;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
    /// <summary>
    /// Edit user privacy comment option
    /// </summary>
    public enum EditUserPrivacyCommentOption
    {
        /// <summary>
        /// Anybody
        /// </summary>
        Anybody,

        /// <summary>
        /// Nobody
        /// </summary>
        Nobody,

        /// <summary>
        /// Contacts
        /// </summary>
        Contacts
    }

    /// <summary>
    /// Edit user privacy view option
    /// </summary>
    public enum EditUserPrivacyViewOption
    {
        /// <summary>
        /// Anybody
        /// </summary>
        Anybody,

        /// <summary>
        /// Nobody
        /// </summary>
        Nobody,

        /// <summary>
        /// Contacts
        /// </summary>
        Contacts,

        /// <summary>
        /// Password
        /// </summary>
        Password,

        /// <summary>
        /// Users
        /// </summary>
        Users,

        /// <summary>
        /// Disable
        /// </summary>
        Disable
    }

    /// <summary>
    ///Edit user privacy embed option
    /// </summary>
    public enum EditUserPrivacyEmbedOption
    {
        /// <summary>
        /// Public
        /// </summary>
        Public,

        /// <summary>
        /// Private
        /// </summary>
        Private,

        /// <summary>
        /// Whitelist
        /// </summary>
        Whitelist
    }

    /// <summary>
    /// Edit user parameters
    /// </summary>
    public class EditUserParameters : IParameterProvider
    {
        /// <summary>
        /// Sets the default download setting for all future videos uploaded by this user. If true, the video can be downloaded by any user.
        /// </summary>
        public bool? VideosPrivacyDownload { get; set; }

        /// <summary>
        /// Sets the default add setting for all future videos uploaded by this user. If true, anyone can add the video to an album, channel, or group.
        /// </summary>
        public bool? VideosPrivacyAdd { get; set; }

        /// <summary>
        /// Sets the default comment setting for all future videos uploaded by this user. It specifies who can comment on the video.
        /// </summary>
        public EditUserPrivacyCommentOption? VideosPrivacyComments { get; set; }

        /// <summary>
        /// Sets the default view setting for all future videos uploaded by this user. It specifies who can view the video.
        /// </summary>
        public EditUserPrivacyViewOption? VideosPrivacyView { get; set; }

        /// <summary>
        /// Sets the default embed setting for all future videos uploaded by this user. Whitelist allows you to define all valid embed domains.
        /// </summary>
        public EditUserPrivacyEmbedOption? VideosPrivacyEmbed { get; set; }

        /// <summary>
        /// The user's display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user's location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The user's bio
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// Performs validation and returns a description of the first error encountered.
        /// </summary>
        /// <returns>Description of first error, or null if none found.</returns>
        public string ValidationError()
        {
            // no parameter restrictions indicated
            return null;
        }

        /// <summary>
        /// Provides universal interface to retrieve parameter values.
        /// </summary>
        /// <returns>Returns all parameters as name/value pairs.</returns>
        public IDictionary<string, string> GetParameterValues()
        {
            Dictionary<string, string> parameterValues = new Dictionary<string, string>();

            if (VideosPrivacyDownload.HasValue)
            {
                parameterValues.Add("videos.privacy.download", VideosPrivacyDownload.Value.ToString().ToLower());
            }

            if (VideosPrivacyAdd.HasValue)
            {
                parameterValues.Add("videos.privacy.add", VideosPrivacyAdd.Value.ToString().ToLower());
            }

            if (VideosPrivacyComments.HasValue)
            {
                parameterValues.Add("videos.privacy.comments", VideosPrivacyComments.Value.GetParameterValue());
            }

            if (VideosPrivacyView.HasValue)
            {
                parameterValues.Add("videos.privacy.view", VideosPrivacyView.Value.GetParameterValue());
            }

            if (VideosPrivacyEmbed.HasValue)
            {
                parameterValues.Add("videos.privacy.embed", VideosPrivacyEmbed.Value.GetParameterValue());
            }

            if (Name != null)
            {
                parameterValues.Add("name", Name);
            }

            if (Location != null)
            {
                parameterValues.Add("location", Location);
            }

            if (Bio != null)
            {
                parameterValues.Add("bio", Bio);
            }

            if (parameterValues.Keys.Count > 0)
            {
                return parameterValues;
            }

            return null;
        }
    }
}