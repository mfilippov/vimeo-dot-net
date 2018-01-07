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
        [PublicAPI]
        [JsonProperty(PropertyName = "view")]
        public string View { get; set; }

        /// <summary>
        /// Embed
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed")]
        public string Embed { get; set; }

        /// <summary>
        /// Download
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "download")]
        public bool Download { get; set; }

        /// <summary>
        /// Add
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "add")]
        public bool Add { get; set; }

        /// <summary>
        /// View privacy
        /// </summary>
        [PublicAPI]
        public VideoPrivacyEnum ViewPrivacy
        {
            get => ModelHelpers.GetEnumValue<VideoPrivacyEnum>(View);
            set => View = ModelHelpers.GetEnumString(value);
        }

        /// <summary>
        /// Embed privacy
        /// </summary>
        [PublicAPI]
        public VideoEmbedPrivacyEnum EmbedPrivacy
        {
            get => ModelHelpers.GetEnumValue<VideoEmbedPrivacyEnum>(Embed);
            set => Embed = ModelHelpers.GetEnumString(value);
        }
    }
}