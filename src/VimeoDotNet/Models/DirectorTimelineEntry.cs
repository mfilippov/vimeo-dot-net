using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// An entry in a video's 360 director timeline
    /// </summary>
    public class DirectorTimelineEntry
    {
        /// <summary>
        /// The timeline pitch value, ranging from a minimum of -90 to a maximum of 90
        /// </summary>
        /// <value>The pitch.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "pitch")]
        [ValueRange(-90, 90)]
        public int Pitch { get; set; }

        /// <summary>
        /// The timeline roll value
        /// </summary>
        /// <value>The roll.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "roll")]
        public int Roll { get; set; }

        /// <summary>
        /// The timeline yaw value, ranging from a minimum of 0 to a maximum of 360
        /// </summary>
        /// <value>The yaw.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "yaw")]
        [ValueRange(0, 360)]
        public int Yaw { get; set; }

        /// <summary>
        /// The timeline time code
        /// </summary>
        /// <value>The time code.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "time_code")]
        public long TimeCode { get; set; }
    }
}