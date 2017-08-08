using System.Net;
using System.Net.Http.Headers;

namespace VimeoDotNet.Net
{

    /// <summary>
    ///  Interface of REST response without payload type
    /// </summary>
    public interface IApiResponse
    {
        /// <summary>HTTP response status code</summary>
        HttpStatusCode StatusCode { get; }
        
        /// <summary>
        /// HTTP response headers
        /// </summary>
        HttpResponseHeaders Headers { get; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Interface of REST response with payload type
    /// </summary>
    public interface IApiResponse<out T> : IApiResponse
    {
        /// <summary>
        /// Response payload
        /// </summary>
        T Content { get; }
    }
    
    internal class ApiResponse : IApiResponse
    {
        public ApiResponse(HttpStatusCode statusCode, HttpResponseHeaders headers)
        {
            StatusCode = statusCode;
            Headers = headers;
        }
        
        public HttpStatusCode StatusCode { get; }
        public HttpResponseHeaders Headers { get; }
    }
    
    internal class ApiResponse<T> : IApiResponse<T>
    {
        public ApiResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, T content)
        {
            StatusCode = statusCode;
            Headers = headers;
            Content = content;
        }

        public HttpStatusCode StatusCode { get; }
        public HttpResponseHeaders Headers { get; }
        public T Content { get; }
    }
}