using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Album
    {
        public string uri { get; set; }
        public User user { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public int duration { get; set; }
        public string created_time { get; set; }
        public List<Picture> pictures { get; set; }
        public Privacy privacy { get; set; }
        public AlbumStats stats { get; set; }
        public AlbumMetadata metadata { get; set; }

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

		private static Regex regexAlbumUri = new Regex(@"/albums/(?<albumId>\d+)/?$");
    }
}