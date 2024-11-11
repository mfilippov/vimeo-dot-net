using JetBrains.Annotations;
using Newtonsoft.Json;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Embed presets
    /// </summary>
    public class EmbedPresets
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value>The identifier.</value>
        [PublicAPI]
        public long? Id => ModelHelpers.ParseModelUriId(Uri);

        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Settings
        /// </summary>
        /// <value>The settings.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "settings")]
        public PresetSettings Settings { get; set; }
    }
}