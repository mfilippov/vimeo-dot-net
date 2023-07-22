using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Models
{
    public class Spatial
    {
        /// <summary>
        /// The video's 360 field of view value
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "field_of_view")]
        [ValueRange(30, 90)]
        public int? FieldOfView { get; set; }

        /// <summary>
        /// The video's 360 spatial projection
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "projection")]
        public SpatialProjectionEnum Projection { get; set; }

        /// <summary>
        /// The video's 360 stereo format
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "stereo_format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StereoFormatEnum StereoFormat { get; set; }

        /// <summary>
        /// The video's 360 director timeline
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "director_timeline")]
        public List<DirectorTimelineEntry> DirectorTimeline { get; set; }
    }
}