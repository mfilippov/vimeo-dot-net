using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Paginated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Paginated<T> where T : class
    {
        /// <summary>
        /// Content
        /// </summary>
        public List<T> data { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// Page
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// Per page
        /// </summary>
        public int per_page { get; set; }
        /// <summary>
        /// Paging
        /// </summary>
        public Paging paging { get; set; }
    }
}