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
        /// <value>The maximum.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "max")]
        public long Max { get; set; }

        /// <summary>
        /// Free
        /// </summary>
        /// <value>The free.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "free")]
        public long Free { get; set; }

        /// <summary>
        /// Used
        /// </summary>
        /// <value>The used.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "used")]
        public long Used { get; set; }

        /// <summary>
        /// Kind of space unit
        /// </summary>
        /// <value>The unit.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "unit")]
        public SpaceUnit Unit { get; set; }
    }
}