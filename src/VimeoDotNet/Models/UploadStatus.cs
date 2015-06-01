using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class UploadStatus
    {
        public string state { get; set; }
        public int progress { get; set; }
        public string message { get; set; }
    }
}