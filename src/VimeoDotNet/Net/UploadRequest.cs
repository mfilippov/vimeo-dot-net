using VimeoDotNet.Helpers;
using VimeoDotNet.Models;

namespace VimeoDotNet.Net
{
    /// <inheritdoc />
    public class UploadRequest : IUploadRequest
    {
        #region Private Fields

        private IBinaryContent _file;

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public UploadTicket Ticket { get; set; }

        /// <inheritdoc />
        public int ChunkSize { get; set; }

        /// <inheritdoc />
        public long BytesWritten { get; set; }

        /// <inheritdoc />
        public bool IsVerifiedComplete { get; set; }

        /// <inheritdoc />
        public string ClipUri { get; set; }

        /// <inheritdoc />
        public IBinaryContent File
        {
            get => _file;
            set
            {
                _file = value;
                FileLength = 0;
                if (_file?.Data != null)
                {
                    FileLength = _file.Data.Length;
                }
            }
        }

        /// <inheritdoc />
        public long FileLength { get; private set; }

        /// <inheritdoc />
        public long? ClipId => ModelHelpers.ParseModelUriId(ClipUri);

        #endregion
    }
}