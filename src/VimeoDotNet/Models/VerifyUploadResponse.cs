using System;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class VerifyUploadResponse
    {
        public UploadStatusEnum Status { get; set; }
        public long BytesWritten { get; set; }
    }
}