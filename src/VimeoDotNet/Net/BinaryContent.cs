using System;
using System.IO;
using System.Threading.Tasks;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Net
{
    /// <summary>
    /// Binary content
    /// </summary>
    [Serializable]
    public class BinaryContent : IDisposable, IBinaryContent
    {
        #region Private Fields

        private const int BUFFER_SIZE = 16384; //16k

        private bool disposeStream = true;

        [NonSerialized]
        private Stream _data;

        #endregion

        #region Properties

        /// <summary>
        /// Original file name
        /// </summary>
        public string OriginalFileName { get; set; }
        /// <summary>
        /// Content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public Stream Data
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Binary content
        /// </summary>
        public BinaryContent()
        {
        }

        /// <summary>
        /// Binary content
        /// </summary>
        /// <param name="filePath">FilePath</param>
        public BinaryContent(string filePath)
        {
            OriginalFileName = Path.GetFileName(filePath);
            ContentType = MimeHelpers.GetMimeMapping(OriginalFileName);
            Data = File.OpenRead(filePath);
        }

        /// <summary>
        /// Binary content
        /// </summary>
        /// <param name="data">Content</param>
        /// <param name="contentType">Content type</param>
        public BinaryContent(Stream data, string contentType)
        {
            ContentType = contentType;
            Data = data;
            disposeStream = false;
        }
        /// <summary>
        /// Binary content
        /// </summary>
        /// <param name="data">Content</param>
        /// <param name="contentType">Content type</param>
        public BinaryContent(byte[] data, string contentType)
        {
            ContentType = contentType;
            Data = new MemoryStream(data);
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Read bytes to byte array
        /// </summary>
        /// <param name="startIndex">Start index</param>
        /// <param name="endIndex">End index</param>
        /// <returns>Byte array</returns>
        public byte[] Read(int startIndex, int endIndex)
        {
            return ReadAsync(startIndex, endIndex).Result;
        }

        /// <summary>
        /// Read all bytes to array
        /// </summary>
        /// <returns>Byte array</returns>
        public byte[] ReadAll()
        {
            return ReadAllAsync().Result;
        }

        /// <summary>
        /// Read all bytes to byte array asynchronously
        /// </summary>
        /// <returns>Byte array</returns>
        public async Task<byte[]> ReadAllAsync()
        {
            VerifyCanRead(0);
            return await ReadDataStream(Data.Length);
        }

        /// <summary>
        /// Read bytes to byte array asynchronously
        /// </summary>
        /// <param name="startIndex">Start index</param>
        /// <param name="endIndex">End index</param>
        /// <returns>Byte array</returns>
        public async Task<byte[]> ReadAsync(long startIndex, long endIndex)
        {
            VerifyCanRead(startIndex);
            return await ReadDataStream(endIndex - startIndex);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Data != null && disposeStream)
            {
                Data.Dispose();
                Data = null;
            }
        }

        #endregion

        #region Helper Functions

        private void VerifyCanRead(long startIndex)
        {
            if (Data == null)
            {
                throw new InvalidOperationException("Content should be populated with a Stream");
            }
            if (!Data.CanRead)
            {
                throw new InvalidOperationException("Content should be a readable Stream");
            }
            if (Data.Position != startIndex)
            {
                if (!Data.CanSeek)
                {
                    throw new InvalidOperationException("Content cannot be advanced to the specified start index: " +
                                                        startIndex);
                }
                Data.Position = startIndex;
            }
        }

        private async Task<byte[]> ReadDataStream(long totalLength)
        {
            var buffer = new byte[BUFFER_SIZE];
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