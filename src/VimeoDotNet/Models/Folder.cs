using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class Folder.
    /// </summary>
    [DebuggerDisplay("{Id} {Name}")]
    public class Folder
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "id")]
        public long? Id => ModelHelpers.ParseModelUriId(Uri);
        /// <summary>
        /// User
        /// </summary>
        /// <value>The user.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        /// <value>The created time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Modified time
        /// </summary>
        /// <value>The modified time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "modified_time")]
        public DateTime ModifiedTime { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        /// <value>The privacy.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <value>The resource key.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "resource_key")]
        public string ResourceKey { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}
