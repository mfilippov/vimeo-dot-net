using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User upload quota
    /// </summary>
    [Serializable]
    public class UserUploadQuota
    {
        /// <summary>
        /// Space
        /// </summary>
        public Space space { get; set; }
        /// <summary>
        /// Resets
        /// </summary>
        public int resets { get; set; }
        /// <summary>
        /// Quota
        /// </summary>
        public UserQuota quota { get; set; }
    }
}