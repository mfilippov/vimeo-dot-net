using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{

    public class Category
    {
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "top_level")]
        public bool TopLevel { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "last_video_featured_time")]
        public string LastVideoFeaturedTime { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public AlbumMetadata MetaData { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "options")]
        public List<string> Options { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "subcategories")]
        public List<Subcategory> Subcategories { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "icon")]
        public Pictures Icon { get; set; }
    }
}