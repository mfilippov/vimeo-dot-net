using System;
using System.Runtime.Serialization;
using VimeoDotNet.Models;

namespace VimeoDotNet.Exceptions
{
    [Serializable]
    public class VimeoUploadException : VimeoApiException
    {
        public UploadRequest Request { get; private set; }

        public VimeoUploadException()
            : base()
        {
        }

        public VimeoUploadException(UploadRequest request)
            : base()
        {
            Request = request;
        }

        public VimeoUploadException(string message)
            : base(message)
        {
        }

        public VimeoUploadException(string message, UploadRequest request)
            : base(message)
        {
            Request = request;
        }

        public VimeoUploadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public VimeoUploadException(string message, UploadRequest request, Exception innerException)
            : base(message, innerException)
        {
            Request = request;
        }

        public VimeoUploadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                Request = info.GetValue("Request", typeof(UploadRequest)) as UploadRequest;
            }
        }
        
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);            
            if (info != null)
            {
                info.AddValue("Request", Request);
            }
        }
    }
}
