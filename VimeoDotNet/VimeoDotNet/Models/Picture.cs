using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models {
    [Serializable]
    public class Picture {
        public bool active { get; set; }
        public string uri { get; set; }
        public List<Size> sizes { get; set; }
    }
}