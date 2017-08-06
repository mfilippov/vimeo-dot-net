using System.Collections.Generic;
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
        /// Reponse content
        /// </summary>
        string Content { get; }
        
        /// <summary>
        /// HTTP response headers
        /// </summary>
        HttpResponseHeaders Headers { get; }
    }

    /// <summary>
    /// Interface of REST response with payload type
    /// </summary>
    public interface IApiResponse<T> : IApiResponse
    {
        /// <summary>
        /// Response payload
        /// </summary>
        T Data { get; }
    }
    
    internal class ApiResponse : IApiResponse
    {
        public ApiResponse(HttpStatusCode statusCode, string content, HttpResponseHeaders headers)
        {
            StatusCode = statusCode;
            Content = content;
            Headers = headers;
        }
        
        public HttpStatusCode StatusCode { get; }
        public string Content { get; }
        public HttpResponseHeaders Headers { get; }
    }
    
    internal class ApiResponse<T> : IApiResponse<T>
    {
        public ApiResponse(HttpStatusCode statusCode, string content, HttpResponseHeaders headers, T data)
        {
            StatusCode = statusCode;
            Content = content;
            Headers = headers;
            Data = data;
        }

        public HttpStatusCode StatusCode { get; }
        public string Content { get; }
        public HttpResponseHeaders Headers { get; }
        public T Data { get; }
    }
}