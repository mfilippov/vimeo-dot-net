using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

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
        string Host { get; set; }
        /// <summary>
        /// Method
        /// </summary>
        Method Method { get; set; }
        /// <summary>
        /// Path
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        int Port { get; set; }
        /// <summary>
        /// Protocol
        /// </summary>
        string Protocol { get; set; }
        /// <summary>
        /// Api version
        /// </summary>
        string ApiVersion { get; set; }
        /// <summary>
        /// Binary content
        /// </summary>
        byte[] BinaryContent { get; set; }
        /// <summary>
        /// Body
        /// </summary>
        object Body { get; set; }
        /// <summary>
        /// Response type
        /// </summary>
        string ResponseType { get; set; }
        /// <summary>
        /// Exclude authorization header
        /// </summary>
        bool ExcludeAuthorizationHeader { get; set; }
        /// <summary>
        ///
        /// </summary>
        IDictionary<string, string> Headers { get; }
        /// <summary>
        ///
        /// </summary>
        IDictionary<string, string> Query { get; }
        /// <summary>
        ///
        /// </summary>
        IDictionary<string, string> UrlSegments { get; }
        /// <summary>
        /// Execute request
        /// </summary>
        /// <returns>Rest repons</returns>
        IRestResponse ExecuteRequest();
        /// <summary>
        /// Execute request
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Rest repons</returns>
        IRestResponse<T> ExecuteRequest<T>() where T : new();
        /// <summary>
        /// Execute request asynchronously
        /// </summary>
        /// <returns>Rest reponse</returns>
        Task<IRestResponse> ExecuteRequestAsync();
        /// <summary>
        /// Execute request asynchronously
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Rest repons</returns>
        Task<IRestResponse<T>> ExecuteRequestAsync<T>() where T : new();
    }
}