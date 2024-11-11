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
        /// <value>The state.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        /// <summary>
        /// Progress
        /// </summary>
        /// <value>The progress.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "progress")]
        public int Progress { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        /// <value>The message.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}