using System;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    public class Paginated<T> where T : class
    {
        public List<T> data { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        public Paging paging { get; set; }
    }
}
