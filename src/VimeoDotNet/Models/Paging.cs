using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Paging
    /// </summary>
    public class Paging
    {
        /// <summary>
        /// Next
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }

        /// <summary>
        /// Previous
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "previous")]
        public string Previous { get; set; }

        /// <summary>
        /// First
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "first")]
        public string First { get; set; }

        /// <summary>
        /// Last
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "last")]
        public string Last { get; set; }
    }
}