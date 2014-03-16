using System;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class UserConnections
    {
        public string activities { get; set; }
        public string albums { get; set; }
        public string channels { get; set; }
        public string feed { get; set; }
        public string followers { get; set; }
        public string following { get; set; }
        public string groups { get; set; }
        public string likes { get; set; }
        public string portfolios { get; set; }
        public string videos { get; set; }
        public string watchlater { get; set; }
    }
}