using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    public class Channel
    {
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public string CreatedTime { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "modified_time")]
        public string ModifiedTime { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "tags")]
        public List<Tag> Tags { get; set; }
        
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "header")]
        public string Header { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "categories")]
        public List<Category> Categories { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public ChannelMetadata Metadata { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "resource_key")]
        public string ResourceKey { get; set; }


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

        private static readonly Regex RegexAlbumUri = new Regex(@"/channels/(?<channelid>\d+)/?$");
    }
}