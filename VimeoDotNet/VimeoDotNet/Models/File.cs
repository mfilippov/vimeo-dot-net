using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class File
    {
        public string quality { get; set; }
        public string type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public DateTime expires { get; set; }
        public string link { get; set; }
    }
}
