using System;

namespace VimeoDotNet.Exceptions
{
    /// <summary>
    /// VimeoApiException
    /// </summary>
    public class VimeoApiException : Exception
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
    }
}