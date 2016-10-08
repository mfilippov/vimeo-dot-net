using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using VimeoDotNet.Constants;

namespace VimeoDotNet.Net
{
    /// <summary>
    /// Api request
    /// </summary>
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

        /// <summary>
        /// Api version
        /// </summary>
        public string ApiVersion { get; set; }
        /// <summary>
        /// response type
        /// </summary>
        public string ResponseType { get; set; }
        /// <summary>
        /// Protocol
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, string> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// Method
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// Path
        /// </summary>
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

        /// <summary>
        /// Body
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, string> Query
        {
            get { return _queryString; }
        }

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, string> UrlSegments
        {
            get { return _urlSegments; }
        }

        /// <summary>
        /// Binary content
        /// </summary>
        public byte[] BinaryContent { get; set; }

        /// <summary>
        /// Exclude authorization header
        /// </summary>
        public bool ExcludeAuthorizationHeader { get; set; }

        #endregion

        #region Private Properties

        /// <summary>
        /// Rest client
        /// </summary>
        protected IRestClient Client { get; private set; }
        /// <summary>
        /// Client Id
        /// </summary>
        protected string ClientId { get; set; }
        /// <summary>
        /// Client secret
        /// </summary>
        protected string ClientSecret { get; set; }
        /// <summary>
        /// Access token
        /// </summary>
        protected string AccessToken { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create new request
        /// </summary>
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

        /// <summary>
        /// Create new request with ClientId and ClientSecret
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">ClientSecret</param>
        public ApiRequest(string clientId, string clientSecret)
            : this()
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        /// <summary>
        /// Create new request with AccessToken
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        public ApiRequest(string accessToken)
            : this()
        {
            AccessToken = accessToken;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// ExecuteRequest
        /// </summary>
        /// <returns>Rest response</returns>
        public IRestResponse ExecuteRequest()
        {
            IRestRequest request = PrepareRequest();
            IRestClient client = PrepareClient();

            IRestResponse response = client.Execute(request);
            return response;
        }
        /// <summary>
        /// Execute request asynchronously
        /// </summary>
        /// <returns>Rest reponse</returns>
        public async Task<IRestResponse> ExecuteRequestAsync()
        {
            return await GetAsyncRequestAwaiter();
        }
        /// <summary>
        /// Execute request
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Rest repons</returns>
        public IRestResponse<T> ExecuteRequest<T>() where T : new()
        {
            IRestRequest request = PrepareRequest();
            IRestClient client = PrepareClient();

            IRestResponse<T> response = client.Execute<T>(request);
            return response;
        }
        /// <summary>
        /// Execute request asynchronously
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Rest repons</returns>
        public async Task<IRestResponse<T>> ExecuteRequestAsync<T>() where T : new()
        {
            return await GetAsyncRequestAwaiter<T>();
        }

        #endregion

        #region Private Methods

        #region Async

        /// <summary>
        /// GetAsyncRequestAwaiter
        /// </summary>
        /// <returns>Rest response</returns>
        protected Task<IRestResponse> GetAsyncRequestAwaiter()
        {
            IRestRequest request = PrepareRequest();
            IRestClient client = PrepareClient();

            var tcs = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, response => tcs.SetResult(response));

            return tcs.Task;
        }

        /// <summary>
        /// GetAsyncRequestAwaiter
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Rest response</returns>
        protected Task<IRestResponse<T>> GetAsyncRequestAwaiter<T>() where T : new()
        {
            IRestRequest request = PrepareRequest();
            IRestClient client = PrepareClient();

            var tcs = new TaskCompletionSource<IRestResponse<T>>();
            client.ExecuteAsync<T>(request, response => tcs.SetResult(response));

            return tcs.Task;
        }

        #endregion

        /// <summary>
        /// PrepareClient
        /// </summary>
        /// <returns>Rest client</returns>
        protected IRestClient PrepareClient()
        {
            IRestClient client = GetClient();
            client.Authenticator = GetAuthenticator();

            return client;
        }
        /// <summary>
        /// PrepareRequest
        /// </summary>
        /// <returns>Rest request</returns>
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

        /// <summary>
        /// Set json response
        /// </summary>
        /// <param name="request">Request</param>
        protected void SetJsonResponse(IRestRequest request)
        {
            request.DateFormat = "yyyy-MM-ddTHH:mm:sszzz";
            request.OnBeforeDeserialization += (IRestResponse response) =>
            {
                // Switch response content type to allow deserializer to work properly.
                response.ContentType = "application/json";
            };
        }

        /// <summary>
        /// Retrun authenticator
        /// </summary>
        /// <returns>Authenticator</returns>
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
        /// <summary>
        /// Retrun bearer authenticator
        /// </summary>
        /// <returns>Bearer authenticator</returns>
        protected IAuthenticator GetBearerAuthenticator()
        {
            return new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer");
        }
        /// <summary>
        /// Retrun basic authenticator
        /// </summary>
        /// <returns>Basic authenticator</returns>
        protected IAuthenticator GetBasicAuthenticator()
        {
            string token = string.Format("{0}:{1}", ClientId, ClientSecret);
            byte[] tokenBytes = Encoding.ASCII.GetBytes(token);
            string encoded = Convert.ToBase64String(tokenBytes);

            return new OAuth2AuthorizationRequestHeaderAuthenticator(encoded, "Basic");
        }

        /// <summary>
        /// Retrun rest client
        /// </summary>
        /// <returns>Rest client</returns>
        protected IRestClient GetClient()
        {
            string baseUrl = GetBaseUrl();
            if (Client == null || Client.BaseUrl.AbsoluteUri != baseUrl)
            {
                Client = new RestClient(baseUrl);
            }
            return Client;
        }

        /// <summary>
        /// Set request url segment
        /// </summary>
        /// <param name="request">Request</param>
        protected void SetUrlSegments(IRestRequest request)
        {
            foreach (var segment in UrlSegments)
            {
                request.AddUrlSegment(segment.Key, segment.Value);
            }
        }

        /// <summary>
        /// Set request headers
        /// </summary>
        /// <param name="request">Request</param>
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

        /// <summary>
        /// Set request query params
        /// </summary>
        /// <param name="request">Request</param>
        protected void SetQueryParams(IRestRequest request)
        {
            foreach (var qsParam in Query)
            {
                request.AddParameter(qsParam.Key, qsParam.Value);
            }
        }

        /// <summary>
        /// Set request body
        /// </summary>
        /// <param name="request">Request</param>
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

        /// <summary>
        /// Set defaults
        /// </summary>
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

        /// <summary>
        /// Build accepts header
        /// </summary>
        /// <returns>Accepts header</returns>
        protected string BuildAcceptsHeader()
        {
            return string.Format("{0};{1}", ResponseType, ApiVersion);
        }

        /// <summary>
        /// Return default port
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <returns>Port</returns>
        protected int GetDefaultPort(string protocol)
        {
            if (Protocol == Request.ProtocolHttps)
            {
                return Request.DefaultHttpsPort;
            }
            return Request.DefaultHttpPort;
        }

        /// <summary>
        /// Retrun base URL
        /// </summary>
        /// <returns>Base URL</returns>
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