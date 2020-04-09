using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Upload ticket
    /// </summary>
    public class TusResumableUploadTicket : Video
    {
        
        /// <summary>
        /// Upload status
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload")]
        public ResumableUploadStatus Upload { get; set; }
    }

    public class ResumableUploadStatus
    {
        /// <summary>
        /// Upload Approach
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "approach")]
        public string Approach { get; set; }

        /// <summary>
        /// Upload Status
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }


        /// <summary>
        /// Upload link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload_link")]
        public string UploadLink { get; set; }

        /// <summary>
        /// Video Size
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }

    }
}
