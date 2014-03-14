using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Privacy
    {
        public string view { get; set; }
        public string embed { get; set; }
        public bool download { get; set; }
        public bool add { get; set; }
    }
}
