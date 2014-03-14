using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class UploadTicketQuota
    {
        public bool sd { get; set; }
        public bool hd { get; set; }
        public long total_space { get; set; }
        public long space_used { get; set; }
        public int free_space { get; set; }
        public long max_file_size { get; set; }
        public DateTime resets { get; set; }
    }
}
