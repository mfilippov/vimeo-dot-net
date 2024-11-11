using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class ChannelMetadata.
    /// </summary>
    [Serializable]
    public class ChannelMetadata
    {

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        /// <value>The connections.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public ChannelConnections Connections { get; set; }

        /// <summary>
        /// Gets or sets the interactions.
        /// </summary>
        /// <value>The interactions.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "interactions")]
        public ChannelInteractions Interactions { get; set; }
    }
}