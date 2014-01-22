using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    public class UploadTicket
    {
        public string ticket { get; set; }
        public string uri { get; set; }
        public string endpoint { get; set; }
        public string endpoint_secure { get; set; }
        public string type { get; set; }
        public User user { get; set; }
        public Video video { get; set; }
        public UploadStatus upload { get; set; }
        public List<Transcode> transcode { get; set; }
        public UploadTicketQuota quota { get; set; }
    }
}
