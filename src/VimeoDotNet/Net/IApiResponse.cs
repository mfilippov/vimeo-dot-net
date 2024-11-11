using System.Net;
using System.Net.Http.Headers;

namespace VimeoDotNet.Net
{
    /// <summary>
    /// Interface of REST response without payload type
    /// </summary>
    public interface IApiResponse
    {
        /// <summary>
        /// HTTP response status code
        /// </summary>
        /// <value>The status code.</value>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// HTTP response headers
        /// </summary>
        /// <value>The headers.</value>
        HttpResponseHeaders Headers { get; }

        /// <summary>
        /// Response text
        /// </summary>
        /// <value>The text.</value>
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
        /// <value>The content.</value>
        T Content { get; }
    }

    /// <summary>
    /// Class ApiResponse.
    /// Implements the <see cref="VimeoDotNet.Net.IApiResponse" />
    /// </summary>
    /// <seealso cref="VimeoDotNet.Net.IApiResponse" />
    internal class ApiResponse : IApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="text">The text.</param>
        public ApiResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, string text)
        {
            StatusCode = statusCode;
            Headers = headers;
            Text = text;
        }

        /// <summary>
        /// HTTP response status code
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; }
        /// <summary>
        /// HTTP response headers
        /// </summary>
        /// <value>The headers.</value>
        public HttpResponseHeaders Headers { get; }
        /// <summary>
        /// Response text
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; }
    }

    /// <summary>
    /// Class ApiResponse.
    /// Implements the <see cref="VimeoDotNet.Net.IApiResponse{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="VimeoDotNet.Net.IApiResponse{T}" />
    internal class ApiResponse<T> : IApiResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="text">The text.</param>
        /// <param name="content">The content.</param>
        public ApiResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, string text, T content)
        {
            StatusCode = statusCode;
            Headers = headers;
            Content = content;
            Text = text;
        }

        /// <summary>
        /// HTTP response status code
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; }
        /// <summary>
        /// HTTP response headers
        /// </summary>
        /// <value>The headers.</value>
        public HttpResponseHeaders Headers { get; }
        /// <summary>
        /// Response text
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; }
        /// <summary>
        /// Response payload
        /// </summary>
        /// <value>The content.</value>
        public T Content { get; }
    }
}