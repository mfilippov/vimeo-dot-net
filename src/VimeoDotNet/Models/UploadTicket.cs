using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Upload ticket
    /// </summary>
    [Serializable]
    public class UploadTicket
    {
        /// <summary>
        /// Ticket id
        /// </summary>
        public string ticket_id { get; set; }
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
        /// <summary>
        /// Complete URI
        /// </summary>
        public string complete_uri { get; set; }
        /// <summary>
        /// Upload URI
        /// </summary>
        public string upload_uri { get; set; }
        /// <summary>
        /// Upload URI secure
        /// </summary>
        public string upload_uri_secure { get; set; }
        /// <summary>
        /// Upload link
        /// </summary>
        public string upload_link { get; set; }
        /// <summary>
        /// Upload link secure
        /// </summary>
        public string upload_link_secure { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// If your upload type is pull, Vimeo will download the video hosted at this public URL. This URL must be valid for at least 24 hours.
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// User
        /// </summary>
        public User user { get; set; }
        /// <summary>
        /// Video
        /// </summary>
        public Video video { get; set; }
        /// <summary>
        /// Upload status
        /// </summary>
        public UploadStatus upload { get; set; }
        /// <summary>
        /// Transcode
        /// </summary>
        public List<Transcode> transcode { get; set; }
        /// <summary>
        /// Quota
        /// </summary>
        public UploadTicketQuota quota { get; set; }
    }
}