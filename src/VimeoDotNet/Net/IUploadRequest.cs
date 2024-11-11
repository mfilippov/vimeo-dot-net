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
        /// <value>The bytes written.</value>
        long BytesWritten { get; set; }

        /// <summary>
        /// Chunk size
        /// </summary>
        /// <value>The size of the chunk.</value>
        long ChunkSize { get; set; }

        /// <summary>
        /// Clip id
        /// </summary>
        /// <value>The clip identifier.</value>
        long? ClipId { get; }

        /// <summary>
        /// Clip URI
        /// </summary>
        /// <value>The clip URI.</value>
        string ClipUri { get; set; }

        /// <summary>
        /// File
        /// </summary>
        /// <value>The file.</value>
        IBinaryContent File { get; set; }

        /// <summary>
        /// File length
        /// </summary>
        /// <value>The length of the file.</value>
        long FileLength { get; }

        /// <summary>
        /// Is verified complete
        /// </summary>
        /// <value><c>true</c> if this instance is verified complete; otherwise, <c>false</c>.</value>
        bool IsVerifiedComplete { get; set; }

        /// <summary>
        /// Ticket
        /// </summary>
        /// <value>The ticket.</value>
        UploadTicket Ticket { get; set; }
    }
}