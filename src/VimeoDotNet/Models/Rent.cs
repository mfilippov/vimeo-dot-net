using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class Rent.
    /// </summary>
    public class Rent
    {
        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        /// <value>The stream.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "stream")]
        public string Stream { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the purchase time.
        /// </summary>
        /// <value>The purchase time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "purchase_time")]
        public string PurchaseTime { get; set; }
    }
}
