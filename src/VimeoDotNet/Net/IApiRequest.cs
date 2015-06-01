using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace VimeoDotNet.Net
{
    public interface IApiRequest
    {
        string Host { get; set; }
        Method Method { get; set; }
        string Path { get; set; }
        int Port { get; set; }
        string Protocol { get; set; }
        string ApiVersion { get; set; }
        byte[] BinaryContent { get; set; }
        object Body { get; set; }
        string ResponseType { get; set; }
        bool ExcludeAuthorizationHeader { get; set; }
        IDictionary<string, string> Headers { get; }
        IDictionary<string, string> Query { get; }
        IDictionary<string, string> UrlSegments { get; }
        IRestResponse ExecuteRequest();
        IRestResponse<T> ExecuteRequest<T>() where T : new();
        Task<IRestResponse> ExecuteRequestAsync();
        Task<IRestResponse<T>> ExecuteRequestAsync<T>() where T : new();
    }
}