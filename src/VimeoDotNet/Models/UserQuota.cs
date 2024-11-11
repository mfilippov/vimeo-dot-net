using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User quota
    /// </summary>
    public class UserQuota
    {
        /// <summary>
        /// HD
        /// </summary>
        /// <value><c>true</c> if hd; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "hd")]
        public bool Hd { get; set; }

        /// <summary>
        /// SD
        /// </summary>
        /// <value><c>true</c> if sd; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "sd")]
        public bool Sd { get; set; }
    }
}