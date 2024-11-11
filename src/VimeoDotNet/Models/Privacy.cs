using JetBrains.Annotations;
using Newtonsoft.Json;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Privacy
    /// </summary>
    public class Privacy
    {
        /// <summary>
        /// View
        /// </summary>
        /// <value>The view.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "view")]
        public string View { get; set; }

        /// <summary>
        /// Embed
        /// </summary>
        /// <value>The embed.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed")]
        public string Embed { get; set; }

        /// <summary>
        /// Download
        /// </summary>
        /// <value><c>true</c> if download; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "download")]
        public bool Download { get; set; }

        /// <summary>
        /// Add
        /// </summary>
        /// <value><c>true</c> if add; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "add")]
        public bool Add { get; set; }

        /// <summary>
        /// View privacy
        /// </summary>
        /// <value>The view privacy.</value>
        [PublicAPI]
        public VideoPrivacyEnum ViewPrivacy
        {
            get => ModelHelpers.GetEnumValue<VideoPrivacyEnum>(View);
            set => View = ModelHelpers.GetEnumString(value);
        }

        /// <summary>
        /// Embed privacy
        /// </summary>
        /// <value>The embed privacy.</value>
        [PublicAPI]
        public VideoEmbedPrivacyEnum EmbedPrivacy
        {
            get => ModelHelpers.GetEnumValue<VideoEmbedPrivacyEnum>(Embed);
            set => Embed = ModelHelpers.GetEnumString(value);
        }
    }
}