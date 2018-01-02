using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VimeoDotNet.Models
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class TextTrack
    {
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
        /// <summary>
        /// Active
        /// </summary>
        public bool active { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TextTrackType? type { get; set; }
        /// <summary>
        /// Language
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// Hls link
        /// </summary>
        public string hls_link { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }
    }

    
    public enum TextTrackType
    {
        captions,
        chapters,
        descriptions,
        metadata,
        subtitles
    }
}