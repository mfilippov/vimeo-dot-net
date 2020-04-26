using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    public class ChannelFollowEntry
    {
       
        [PublicAPI]
        [JsonProperty(PropertyName = "added")]
        public bool Added { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "added_time")]
        public DateTime? AddedTime { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}