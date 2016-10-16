using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Tag
    /// </summary>
    [Serializable]
    public class Tag
    {
        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Canonical
        /// </summary>
        public string canonical { get; set; }
    }
}