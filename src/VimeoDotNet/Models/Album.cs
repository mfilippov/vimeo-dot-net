using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album model
    /// </summary>
    public class Album
    {
        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// User
        /// </summary>
        /// <value>The user.</value>

        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        /// <value>The description.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        /// <value>The duration.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        /// <summary>
        /// CreatedTime
        /// </summary>
        /// <value>The created time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public string CreatedTime { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        /// <value>The pictures.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        /// <value>The privacy.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        /// <summary>
        /// Stats
        /// </summary>
        /// <value>The stats.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "stats")]
        public AlbumStats Stats { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        /// <value>The metadata.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public AlbumMetadata Metadata { get; set; }

        /// <summary>
        /// Return album id if exists
        /// </summary>
        /// <returns>AlbumId or null</returns>
        [PublicAPI]
        public long? GetAlbumId()
        {
            if (string.IsNullOrEmpty(Uri))
            {
                return null;
            }

            var match = RegexAlbumUri.Match(Uri);
            if (match.Success)
            {
                return long.Parse(match.Groups["albumId"].Value);
            }

            return null;
        }

        /// <summary>
        /// The regex album URI
        /// </summary>
        private static readonly Regex RegexAlbumUri = new Regex(@"/albums/(?<albumId>\d+)/?$");
    }
}