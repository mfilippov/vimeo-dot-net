using System.Collections.Generic;
using System.IO;

namespace VimeoDotNet.Helpers
{
    internal static class MimeHelpers
    {
        private const string DEFAULT_CONTENT_TYPE = "application/octet-stream";

        private static readonly IDictionary<string, string> _mimeMappings = new Dictionary<string, string>
        {
            {".flv", "video/x-flv"},
            {".mp4", "video/mp4"},
            {".ts", "video/MP2T"},
            {".3gp", "video/3gpp"},
            {".mov", "video/quicktime"},
            {".divx", "video/avi"},
            {".avi", "video/avi"},
            {".wmv", "video/x-ms-wmv"},
            {".mpg", "video/mpeg"},
            {".mpeg", "video/mpeg"},
            {".ogg", "video/ogg"},
            {".ogv", "video/ogg"},
            {".ogx", "video/ogg"},
            {".webm", "video/webm"},
            {".mkv", "video/matroska"},
            {".mk3d", "video/matroska"}
        };

        public static string GetMimeMapping(string videoFileName)
        {
            if (string.IsNullOrEmpty(videoFileName))
            {
                return DEFAULT_CONTENT_TYPE;
            }
            if (!videoFileName.StartsWith("."))
            {
                videoFileName = Path.GetExtension(videoFileName);
            }
            string normalized = videoFileName.Trim().ToLower();
            if (!_mimeMappings.ContainsKey(normalized))
            {
                return DEFAULT_CONTENT_TYPE;
            }
            return _mimeMappings[normalized];
        }
    }
}