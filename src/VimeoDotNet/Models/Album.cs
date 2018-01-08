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
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// User
        /// </summary>

        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        /// <summary>
        /// CreatedTime
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public string CreatedTime { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        /// <summary>
        /// Stats
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "stats")]
        public AlbumStats Stats { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
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

        private static readonly Regex RegexAlbumUri = new Regex(@"/albums/(?<albumId>\d+)/?$");
    }
}