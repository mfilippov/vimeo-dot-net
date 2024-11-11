using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{

    /// <summary>
    /// Class Category.
    /// </summary>
    public class Category
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
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [top level].
        /// </summary>
        /// <value><c>true</c> if [top level]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "top_level")]
        public bool TopLevel { get; set; }

        /// <summary>
        /// Gets or sets the pictures.
        /// </summary>
        /// <value>The pictures.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Gets or sets the last video featured time.
        /// </summary>
        /// <value>The last video featured time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "last_video_featured_time")]
        public string LastVideoFeaturedTime { get; set; }

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public AlbumMetadata MetaData { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "options")]
        public List<string> Options { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the subcategories.
        /// </summary>
        /// <value>The subcategories.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "subcategories")]
        public List<Subcategory> Subcategories { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "icon")]
        public Pictures Icon { get; set; }
    }
}