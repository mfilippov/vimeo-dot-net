using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Web site
    /// </summary>
    [Serializable]
    public class Website
    {
        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }
    }
}