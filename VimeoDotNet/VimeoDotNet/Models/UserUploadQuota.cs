using System;

namespace VimeoDotNet.Models
{
    public class UserUploadQuota
    {
        public Space space { get; set; }
        public int resets { get; set; }
        public UserQuota quota { get; set; }
    }
}
