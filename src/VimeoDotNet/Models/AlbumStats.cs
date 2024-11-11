
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album statistics
    /// </summary>
    public class AlbumStats
    {
        /// <summary>
        /// Videos
        /// </summary>
        /// <value>The plays.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "plays")]
        public int Plays { get; set; }
    }
}