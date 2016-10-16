using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Download model
    /// </summary>
    [Serializable]
    public class Download
    {
        /// <summary>
        /// Quality
        /// </summary>
        public string quality { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// Expires
        /// </summary>
        public DateTime? expires { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }
    }
}