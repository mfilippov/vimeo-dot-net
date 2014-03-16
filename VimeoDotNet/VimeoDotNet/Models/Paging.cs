using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Paging
    {
        public string next { get; set; }
        public string previous { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }
}