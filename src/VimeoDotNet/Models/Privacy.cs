using System;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Privacy
    /// </summary>
    [Serializable]
    public class Privacy
    {
        /// <summary>
        /// View
        /// </summary>
        public string view { get; set; }
        /// <summary>
        /// Embed
        /// </summary>
        public string embed { get; set; }
        /// <summary>
        /// Download
        /// </summary>
        public bool download { get; set; }
        /// <summary>
        /// Add
        /// </summary>
        public bool add { get; set; }

        /// <summary>
        /// View privacy
        /// </summary>
        public VideoPrivacyEnum ViewPrivacy
        {
            get { return ModelHelpers.GetEnumValue<VideoPrivacyEnum>(view); }
            set { view = ModelHelpers.GetEnumString(value); }
        }

        /// <summary>
        /// Embed privacy
        /// </summary>
        public VideoEmbedPrivacyEnum EmbedPrivacy
        {
            get { return ModelHelpers.GetEnumValue<VideoEmbedPrivacyEnum>(embed); }
            set { embed = ModelHelpers.GetEnumString(value); }
        }
    }
}