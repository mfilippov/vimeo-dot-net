using System;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Embed presets
    /// </summary>
    [Serializable]
    public class EmbedPresets
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? id
        {
            get { return ModelHelpers.ParseModelUriId(uri); }
        }

        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Settings
        /// </summary>
        public PresetSettings settings { get; set; }
    }
}