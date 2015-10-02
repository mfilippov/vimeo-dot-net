using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using VimeoDotNet.Constants;

namespace VimeoDotNet.Net
{
    public class ApiRequest : IApiRequest
    {
        #region Private Fields

        private readonly Dictionary<string, string> _hashValues = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _queryString = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _urlSegments = new Dictionary<string, string>();

        private string _path;

        #endregion

        #region Public Properties

        public string ApiVersion { get; set; }
        public string ResponseType { get; set; }
        public string Protocol { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public IDictionary<string, string> Headers
        {
            get { return _headers; }
        }

        public Method Method { get; set; }

        public string Path
        {
            get { return _path; }
            set
            {
                Uri parsed;
                if (Uri.TryCreate(value, UriKind.Absolute, out parsed))
                {
                    Protocol = parsed.Scheme;
                    Host = parsed.Host;
                    Path = parsed.PathAndQuery;
                    Port = parsed.Port;
                }
                else
                {
                    _path = value;
                }
            }
        }

        public object Body { get; set; }

        public IDictionary<string, string> Query
        {
            get { return _queryString; }
        }

        public IDictionary<string, string> UrlSegments
        {
            get { return _urlSegments; }
        }

        public byte[] BinaryContent { get; set; }
        public bool ExcludeAuthorizationHeader { get; set; }

        #endregion

        #region Private Properties

        protected IRestClient Client { get; private set; }
        protected string ClientId { get; set; }
        protected string ClientSecret { get; set; }
        protected string AccessToken { get; set; }

        #endregion

        #region Constructors

        public ApiRequest()
        {
            Protocol = Request.DefaultProtocol;
            Host = Request.DefaultHostName;
            Port = Request.DefaultHttpsPort;
            Method = Request.DefaultMethod;
            ResponseType = ResponseTypes.Wildcard;
            ApiVersion = ApiVersions.v3_2;
            ExcludeAuthorizationHeader = false;
        }

        public ApiRequest(string clientId, string clientSecret)
            : this()
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public ApiRequest(string accessToken)
            : this()
        {
            AccessToken = accessToken;
        }

        #endregion

        #region Public Methods

        public IRestResponse ExecuteRequest()
        {
            IRestRequest request = PrepareRequest();
            IRestClient client = PrepareClient();

            IRestResponse response = client.Execute(request);
            return response;
        }

        public async Task<IRestResponse> ExecuteRequestAsync()
        {
            return await GetAsyncRequestAwaiter();
        }

        public IRestResponse<T> ExecuteRequest<T>() where T : new()
        {
            IRestRequest request = PrepareRequest();
            IRestClient client = PrepareClient();

            IRestResponse<T> response = client.Execute<T>(request);
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteRequestAsync<T>() where T : new()
        {
            return await GetAsyncRequestAwaiter<T>();
        }

        #endregion

        #region Private Methods

        #region Async

        protected Task<IRestResponse> GetAsyncRequestAwaiter()
        {
            IRestRequest request = PrepareRequest();
            IRestClient client = PrepareClient();

            var tcs = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, response => tcs.SetResult(response));

            return tcs.Task;
        }

        protected Task<IRestResponse<T>> GetAsyncRequestAwaiter<T>() where T : new()
        {
            IRestRequest request = PrepareRequest();
            IRestClient client = PrepareClient();

            var tcs = new TaskCompletionSource<IRestResponse<T>>();
            client.ExecuteAsync<T>(request, response => tcs.SetResult(response));

            return tcs.Task;
        }

        #endregion

        protected IRestClient PrepareClient()
        {
            IRestClient client = GetClient();
            client.Authenticator = GetAuthenticator();

            return client;
        }

        protected IRestRequest PrepareRequest()
        {
            SetDefaults();
            IRestClient client = GetClient();
            if (!ExcludeAuthorizationHeader)
            {
                client.Authenticator = GetAuthenticator();
            }

            var request = new RestRequest(Path, Method);
            SetHeaders(request);
            SetUrlSegments(request);
            SetQueryParams(request);
            SetBody(request);
            SetJsonResponse(request);

            return request;
        }

        protected void SetJsonResponse(IRestRequest request)
        {
            request.DateFormat = "yyyy-MM-ddTHH:mm:sszzz";
            request.OnBeforeDeserialization += (IRestResponse response) =>
            {
                // Switch response content type to allow deserializer to work properly.
                response.ContentType = "application/json";
            };
        }

        protected IAuthenticator GetAuthenticator()
        {
            if (!string.IsNullOrWhiteSpace(AccessToken))
            {
                return GetBearerAuthenticator();
            }
            if (!string.IsNullOrWhiteSpace(ClientId) && !string.IsNullOrWhiteSpace(ClientSecret))
            {
                return GetBasicAuthenticator();
            }
            return null;
        }

        protected IAuthenticator GetBearerAuthenticator()
        {
            return new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer");
        }

        protected IAuthenticator GetBasicAuthenticator()
        {
            string token = string.Format("{0}:{1}", ClientId, ClientSecret);
            byte[] tokenBytes = Encoding.ASCII.GetBytes(token);
            string encoded = Convert.ToBase64String(tokenBytes);

            return new OAuth2AuthorizationRequestHeaderAuthenticator(encoded, "Basic");
        }

        protected IRestClient GetClient()
        {
            string baseUrl = GetBaseUrl();
            if (Client == null || Client.BaseUrl.AbsoluteUri != baseUrl)
            {
                Client = new RestClient(baseUrl);
            }
            return Client;
        }

        protected void SetUrlSegments(IRestRequest request)
        {
            foreach (var segment in UrlSegments)
            {
                request.AddUrlSegment(segment.Key, segment.Value);
            }
        }

        protected void SetHeaders(IRestRequest request)
        {
            if (!Headers.ContainsKey(Request.HeaderContentType) &&
                (Method == Method.POST || Method == Method.PATCH || Method == Method.PUT))
            {
                Headers[Request.HeaderContentType] = "application/x-www-form-urlencoded";
            }
            if (!Headers.ContainsKey(Request.HeaderAccepts))
            {
                Headers.Add(Request.HeaderAccepts, BuildAcceptsHeader());
            }
            foreach (var header in Headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
        }

        protected void SetQueryParams(IRestRequest request)
        {
            foreach (var qsParam in Query)
            {
                request.AddParameter(qsParam.Key, qsParam.Value);
            }
        }

        protected void SetBody(IRestRequest request)
        {
            if (Body != null)
            {
                request.AddBody(Body);
                request.RequestFormat = DataFormat.Json;
            }
            else if (BinaryContent != null)
            {
                request.AddParameter(string.Empty, BinaryContent, ParameterType.RequestBody);
            }
        }

        protected void SetDefaults()
        {
            Protocol = string.IsNullOrWhiteSpace(Protocol) ? Request.DefaultProtocol : Protocol;
            Host = string.IsNullOrWhiteSpace(Host) ? Request.DefaultHostName : Host;
            ResponseType = string.IsNullOrWhiteSpace(ResponseType) ? ResponseTypes.Wildcard : ResponseType;
            ApiVersion = string.IsNullOrWhiteSpace(ApiVersion) ? ApiVersions.v3_2 : ApiVersion;

            Protocol = Protocol.ToLower();
            Host = Host.ToLower();

            Port = Port > 0 ? Port : GetDefaultPort(Protocol);
        }

        protected string BuildAcceptsHeader()
        {
            return string.Format("{0};{1}", ResponseType, ApiVersion);
        }

        protected int GetDefaultPort(string protocol)
        {
            if (Protocol == Request.ProtocolHttps)
            {
                return Request.DefaultHttpsPort;
            }
            return Request.DefaultHttpPort;
        }

        protected string GetBaseUrl()
        {
            string url = Protocol.ToLower() + "://";

            if (Host.EndsWith("/"))
            {
                Host = Host.Substring(0, Host.Length - 1);
            }
            url += Host;

            if (Port != GetDefaultPort(Protocol))
            {
                url += ":" + Port;
            }

            return url;
        }

        #endregion
    }
}