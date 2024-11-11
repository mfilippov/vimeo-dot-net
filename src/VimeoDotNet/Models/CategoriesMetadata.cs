using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album metadata
    /// </summary>
    [Serializable]
    public class CategoriesMetadata
    {
        /// <summary>
        /// Connections
        /// </summary>
        /// <value>The connections.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public CategoriesConnections Connections { get; set; }
    }
}