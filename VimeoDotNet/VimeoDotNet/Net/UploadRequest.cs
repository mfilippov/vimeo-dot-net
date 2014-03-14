using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Helpers;
using VimeoDotNet.Models;

namespace VimeoDotNet.Net
{
    [Serializable]
    public class UploadRequest : VimeoDotNet.Net.IUploadRequest
    {
        #region Private Fields

        private IBinaryContent _file;
        private long _fileLength;

        #endregion

        #region Public Properties

        public UploadTicket Ticket { get; set; }
        public int ChunkSize { get; set; }
        public long BytesWritten { get; set; }
        public bool IsVerifiedComplete { get; set; }
        public string ClipUri { get; set; }

        public IBinaryContent File
        {
            get { return _file; }
            set {
                _file = value;
                _fileLength = 0;
                if (_file != null && _file.Data != null) {
                    _fileLength = _file.Data.Length;
                }
            }
        }

        public long FileLength
        {
            get
            {
                return _fileLength;
            }
        }

        public bool AllBytesWritten
        {
            get
            {
                if (File == null || File.Data == null) { return false; }
                return BytesWritten == FileLength;
            }
        }

        public long? ClipId
        {
            get
            {
                return ModelHelpers.ParseModelUriId(ClipUri);
            }
        }

        #endregion
    }
}
