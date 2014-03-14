using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class PresetButtons
    {
        public bool like { get; set; }
        public bool watchlater { get; set; }
        public bool share { get; set; }
        public bool embed { get; set; }
        public bool vote { get; set; }
        public bool hd { get; set; }
    }
}
