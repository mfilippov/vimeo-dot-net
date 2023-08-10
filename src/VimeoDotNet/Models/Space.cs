using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Space
    /// </summary>
    public class Space
    {
        /// <summary>
        /// Max
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "max")]
        public long Max { get; set; }

        /// <summary>
        /// Free
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "free")]
        public long Free { get; set; }

        /// <summary>
        /// Used
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "used")]
        public long Used { get; set; }
        
        /// <summary>
        /// Kind of space unit
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "unit")]
        public SpaceUnit Unit { get; set; }
    }
}