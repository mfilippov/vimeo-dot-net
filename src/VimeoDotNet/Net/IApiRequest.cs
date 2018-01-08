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
        string Host { get; set; }

        /// <summary>
        /// Method
        /// </summary>
        HttpMethod Method { get; set; }

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
        HttpContent Body { get; set; }

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
        List<string> Fields { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<string, string> Query { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<string, string> UrlSegments { get; }

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