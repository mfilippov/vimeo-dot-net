using System.IO;
using System.Threading.Tasks;

namespace VimeoDotNet.Net
{
    /// <summary>
    /// IBinaryContent
    /// </summary>
    public interface IBinaryContent
    {
        /// <summary>
        /// Content type
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        Stream Data { get; }

        /// <summary>
        /// Original file name
        /// </summary>
        string OriginalFileName { get; set; }

        /// <summary>
        /// Read all bytes to byte array asynchronously
        /// </summary>
        /// <returns>Byte array</returns>
        Task<byte[]> ReadAllAsync();

        /// <summary>
        /// Read bytes to byte array asynchronously
        /// </summary>
        /// <param name="startIndex">Start index</param>
        /// <param name="endIndex">End index</param>
        /// <returns>Byte array</returns>
        Task<byte[]> ReadAsync(long startIndex, long endIndex);
    }
}