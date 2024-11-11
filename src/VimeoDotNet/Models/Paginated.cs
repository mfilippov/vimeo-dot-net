using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Paginated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paginated<T> where T : class
    {
        /// <summary>
        /// Content
        /// </summary>
        /// <value>The data.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "data")]
        public List<T> Data { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        /// <value>The total.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        /// <summary>
        /// Page
        /// </summary>
        /// <value>The page.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        /// <summary>
        /// Per page
        /// </summary>
        /// <value>The per page.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "per_page")]
        public int PerPage { get; set; }

        /// <summary>
        /// Paging
        /// </summary>
        /// <value>The paging.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "paging")]
        public Paging Paging { get; set; }
    }
}