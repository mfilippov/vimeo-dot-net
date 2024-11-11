using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class Item.
    /// </summary>
    [DebuggerDisplay("{Type}")]
    public class Item
    {
        /// <summary>
        /// "video","folder"
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>The folder.</value>
        [JsonProperty("folder")]
        public Folder Folder { get; set; }
        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>The video.</value>
        [JsonProperty("video")]
        public Video Video { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is video.
        /// </summary>
        /// <value><c>true</c> if this instance is video; otherwise, <c>false</c>.</value>
        public bool IsVideo => Type == "video";

        /// <summary>
        /// Gets a value indicating whether this instance is folder.
        /// </summary>
        /// <value><c>true</c> if this instance is folder; otherwise, <c>false</c>.</value>
        public bool IsFolder => Type == "folder";
    }
}
