using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    [DebuggerDisplay("{Type}")]
    public class Item
    {
        /// <summary>
        /// "video","folder"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("folder")]
        public Folder Folder { get; set; }
        [JsonProperty("video")]
        public Video Video { get; set; }

        public bool IsVideo => Type == "video";

        public bool IsFolder => Type == "folder";
    }
}
