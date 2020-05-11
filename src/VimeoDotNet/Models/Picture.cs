using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// The type of picture
    /// </summary>
    public enum PictureType
    {
        /// <summary>
        /// The default image for the video.
        /// </summary>
        Default,
        /// <summary>
        /// An image that is appropriate for all ages.
        /// </summary>
        Caution,
        /// <summary>
        /// A custom image for the video.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Picture
    /// </summary>
    public class Picture
    {
        /// <summary>
        /// Whether this picture is the active picture for its parent resource.
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// The picture's URI.
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// An array containing reference information about all available image files.
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "sizes")]
        public List<Size> Sizes { get; set; }

        /// <summary>
        /// The upload URL for the picture. This field appears when you create the picture resource for the first time.
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        ///
        /// The picture's resource key string.
        ///
        [PublicAPI]
        [JsonProperty(PropertyName = "resource_key")]
        public string ResourceKey { get; set; }
        
        /// <summary>
        /// The type of picture:
        /// </summary>
        [PublicAPI]
        [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(CamelCaseNamingStrategy))]
        [JsonProperty(PropertyName = "type")]
        public PictureType Type { get; set; }
    }
}