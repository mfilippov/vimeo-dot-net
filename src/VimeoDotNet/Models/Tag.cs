using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Tag
    {
        public string name { get; set; }
        public string canonical { get; set; }
    }
}