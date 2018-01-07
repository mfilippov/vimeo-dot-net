using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Text track
    /// </summary>
    public class TextTrack
    {
        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TextTrackType? Type { get; set; }

        /// <summary>
        /// Language
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Hls link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "hls_link")]
        public string HlsLink { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    [PublicAPI]
    public enum TextTrackType
    {
        Captions,
        Chapters,
        Descriptions,
        Metadata,
        SubTitles
    }
}