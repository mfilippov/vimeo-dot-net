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
        /// <value>The space.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "space")]
        public Space Space { get; set; }

        /// <summary>
        /// Resets
        /// </summary>
        /// <value>The resets.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "resets")]
        public int Resets { get; set; }

        /// <summary>
        /// Quota
        /// </summary>
        /// <value>The quota.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "quota")]
        public UserQuota Quota { get; set; }
    }
}