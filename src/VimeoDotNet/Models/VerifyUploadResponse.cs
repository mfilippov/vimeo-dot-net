using System;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Verify upload response
    /// </summary>
    [Serializable]
    public class VerifyUploadResponse
    {
        /// <summary>
        /// Status
        /// </summary>
        /// <value>The status.</value>
        public UploadStatusEnum Status { get; set; }

        /// <summary>
        /// Bytes written
        /// </summary>
        /// <value>The bytes written.</value>
        public long BytesWritten { get; set; }
    }
}