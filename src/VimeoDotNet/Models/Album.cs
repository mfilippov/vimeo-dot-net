using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Album model
    /// </summary>
    [Serializable]
    public class Album
    {
        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public User user { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        public int duration { get; set; }

        /// <summary>
        /// CreatedTime
        /// </summary>
        public string created_time { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        public List<Picture> pictures { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        public Privacy privacy { get; set; }

        /// <summary>
        /// Stats
        /// </summary>
        public AlbumStats stats { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        public AlbumMetadata metadata { get; set; }

		/// <summary>
		/// Return album id if exists
		/// </summary>
		/// <returns>AlbumId or null</returns>
		public long? GetAlbumId()
		{
			if (String.IsNullOrEmpty(uri)) { return null; }

			var match = regexAlbumUri.Match(uri);
			if (match.Success)
			{
				return long.Parse(match.Groups["albumId"].Value);
			}

			return null;
		}

		private static readonly Regex regexAlbumUri = new Regex(@"/albums/(?<albumId>\d+)/?$");
    }
}