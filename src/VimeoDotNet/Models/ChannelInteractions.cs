using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class ChannelInteractions.
    /// </summary>
    public class ChannelInteractions
    {
        /// <summary>
        /// Gets or sets the add moderators.
        /// </summary>
        /// <value>The add moderators.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "add_moderators")]
        public ChannelInteractionsEntry AddModerators { get; set; }

        /// <summary>
        /// Gets or sets the moderate videos.
        /// </summary>
        /// <value>The moderate videos.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "moderate_videos")]
        public ChannelInteractionsEntry ModerateVideos { get; set; }

        /// <summary>
        /// Gets or sets the follow.
        /// </summary>
        /// <value>The follow.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "follow")]
        public ChannelFollowEntry Follow { get; set; }
    }
}