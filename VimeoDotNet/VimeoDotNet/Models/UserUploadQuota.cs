using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class UserUploadQuota
    {
        public Space space { get; set; }
        public int resets { get; set; }
        public UserQuota quota { get; set; }
    }
}