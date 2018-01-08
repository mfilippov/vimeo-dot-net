using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Tag
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Canonical
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "canonical")]
        public string Canonical { get; set; }
    }
}