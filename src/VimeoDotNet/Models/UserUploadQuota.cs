using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User upload quota
    /// </summary>
    public class UserUploadQuota
    {
        /// <summary>
        /// Space
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "space")]
        public Space Space { get; set; }

        /// <summary>
        /// Resets
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "resets")]
        public int Resets { get; set; }

        /// <summary>
        /// Quota
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "quota")]
        public UserQuota Quota { get; set; }
    }
}