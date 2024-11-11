using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
    /// <summary>
    /// Edit user privacy comment option
    /// </summary>
    [PublicAPI]
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
    [PublicAPI]
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
    /// Edit user privacy embed option
    /// </summary>
    [PublicAPI]
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

    /// <inheritdoc />
    public class EditUserParameters : IParameterProvider
    {
        /// <summary>
        /// Sets the default download setting for all future videos uploaded by this user. If true, the video can be downloaded by any user.
        /// </summary>
        /// <value><c>null</c> if [videos privacy download] contains no value, <c>true</c> if [videos privacy download]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        public bool? VideosPrivacyDownload { get; set; }

        /// <summary>
        /// Sets the default add setting for all future videos uploaded by this user. If true, anyone can add the video to an album, channel, or group.
        /// </summary>
        /// <value><c>null</c> if [videos privacy add] contains no value, <c>true</c> if [videos privacy add]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        public bool? VideosPrivacyAdd { get; set; }

        /// <summary>
        /// Sets the default comment setting for all future videos uploaded by this user. It specifies who can comment on the video.
        /// </summary>
        /// <value>The videos privacy comments.</value>
        [PublicAPI]
        [JsonConverter(typeof(StringEnumConverter))]
        public EditUserPrivacyCommentOption? VideosPrivacyComments { get; set; }

        /// <summary>
        /// Sets the default view setting for all future videos uploaded by this user. It specifies who can view the video.
        /// </summary>
        /// <value>The videos privacy view.</value>
        [PublicAPI]
        [JsonConverter(typeof(StringEnumConverter))]
        public EditUserPrivacyViewOption? VideosPrivacyView { get; set; }

        /// <summary>
        /// Sets the default embed setting for all future videos uploaded by this user. Whitelist allows you to define all valid embed domains.
        /// </summary>
        /// <value>The videos privacy embed.</value>
        [PublicAPI]
        [JsonConverter(typeof(StringEnumConverter))]
        public EditUserPrivacyEmbedOption? VideosPrivacyEmbed { get; set; }

        /// <summary>
        /// The user's display name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        public string Name { get; set; }

        /// <summary>
        /// The user's location
        /// </summary>
        /// <value>The location.</value>
        [PublicAPI]
        public string Location { get; set; }

        /// <summary>
        /// The user's bio
        /// </summary>
        /// <value>The bio.</value>
        [PublicAPI]
        public string Bio { get; set; }

        /// <inheritdoc />
        [PublicAPI]
        public string ValidationError()
        {
            // no parameter restrictions indicated
            return null;
        }

        /// <inheritdoc />
        [PublicAPI]
        public IDictionary<string, string> GetParameterValues()
        {
            var parameterValues = new Dictionary<string, string>();

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

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (parameterValues.Keys.Count > 0)
            {
                return parameterValues;
            }

            return null;
        }
    }
}