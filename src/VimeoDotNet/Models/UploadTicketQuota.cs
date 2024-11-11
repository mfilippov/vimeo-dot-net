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
        /// <value><c>true</c> if sd; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "sd")]
        public bool Sd { get; set; }

        /// <summary>
        /// HD
        /// </summary>
        /// <value><c>true</c> if hd; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "hd")]
        public bool Hd { get; set; }

        /// <summary>
        /// Total space
        /// </summary>
        /// <value>The total space.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "total_space")]
        public long TotalSpace { get; set; }

        /// <summary>
        /// Space used
        /// </summary>
        /// <value>The space used.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "space_used")]
        public long SpaceUsed { get; set; }

        /// <summary>
        /// Free space
        /// </summary>
        /// <value>The free space.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "free_space")]
        public long FreeSpace { get; set; }

        /// <summary>
        /// Max file size
        /// </summary>
        /// <value>The maximum size of the file.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "max_file_size")]
        public long MaxFileSize { get; set; }

        /// <summary>
        /// Resets
        /// </summary>
        /// <value>The resets.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "resets")]
        public DateTime Resets { get; set; }
    }
}