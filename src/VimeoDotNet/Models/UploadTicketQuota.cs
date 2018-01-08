using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Upload ticket quota
    /// </summary>
    public class UploadTicketQuota
    {
        /// <summary>
        /// SD
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "sd")]
        public bool Sd { get; set; }

        /// <summary>
        /// HD
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "hd")]
        public bool Hd { get; set; }

        /// <summary>
        /// Total space
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "total_space")]
        public long TotalSpace { get; set; }

        /// <summary>
        /// Space used
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "space_used")]
        public long SpaceUsed { get; set; }

        /// <summary>
        /// Free space
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "free_space")]
        public long FreeSpace { get; set; }

        /// <summary>
        /// Max file size
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "max_file_size")]
        public long MaxFileSize { get; set; }

        /// <summary>
        /// Resets
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "resets")]
        public DateTime Resets { get; set; }
    }
}