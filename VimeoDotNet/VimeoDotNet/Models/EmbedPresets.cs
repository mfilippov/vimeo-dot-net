using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class EmbedPresets
    {
        public string uri { get; set; }
        public string name { get; set; }
        public PresetSettings settings { get; set; }
    }
}
