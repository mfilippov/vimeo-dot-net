using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace VimeoDotNet.Net
{
    /// <summary>
    /// IApiRequest
    /// </summary>
    public interface IApiRequest
    {
        /// <summary>
        /// Host
        /// </summary>
        /// <value>The host.</value>
        string Host { get; set; }

        /// <summary>
        /// Method
        /// </summary>
        /// <value>The method.</value>
        HttpMethod Method { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        /// <value>The path.</value>
        string Path { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        /// <value>The port.</value>
        int Port { get; set; }

        /// <summary>
        /// Protocol
        /// </summary>
        /// <value>The protocol.</value>
        string Protocol { get; set; }

        /// <summary>
        /// Api version
        /// </summary>
        /// <value>The API version.</value>
        string ApiVersion { get; set; }

        /// <summary>
        /// Binary content
        /// </summary>
        /// <value>The content of the binary.</value>
        byte[] BinaryContent { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        /// <value>The body.</value>
        HttpContent Body { get; set; }

        /// <summary>
        /// Response type
        /// </summary>
        /// <value>The type of the response.</value>
        string ResponseType { get; set; }

        /// <summary>
        /// Exclude authorization header
        /// </summary>
        /// <value><c>true</c> if [exclude authorization header]; otherwise, <c>false</c>.</value>
        bool ExcludeAuthorizationHeader { get; set; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>The fields.</value>
        List<string> Fields { get; }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <value>The query.</value>
        IDictionary<string, string> Query { get; }

        /// <summary>
        /// Gets the URL segments.
        /// </summary>
        /// <value>The URL segments.</value>
        IDictionary<string, string> UrlSegments { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is add tus header.
        /// </summary>
        /// <value><c>true</c> if this instance is add tus header; otherwise, <c>false</c>.</value>
        bool IsAddTusHeader { get; set; }

        /// <summary>
        /// Execute request asynchronously
        /// </summary>
        /// <returns>Rest reponse</returns>
        Task<IApiResponse> ExecuteRequestAsync();

        /// <summary>
        /// Execute request asynchronously
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Rest reponse</returns>
        Task<IApiResponse<T>> ExecuteRequestAsync<T>() where T : new();
    }
}