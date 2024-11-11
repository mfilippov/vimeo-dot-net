using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Size
    /// </summary>
    public class Size
    {
        /// <summary>
        /// The width of the image.
        /// </summary>
        /// <value>The width.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        /// <summary>
        /// The height of the image.
        /// </summary>
        /// <value>The height.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        /// <summary>
        /// The direct link to the image.
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// The direct link to the image with a play button overlay.
        /// </summary>
        /// <value>The link with play button.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link_with_play_button")]
        public string LinkWithPlayButton { get; set; }
    }
}