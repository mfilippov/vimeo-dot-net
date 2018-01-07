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

        /// <summary>
        /// Response text
        /// </summary>
        string Text { get; }
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
        public ApiResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, string text)
        {
            StatusCode = statusCode;
            Headers = headers;
            Text = text;
        }

        public HttpStatusCode StatusCode { get; }
        public HttpResponseHeaders Headers { get; }
        public string Text { get; }
    }

    internal class ApiResponse<T> : IApiResponse<T>
    {
        public ApiResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, string text, T content)
        {
            StatusCode = statusCode;
            Headers = headers;
            Content = content;
            Text = text;
        }

        public HttpStatusCode StatusCode { get; }
        public HttpResponseHeaders Headers { get; }
        public string Text { get; }
        public T Content { get; }
    }
}