using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Web site
    /// </summary>
    public class Website
    {
        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        /// <value>The description.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}