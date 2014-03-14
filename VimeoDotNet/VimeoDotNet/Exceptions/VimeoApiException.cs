using System;
using System.Runtime.Serialization;

namespace VimeoDotNet.Exceptions
{
    [Serializable]
    public class VimeoApiException : ApplicationException
    {
        public VimeoApiException()
            : base()
        {
        }

        public VimeoApiException(string message)
            : base(message)
        {
        }

        public VimeoApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public VimeoApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
