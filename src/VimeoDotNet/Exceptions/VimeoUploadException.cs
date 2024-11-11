using System;
using JetBrains.Annotations;
using VimeoDotNet.Net;

namespace VimeoDotNet.Exceptions
{
    /// <summary>
    /// Class VimeoUploadException.
    /// Implements the <see cref="VimeoDotNet.Exceptions.VimeoApiException" />
    /// </summary>
    /// <seealso cref="VimeoDotNet.Exceptions.VimeoApiException" />
    internal class VimeoUploadException : VimeoApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VimeoUploadException"/> class.
        /// </summary>
        /// <inheritdoc />
        [PublicAPI]
        public VimeoUploadException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VimeoUploadException"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        [PublicAPI]
        public VimeoUploadException(IUploadRequest request)
        {
            Request = request;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VimeoUploadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <inheritdoc />
        [PublicAPI]
        public VimeoUploadException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VimeoUploadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="request">The request.</param>
        [PublicAPI]
        public VimeoUploadException(string message, IUploadRequest request)
            : base(message)
        {
            Request = request;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VimeoUploadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <inheritdoc />
        [PublicAPI]
        public VimeoUploadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VimeoUploadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="request">The request.</param>
        /// <param name="innerException">The inner exception.</param>
        [PublicAPI]
        public VimeoUploadException(string message, IUploadRequest request, Exception innerException)
            : base(message, innerException)
        {
            Request = request;
        }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        /// <value>The request.</value>
        [PublicAPI]
        public IUploadRequest Request { get; set; }
    }
}