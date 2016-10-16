using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Upload status
    /// </summary>
    [Serializable]
    public class UploadStatus
    {
        /// <summary>
        /// State
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// Progress
        /// </summary>
        public int progress { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string message { get; set; }
    }
}