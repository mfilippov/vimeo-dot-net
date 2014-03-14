using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VimeoDotNet.Constants;

namespace VimeoDotNet.Net
{
    public class ApiRequest
    {
        #region Private Fields

        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _urlSegments = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _queryString = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _hashValues = new Dictionary<string, string>();

        private string _path;

        #endregion

        #region Public Properties

        public RestClient Client { get; private set; }

        public string ApiVersion { get; set; }
        public string ResponseType { get; set; }
        public string Protocol { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public IDictionary<string, string> Headers { get { return _headers; } }
        public Method Method { get; set; }
        public string Path {
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
        public IDictionary<string, string> Query { get { return _queryString; } }
        public IDictionary<string, string> UrlSegments { get { return _urlSegments; } }
        public byte[] BinaryContent { get; set; }
        public bool ExcludeAuthorizationHeader { get; set; }

        #endregion

        #region Private Properties

        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string AccessToken { get; set; }

        #endregion

        #region Constructors

        public ApiRequest()
        {
            Protocol = Request.DefaultProtocol;
            Host = Request.DefaultHostName;
            Port = Request.DefaultHttpsPort;
            Method = Request.DefaultMethod;
            ResponseType = ResponseTypes.Wildcard;
            ApiVersion = ApiVersions.v3_0;
            ExcludeAuthorizationHeader = false;
        }

        public ApiRequest(string clientId, string clientSecret)
            :this()
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public ApiRequest(string accessToken)
            :this()
        {
            AccessToken = accessToken;
        }

        #endregion

        #region Public Methods

        public IRestResponse ExecuteRequest()
        {
            var request = PrepareRequest();
            var client = PrepareClient();

            var response = client.Execute(request);
            return response;
        }

        public async Task<IRestResponse> ExecuteRequestAsync()
        {
            return await GetAsyncRequestAwaiter();
        }

        public IRestResponse<T> ExecuteRequest<T>() where T : new()
        {
            var request = PrepareRequest();
            var client = PrepareClient();

            var response = client.Execute<T>(request);
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteRequestAsync<T>() where T : new()
        {
            return await GetAsyncRequestAwaiter<T>();
        }

        #endregion

        #region Private Methods

        #region Async

        public Task<IRestResponse> GetAsyncRequestAwaiter()
        {
            var request = PrepareRequest();
            var client = PrepareClient();

            var tcs = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, response => tcs.SetResult(response));

            return tcs.Task;
        }

        public Task<IRestResponse<T>> GetAsyncRequestAwaiter<T>() where T : new()
        {
            var request = PrepareRequest();
            var client = PrepareClient();

            var tcs = new TaskCompletionSource<IRestResponse<T>>();
            client.ExecuteAsync<T>(request, response => tcs.SetResult(response));

            return tcs.Task;
        }

        #endregion

        private RestClient PrepareClient() {
            var client = GetClient();
            client.Authenticator = GetAuthenticator();

            return client;
        }

        private RestRequest PrepareRequest() {
            SetDefaults();
            var client = GetClient();
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

        private void SetJsonResponse(IRestRequest request) {
            request.OnBeforeDeserialization += (IRestResponse response) =>
            {
                // Switch response content type to allow deserializer to work properly.
                response.ContentType = "application/json";
            };
        }

        private IAuthenticator GetAuthenticator() {
            if (!string.IsNullOrWhiteSpace(AccessToken)) {
                return GetBearerAuthenticator();
            }
            else if (!string.IsNullOrWhiteSpace(ClientId) && !string.IsNullOrWhiteSpace(ClientSecret)) {
                return GetBasicAuthenticator();
            }
            return null;
        }

        private IAuthenticator GetBearerAuthenticator() {
            return new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer");
        }

        private IAuthenticator GetBasicAuthenticator() {
            string token = string.Format("{0}:{1}", ClientId, ClientSecret);
            byte[] tokenBytes = System.Text.Encoding.ASCII.GetBytes(token);
            string encoded = Convert.ToBase64String(tokenBytes);
            
            return new OAuth2AuthorizationRequestHeaderAuthenticator(encoded, "Basic");
        }

        private RestClient GetClient() {
            var baseUrl = GetBaseUrl();
            if (Client == null || Client.BaseUrl != baseUrl)
            {
                Client = new RestClient(baseUrl);
            }
            return Client;
        }

        private void SetUrlSegments(RestRequest request)
        {
            foreach (var segment in UrlSegments)
            {
                request.AddUrlSegment(segment.Key, segment.Value);
            }
        }

        private void SetHeaders(RestRequest request) {
            if (!Headers.ContainsKey(Request.HeaderContentType) && (Method == Method.POST || Method == Method.PATCH || Method == Method.PUT)) {
                Headers[Request.HeaderContentType] = "application/x-www-form-urlencoded";
            }
            if (!Headers.ContainsKey(Request.HeaderAccepts)) {
                Headers.Add(Request.HeaderAccepts, BuildAcceptsHeader());
            }
            foreach(var header in Headers) {
                request.AddHeader(header.Key, header.Value);
            }
        }

        private void SetQueryParams(RestRequest request) {
            foreach(var qsParam in Query) {
                request.AddParameter(qsParam.Key, qsParam.Value);
            }
        }

        private void SetBody(RestRequest request)
        {
            if (Body != null)
            {
                request.AddBody(Body);
                request.RequestFormat = DataFormat.Json;
            }
            else if (BinaryContent != null)
            {
                request.AddParameter("", BinaryContent, ParameterType.RequestBody);
            }
        }

        private void SetDefaults()
        {
            Protocol = string.IsNullOrWhiteSpace(Protocol) ? Request.DefaultProtocol : Protocol;
            Host = string.IsNullOrWhiteSpace(Host) ? Request.DefaultHostName : Host;
            ResponseType = string.IsNullOrWhiteSpace(ResponseType) ? ResponseTypes.Wildcard : ResponseType;
            ApiVersion = string.IsNullOrWhiteSpace(ApiVersion) ? ApiVersions.v3_0 : ApiVersion;

            Protocol = Protocol.ToLower();
            Host = Host.ToLower();

            Port = Port > 0 ? Port : GetDefaultPort(Protocol);
        }

        private string BuildAcceptsHeader() {
            return string.Format("{0};{1}", ResponseType, ApiVersion);
        }

        private int GetDefaultPort(string protocol)
        {
            if (Protocol == Request.ProtocolHttps)
            {
                return Request.DefaultHttpsPort;
            }
            return Request.DefaultHttpPort;
        }

        private string GetBaseUrl()
        {
            string url = Protocol.ToLower() + "://";

            if (Host.EndsWith("/"))
            {
                Host = Host.Substring(0, Host.Length - 1);
            }
            url += Host;

            if (Port != GetDefaultPort(Protocol))
            {
                url += ":" + Port.ToString();
            }

            return url;
        }

        #endregion
    }
}
