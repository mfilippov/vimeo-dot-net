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
        /// <value>The follow.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "follow")]
        public Follow Follow { get; set; }
    }
}