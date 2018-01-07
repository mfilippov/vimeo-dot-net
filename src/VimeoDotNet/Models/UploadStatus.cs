using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Upload status
    /// </summary>
    public class UploadStatus
    {
        /// <summary>
        /// State
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        /// <summary>
        /// Progress
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "progress")]
        public int Progress { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}