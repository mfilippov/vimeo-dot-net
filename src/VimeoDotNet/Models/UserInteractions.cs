using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User interactions
    /// </summary>
    public class UserInteractions
    {
        /// <summary>
        /// Follow
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "follow")]
        public Follow Follow { get; set; }
    }
}