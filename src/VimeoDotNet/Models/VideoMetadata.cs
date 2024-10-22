using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Video metadata
    /// </summary>
    public class VideoMetadata
    {
        /// <summary>
        /// Connections
        /// </summary>
        /// <value>The connections.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public VideoConnections Connections { get; set; }

        /// <summary>
        /// Interactions
        /// </summary>
        /// <value>The interactions.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "interactions")]
        public VideoInteractions Interactions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is screen record.
        /// </summary>
        /// <value><c>true</c> if this instance is screen record; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "is_screen_record")]
        public bool IsScreenRecord { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is vimeo create.
        /// </summary>
        /// <value><c>true</c> if this instance is vimeo create; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "is_vimeo_create")]
        public bool IsVimeoCreate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is webinar.
        /// </summary>
        /// <value><c>true</c> if this instance is webinar; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "is_webinar")]
        public bool IsWebinar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be replaced.
        /// </summary>
        /// <value><c>true</c> if this instance can be replaced; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "can_be_replaced")]
        public bool CanBeReplaced { get; set; }
    }
}