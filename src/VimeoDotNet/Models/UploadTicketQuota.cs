using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Upload ticket quota
    /// </summary>
    [Serializable]
    public class UploadTicketQuota
    {
        /// <summary>
        /// SD
        /// </summary>
        public bool sd { get; set; }
        /// <summary>
        /// HD
        /// </summary>
        public bool hd { get; set; }
        /// <summary>
        /// Total space
        /// </summary>
        public long total_space { get; set; }
        /// <summary>
        /// Space used
        /// </summary>
        public long space_used { get; set; }
        /// <summary>
        /// Free space
        /// </summary>
        public long free_space { get; set; }
        /// <summary>
        /// Max file size
        /// </summary>
        public long max_file_size { get; set; }
        /// <summary>
        /// Resets
        /// </summary>
        public DateTime resets { get; set; }
    }
}
