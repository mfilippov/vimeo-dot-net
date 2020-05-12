using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class ChannelMetadata
    {

        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public ChannelConnections Connections { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "interactions")]
        public ChannelInteractions Interactions { get; set; }
    }
}