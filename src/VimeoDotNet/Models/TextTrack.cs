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
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        /// <value>The type.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TextTrackType? Type { get; set; }

        /// <summary>
        /// Language
        /// </summary>
        /// <value>The language.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Hls link
        /// </summary>
        /// <value>The HLS link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "hls_link")]
        public string HlsLink { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Enum TextTrackType
    /// </summary>
    [PublicAPI]
    public enum TextTrackType
    {
        /// <summary>
        /// The captions
        /// </summary>
        Captions,
        /// <summary>
        /// The chapters
        /// </summary>
        Chapters,
        /// <summary>
        /// The descriptions
        /// </summary>
        Descriptions,
        /// <summary>
        /// The metadata
        /// </summary>
        Metadata,
        /// <summary>
        /// The sub titles
        /// </summary>
        SubTitles
    }
}