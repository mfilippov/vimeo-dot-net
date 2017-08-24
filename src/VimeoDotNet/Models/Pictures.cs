using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User pictures
    /// </summary>
    public class Pictures
    {
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
        
        /// <summary>
        /// Active
        /// </summary>
        public bool active { get; set; }
        
        /// <summary>
        /// Type
        /// </summary>
        public string type { get; set; }
        
        /// <summary>
        /// Sizes
        /// </summary>
        public List<Size> sizes { get; set; }
        
        /// <summary>
        /// Resources key
        /// </summary>
        public string resource_key { get; set; }
    }
}