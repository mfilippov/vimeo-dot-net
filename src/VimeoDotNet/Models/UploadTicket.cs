using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Upload ticket
    /// </summary>
    public class UploadTicket
    {
        /// <summary>
        /// Ticket id
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "ticket_id")]
        public string TicketId { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Complete URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "complete_uri")]
        public string CompleteUri { get; set; }

        /// <summary>
        /// Upload URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload_uri")]
        public string UploadUri { get; set; }

        /// <summary>
        /// Upload URI secure
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload_uri_secure")]
        public string UploadUriSecure { get; set; }

        /// <summary>
        /// Upload link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload_link")]
        public string UploadLink { get; set; }

        /// <summary>
        /// Upload link secure
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload_link_secure")]
        public string UploadLinkSecure { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        /// <summary>
        /// Video
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "video")]
        public Video Video { get; set; }

        /// <summary>
        /// Upload status
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload")]
        public UploadStatus Upload { get; set; }

        /// <summary>
        /// Transcode
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "transcode")]
        public List<Transcode> Transcode { get; set; }

        /// <summary>
        /// Quota
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "quota")]
        public UploadTicketQuota Quota { get; set; }
    }
}