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
        /// <value>The next.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }

        /// <summary>
        /// Previous
        /// </summary>
        /// <value>The previous.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "previous")]
        public string Previous { get; set; }

        /// <summary>
        /// First
        /// </summary>
        /// <value>The first.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "first")]
        public string First { get; set; }

        /// <summary>
        /// Last
        /// </summary>
        /// <value>The last.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "last")]
        public string Last { get; set; }
    }
}