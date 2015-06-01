using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class UploadTicket
    {
        public string ticket_id { get; set; }
        public string uri { get; set; }
        public string complete_uri { get; set; }
        public string upload_uri { get; set; }
        public string upload_uri_secure { get; set; }
        public string upload_link { get; set; }
        public string upload_link_secure { get; set; }
        public string type { get; set; }
        public User user { get; set; }
        public Video video { get; set; }
        public UploadStatus upload { get; set; }
        public List<Transcode> transcode { get; set; }
        public UploadTicketQuota quota { get; set; }
    }
}