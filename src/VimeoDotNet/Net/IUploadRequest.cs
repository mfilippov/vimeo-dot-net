using VimeoDotNet.Models;

namespace VimeoDotNet.Net
{
    /// <summary>
    /// IUploadRequest
    /// </summary>
    public interface IUploadRequest
    {
        /// <summary>
        /// Bytes written
        /// </summary>
        long BytesWritten { get; set; }
        /// <summary>
        /// Chunk size
        /// </summary>
        int ChunkSize { get; set; }
        /// <summary>
        /// Clip id
        /// </summary>
        long? ClipId { get; }
        /// <summary>
        /// Clip URI
        /// </summary>
        string ClipUri { get; set; }
        /// <summary>
        /// File
        /// </summary>
        IBinaryContent File { get; set; }
        /// <summary>
        /// File length
        /// </summary>
        long FileLength { get; }
        /// <summary>
        /// Is verified complete
        /// </summary>
        bool IsVerifiedComplete { get; set; }
        /// <summary>
        /// Ticket
        /// </summary>
        UploadTicket Ticket { get; set; }
    }
}