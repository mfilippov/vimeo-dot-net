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
        [PublicAPI]
        public long? Id => ModelHelpers.ParseModelUriId(Uri);

        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Settings
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "settings")]
        public PresetSettings Settings { get; set; }
    }
}