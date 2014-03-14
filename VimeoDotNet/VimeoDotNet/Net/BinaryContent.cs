using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Net
{
    [Serializable]
    public class BinaryContent : IDisposable, IBinaryContent
    {
        #region Private Fields

        private const int BUFFER_SIZE = 16384; //16k

        [NonSerialized]
        private Stream _data;

        #endregion

        #region Properties

        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public Stream Data
        {
            get { return _data; }
            set { _data = value; } 
        }

        #endregion

        #region Constructors

        public BinaryContent()
        {
        }

        public BinaryContent(string filePath)
        {
            OriginalFileName = Path.GetFileName(filePath);
            ContentType = MimeHelpers.GetMimeMapping(OriginalFileName);
            Data = File.OpenRead(filePath);
        }

        public BinaryContent(Stream data, string contentType)
        {
            ContentType = contentType;
            Data = data;
        }

        public BinaryContent(byte[] data, string contentType)
        {
            ContentType = contentType;
            Data = new MemoryStream(data);
        }

        #endregion

        #region Public Functions

        public byte[] Read(int startIndex, int endIndex)
        {
            return ReadAsync(startIndex, endIndex).Result;
        }

        public byte[] ReadAll()
        {
            return ReadAllAsync().Result;
        }

        public async Task<byte[]> ReadAllAsync()
        {
            VerifyCanRead(0);
            return await ReadDataStream(Data.Length);
        }

        public async Task<byte[]> ReadAsync(long startIndex, long endIndex)
        {
            VerifyCanRead(startIndex);
            return await ReadDataStream(endIndex - startIndex);
        }

        public void Dispose()
        {
            if (Data != null)
            {
                Data.Dispose();
                Data = null;
            }
        }

        #endregion

        #region Helper Functions

        private void VerifyCanRead(long startIndex)
        {
            if (Data == null) { throw new InvalidOperationException("Data should be populated with a Stream"); }
            if (!Data.CanRead) { throw new InvalidOperationException("Data should be a readable Stream"); }
            if (Data.Position != startIndex)
            {
                if (!Data.CanSeek) { throw new InvalidOperationException("Data cannot be advanced to the specified start index: " + startIndex); }
                Data.Position = startIndex;
            }
        }

        private async Task<byte[]> ReadDataStream(long totalLength)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            int read = 0;
            int totalRead = 0;
            using (var ms = new MemoryStream())
            {
                while (totalRead < totalLength)
                {
                    read = await Data.ReadAsync(buffer, 0, buffer.Length);
                    totalRead += read;
                    await ms.WriteAsync(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        #endregion
    }
}
