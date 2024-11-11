using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class ChannelFollowEntry.
    /// </summary>
    public class ChannelFollowEntry
    {

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ChannelFollowEntry" /> is added.
        /// </summary>
        /// <value><c>true</c> if added; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "added")]
        public bool Added { get; set; }

        /// <summary>
        /// Gets or sets the added time.
        /// </summary>
        /// <value>The added time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "added_time")]
        public DateTime? AddedTime { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}