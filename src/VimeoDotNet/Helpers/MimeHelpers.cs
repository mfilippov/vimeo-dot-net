using System.Collections.Generic;
using System.IO;

namespace VimeoDotNet.Helpers
{
    /// <summary>
    /// Class MimeHelpers.
    /// </summary>
    internal static class MimeHelpers
    {
        /// <summary>
        /// The default content type
        /// </summary>
        private const string DefaultContentType = "application/octet-stream";

        /// <summary>
        /// The MIME mappings
        /// </summary>
        private static readonly IDictionary<string, string> MimeMappings = new Dictionary<string, string>
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

        /// <summary>
        /// Gets the MIME mapping.
        /// </summary>
        /// <param name="videoFileName">Name of the video file.</param>
        /// <returns>System.String.</returns>
        public static string GetMimeMapping(string videoFileName)
        {
            if (string.IsNullOrEmpty(videoFileName))
            {
                return DefaultContentType;
            }

            if (!videoFileName.StartsWith("."))
            {
                videoFileName = Path.GetExtension(videoFileName);
            }

            var normalized = videoFileName.Trim().ToLower();
            return !MimeMappings.ContainsKey(normalized) ? DefaultContentType : MimeMappings[normalized];
        }
    }
}