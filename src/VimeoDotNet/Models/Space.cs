using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Space
    /// </summary>
    [Serializable]
    public class Space
    {
        /// <summary>
        /// Max
        /// </summary>
        public long max { get; set; }
        /// <summary>
        /// Free
        /// </summary>
        public long free { get; set; }
        /// <summary>
        /// Used
        /// </summary>
        public long used { get; set; }
    }
}
