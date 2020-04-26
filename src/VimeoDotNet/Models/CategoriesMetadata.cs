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
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public CategoriesConnections Connections { get; set; }
    }
}