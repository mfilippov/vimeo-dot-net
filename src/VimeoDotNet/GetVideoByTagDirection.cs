using System;
using JetBrains.Annotations;

namespace VimeoDotNet
{
    /// <summary>
    /// Get video by tag direction type
    /// </summary>
    [PublicAPI]
    public enum GetVideoByTagDirection
    {
        /// <summary>
        /// Ascending
        /// </summary>
        Asc,
        /// <summary>
        /// Descending
        /// </summary>
        Desc
    }

    /// <summary>
    /// Get video by tag direction type
    /// </summary>
    internal static class GetVideoByTagDirectionEx
    {
        /// <summary>
        /// Return string representation for enum value
        /// </summary>
        /// <param name="direction">Value</param>
        /// <returns>String representation</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throw if value not handled.</exception>
        public static string GetStringValue(this GetVideoByTagDirection direction)
        {
            switch (direction)
            {
                case GetVideoByTagDirection.Asc:
                    return "asc";
                case GetVideoByTagDirection.Desc:
                    return "desc";
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}