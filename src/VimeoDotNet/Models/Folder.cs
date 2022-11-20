using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [DebuggerDisplay("{Id} {Name}")]
    public class Folder
    {
        [PublicAPI]
        [JsonProperty(PropertyName = "id")]
        public long? Id => ModelHelpers.ParseModelUriId(Uri);
        /// <summary>
        /// User
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Modified time
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "modified_time")]
        public DateTime ModifiedTime { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "resource_key")]
        public string ResourceKey { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}
