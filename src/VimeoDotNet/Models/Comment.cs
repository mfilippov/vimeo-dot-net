using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Comment model
    /// </summary>
    [Serializable]
    public class Comment
    {
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public User user { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        public DateTime created_time { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public List<Comment> comments { get; set; }
    }
}