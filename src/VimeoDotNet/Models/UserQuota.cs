using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User quota
    /// </summary>
    [Serializable]
    public class UserQuota
    {
        /// <summary>
        /// HD
        /// </summary>
        public bool hd { get; set; }
        /// <summary>
        /// SD
        /// </summary>
        public bool sd { get; set; }
    }
}