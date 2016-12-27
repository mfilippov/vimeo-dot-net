using System;
using System.Runtime.Serialization;

namespace VimeoDotNet.Exceptions
{
    /// <summary>
    /// VimeoApiException
    /// </summary>
    [Serializable]
    public class VimeoApiException : ApplicationException
    {
        /// <summary>
        /// Create new VimeoApiException
        /// </summary>
        public VimeoApiException()
        {
        }

        /// <summary>
        /// Create new VimeoApiException with message
        /// </summary>
        /// <param name="message">Message</param>
        public VimeoApiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create new VimeoApiException with message and inner exception
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException"></param>
        public VimeoApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Create new VimeoApiException
        /// </summary>
        public VimeoApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}