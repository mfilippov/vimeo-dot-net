namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album connections entry
    /// </summary>
    public class AlbumConnectionsEntry
    {
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
        
        /// <summary>
        /// Options
        /// </summary>
        public string[] options { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public int total { get; set; }
    }
}