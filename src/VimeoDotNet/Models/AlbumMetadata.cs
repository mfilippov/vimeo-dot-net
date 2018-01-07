using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album metadata
    /// </summary>
    [Serializable]
    public class AlbumMetadata
    {
        /// <summary>
        /// Connections
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "connections")]
        public AlbumConnections Connections { get; set; }
    }
}