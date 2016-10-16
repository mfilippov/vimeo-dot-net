using System;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Preset buttons
    /// </summary>
    [Serializable]
    public class PresetButtons
    {
        /// <summary>
        /// Like
        /// </summary>
        public bool like { get; set; }
        /// <summary>
        /// Watch later
        /// </summary>
        public bool watchlater { get; set; }
        /// <summary>
        /// Share
        /// </summary>
        public bool share { get; set; }
        /// <summary>
        /// Embed
        /// </summary>
        public bool embed { get; set; }
        /// <summary>
        /// Vote
        /// </summary>
        public bool vote { get; set; }
        /// <summary>
        /// HD
        /// </summary>
        public bool hd { get; set; }
    }
}