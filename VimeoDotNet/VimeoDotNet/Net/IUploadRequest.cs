using System;

namespace VimeoDotNet.Net
{
    public interface IUploadRequest
    {
        bool AllBytesWritten { get; }
        long BytesWritten { get; set; }
        int ChunkSize { get; set; }
        long? ClipId { get; }
        string ClipUri { get; set; }
        IBinaryContent File { get; set; }
        long FileLength { get; }
        bool IsVerifiedComplete { get; set; }
        VimeoDotNet.Models.UploadTicket Ticket { get; set; }
    }
}
