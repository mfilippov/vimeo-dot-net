using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    public class ChannelInteractions
    {
        [PublicAPI]
        [JsonProperty(PropertyName = "add_moderators")]
        public ChannelInteractionsEntry AddModerators { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "moderate_videos")]
        public ChannelInteractionsEntry ModerateVideos { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "follow")]
        public ChannelFollowEntry Follow { get; set; }
    }
}