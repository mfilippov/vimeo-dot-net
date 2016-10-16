using System;
using VimeoDotNet.Helpers;
using VimeoDotNet.Models;

namespace VimeoDotNet.Net
{
    /// <summary>
    /// Upload request
    /// </summary>
    [Serializable]
    public class UploadRequest : IUploadRequest
    {
        #region Private Fields

        private IBinaryContent _file;
        private long _fileLength;

        #endregion

        #region Public Properties

        /// <summary>
        /// Ticket
        /// </summary>
        public UploadTicket Ticket { get; set; }

        /// <summary>
        /// Chunk size
        /// </summary>
        public int ChunkSize { get; set; }

        /// <summary>
        /// Bytes written
        /// </summary>
        public long BytesWritten { get; set; }

        /// <summary>
        /// Is verified complete
        /// </summary>
        public bool IsVerifiedComplete { get; set; }

        /// <summary>
        /// Clip URI
        /// </summary>
        public string ClipUri { get; set; }

        /// <summary>
        /// File
        /// </summary>
        public IBinaryContent File
        {
            get { return _file; }
            set
            {
                _file = value;
                _fileLength = 0;
                if (_file != null && _file.Data != null)
                {
                    _fileLength = _file.Data.Length;
                }
            }
        }

        /// <summary>
        /// File length
        /// </summary>
        public long FileLength
        {
            get { return _fileLength; }
        }

        /// <summary>
        /// All bytes written
        /// </summary>
        public bool AllBytesWritten
        {
            get
            {
                return FileLength > 0 && BytesWritten == FileLength;
            }
        }

        /// <summary>
        /// Clip id
        /// </summary>
        public long? ClipId
        {
            get { return ModelHelpers.ParseModelUriId(ClipUri); }
        }

        #endregion
    }
}