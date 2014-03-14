using System;

namespace VimeoDotNet.Net
{
    public interface IApiRequest
    {
        string Host { get; set; }
        RestSharp.Method Method { get; set; }
        string Path { get; set; }
        int Port { get; set; }
        string Protocol { get; set; }
        string ApiVersion { get; set; }
        byte[] BinaryContent { get; set; }
        object Body { get; set; }
        string ResponseType { get; set; }
        bool ExcludeAuthorizationHeader { get; set; }
        System.Collections.Generic.IDictionary<string, string> Headers { get; }
        System.Collections.Generic.IDictionary<string, string> Query { get; }
        System.Collections.Generic.IDictionary<string, string> UrlSegments { get; }
        RestSharp.IRestResponse ExecuteRequest();
        RestSharp.IRestResponse<T> ExecuteRequest<T>() where T : new();
        System.Threading.Tasks.Task<RestSharp.IRestResponse> ExecuteRequestAsync();
        System.Threading.Tasks.Task<RestSharp.IRestResponse<T>> ExecuteRequestAsync<T>() where T : new();
    }
}
