using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class Channel.
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        /// <value>The created time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public string CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the modified time.
        /// </summary>
        /// <value>The modified time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "modified_time")]
        public string ModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "tags")]
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the pictures.
        /// </summary>
        /// <value>The pictures.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "header")]
        public Picture Header { get; set; }

        /// <summary>
        /// Gets or sets the privacy.
        /// </summary>
        /// <value>The privacy.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "categories")]
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public ChannelMetadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets the resource key.
        /// </summary>
        /// <value>The resource key.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "resource_key")]
        public string ResourceKey { get; set; }


        /// <summary>
        /// Gets the channel identifier.
        /// </summary>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        [PublicAPI]
        public long? GetChannelId()
        {
            if (string.IsNullOrEmpty(Uri))
            {
                return null;
            }

            var match = RegexAlbumUri.Match(Uri);
            if (match.Success)
            {
                return long.Parse(match.Groups["channelid"].Value);
            }

            return null;
        }

        /// <summary>
        /// The regex album URI
        /// </summary>
        private static readonly Regex RegexAlbumUri = new Regex(@"/channels/(?<channelid>\d+)/?$");
    }
}