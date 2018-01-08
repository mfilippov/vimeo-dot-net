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
        [PublicAPI]
        [JsonProperty(PropertyName = "data")]
        public List<T> Data { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        /// <summary>
        /// Page
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        /// <summary>
        /// Per page
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "per_page")]
        public int PerPage { get; set; }

        /// <summary>
        /// Paging
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "paging")]
        public Paging Paging { get; set; }
    }
}