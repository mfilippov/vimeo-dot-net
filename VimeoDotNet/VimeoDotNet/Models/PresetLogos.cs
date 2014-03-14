using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class PresetLogos
    {
        public bool vimeo { get; set; }
        public bool custom { get; set; }
        public bool sticky_custom { get; set; }
    }
}
