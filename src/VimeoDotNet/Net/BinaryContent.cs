using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Net
{
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// Binary content
    /// </summary>
    [Serializable]
    public class BinaryContent : IDisposable, IBinaryContent
    {
        #region Private Fields

        private const int BufferSize = 16384; //16k

        private readonly bool _shouldDisposeStream = true;
        private bool _disposed;

        [NonSerialized] private Stream _data;

        #endregion

        #region Properties

        /// <inheritdoc />
        public string OriginalFileName { get; set; }

        /// <inheritdoc />
        public string ContentType { get; set; }

        /// <inheritdoc />
        [NotNull]
        public Stream Data
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException("BinaryContent");
                return _data;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Binary content
        /// </summary>
        /// <param name="filePath">FilePath</param>
        [PublicAPI]
        public BinaryContent(string filePath)
        {
            OriginalFileName = Path.GetFileName(filePath);
            ContentType = MimeHelpers.GetMimeMapping(OriginalFileName);
            _data = File.OpenRead(filePath);
        }

        /// <summary>
        /// Binary content
        /// </summary>
        /// <param name="data">Content</param>
        /// <param name="contentType">Content type</param>
        public BinaryContent([NotNull] Stream data, string contentType)
        {
            ContentType = contentType;
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _shouldDisposeStream = false;
        }

        /// <summary>
        /// Binary content
        /// </summary>
        /// <param name="data">Content</param>
        /// <param name="contentType">Content type</param>
        public BinaryContent(byte[] data, string contentType)
        {
            ContentType = contentType;
            _data = new MemoryStream(data);
        }

        #endregion

        #region Public Functions

        /// <inheritdoc />
        public async Task<byte[]> ReadAllAsync()
        {
            VerifyCanRead(0);
            return await ReadDataStream(0, Data.Length).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<byte[]> ReadAsync(long startIndex, long endIndex)
        {
            VerifyCanRead(startIndex);
            return await ReadDataStream(startIndex, endIndex - startIndex).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException("BinaryContent");
            if (_shouldDisposeStream)
            {
                _data.Dispose();
                _data = null;
            }

            _disposed = true;
        }

        #endregion

        #region Helper Functions

        private void VerifyCanRead(long startIndex)
        {
            if (_disposed)
                throw new ObjectDisposedException("BinaryContent");
            if (!Data.CanRead)
            {
                throw new InvalidOperationException("Content should be a readable Stream");
            }

            if (Data.Position == startIndex) return;
            if (!Data.CanSeek)
            {
                throw new InvalidOperationException("Content cannot be advanced to the specified start index: " +
                                                    startIndex);
            }

            Data.Position = startIndex;
        }

        private async Task<byte[]> ReadDataStream(long startIndex, long length)
        {
            var buffer = new byte[BufferSize];
            var totalRead = 0;
            using (var ms = new MemoryStream())
            {
                if (startIndex != 0)
                    Data.Seek(startIndex, SeekOrigin.Begin);
                while (totalRead < length)
                {
                    var read = await Data.ReadAsync(buffer, 0,
                        length - totalRead > buffer.Length ? buffer.Length : (int) (length - totalRead))
                        .ConfigureAwait(false);
                    totalRead += read;
                    await ms.WriteAsync(buffer, 0, read).ConfigureAwait(false);
                }

                return ms.ToArray();
            }
        }

        #endregion
    }
}