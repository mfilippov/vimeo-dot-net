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
        [PublicAPI]
        [JsonProperty(PropertyName = "hd")]
        public bool Hd { get; set; }

        /// <summary>
        /// SD
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "sd")]
        public bool Sd { get; set; }
    }
}