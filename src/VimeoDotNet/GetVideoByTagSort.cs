using System;
using JetBrains.Annotations;

namespace VimeoDotNet
{
    /// <summary>
    /// Get video by tag sort type
    /// </summary>
    [PublicAPI]
    public enum GetVideoByTagSort
    {
        /// <summary>
        /// By created time
        /// </summary>
        CreatedTime,

        /// <summary>
        /// By duration
        /// </summary>
        Duration,

        /// <summary>
        /// By name
        /// </summary>
        Name
    }

    /// <summary>
    /// Class GetVideoByTagSortEx.
    /// </summary>
    internal static class GetVideoByTagSortEx
    {
        /// <summary>
        /// Return string representation for enum value
        /// </summary>
        /// <param name="sort">Value</param>
        /// <returns>String representation</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">sort - null</exception>
        public static string GetStringValue(this GetVideoByTagSort sort)
        {
            switch (sort)
            {
                case GetVideoByTagSort.CreatedTime:
                    return "created_time";
                case GetVideoByTagSort.Duration:
                    return "duration";
                case GetVideoByTagSort.Name:
                    return "name";
                default:
                    throw new ArgumentOutOfRangeException(nameof(sort), sort, null);
            }
        }
    }
}