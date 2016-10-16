using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Paging
    /// </summary>
    [Serializable]
    public class Paging
    {
        /// <summary>
        /// Next
        /// </summary>
        public string next { get; set; }
        /// <summary>
        /// Previous
        /// </summary>
        public string previous { get; set; }
        /// <summary>
        /// First
        /// </summary>
        public string first { get; set; }
        /// <summary>
        /// Last
        /// </summary>
        public string last { get; set; }
    }
}