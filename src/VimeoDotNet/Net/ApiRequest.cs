using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        private readonly Dictionary<string, string> _queryString = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _urlSegments = new Dictionary<string, string>();
        private readonly List<string> _fields = new List<string>();
        private static readonly JsonSerializerSettings DateFormatSettings = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-ddTHH:mm:sszzz",
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

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
        /// Method
        /// </summary>
        public HttpMethod Method { get; set; }

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
        public HttpContent Body { get; set; }

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
        public List<string> Fields
        {
            get { return _fields; }
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
        protected static readonly HttpClient Client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });

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
        /// Execute request asynchronously
        /// </summary>
        /// <returns>Rest reponse</returns>
        public async Task<IApiResponse> ExecuteRequestAsync()
        {
            var response = await Client.SendAsync(PrepareRequest());
            var text = await response.Content.ReadAsStringAsync();
            return new ApiResponse(response.StatusCode, response.Headers, text);
        }

        /// <summary>
        /// Execute request asynchronously
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Rest repons</returns>
        public async Task<IApiResponse<T>> ExecuteRequestAsync<T>() where T : new()
        {
            var request = PrepareRequest();
            var response = await Client.SendAsync(request);
            var text = await response.Content.ReadAsStringAsync();
            return new ApiResponse<T>(response.StatusCode, response.Headers, text,
                JsonConvert.DeserializeObject<T>(text, DateFormatSettings));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// PrepareRequest
        /// </summary>
        /// <returns>Rest request</returns>
        protected HttpRequestMessage PrepareRequest()
        {
            SetDefaults();
            var request = new HttpRequestMessage(Method, BuildUrl());
            if (!ExcludeAuthorizationHeader)
            {
                SetAuth(request);
            }
            SetHeaders(request);
            request.Content = Body;
            return request;
        }

        /// <summary>
        /// Set request headers
        /// </summary>
        /// <param name="request">Request</param>
        protected void SetHeaders(HttpRequestMessage request)
        {
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(BuildAcceptsHeader()));
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        }

        /// <summary>
        /// Set authentication
        /// </summary>
        protected void SetAuth(HttpRequestMessage request)
        {
            if (!string.IsNullOrWhiteSpace(AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }
            if (string.IsNullOrWhiteSpace(ClientId) || string.IsNullOrWhiteSpace(ClientSecret))
                return;
            var token = $"{ClientId}:{ClientSecret}";
            var tokenBytes = Encoding.ASCII.GetBytes(token);
            var encoded = Convert.ToBase64String(tokenBytes);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encoded);
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

        /// <summary>
        /// Build request URL
        /// </summary>
        /// <returns>Request URL</returns>
        protected string BuildUrl()
        {
            var sb = new StringBuilder();
            sb.Append(GetBaseUrl());
            var path = Path;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var urlSegment in UrlSegments)
            {
                path = path.Replace($"{{{urlSegment.Key}}}", urlSegment.Value);
            }
            sb.Append(path);
            if (_fields.Count > 0)
            {
                Query.Add("fields", string.Join(",", _fields));
            }
            if (Query.Keys.Count == 0)
                return sb.ToString();
            sb.Append("?");
            sb.Append(string.Join("&", Query.Select(q => $"{q.Key}={q.Value}")));
            return sb.ToString();
        }

        #endregion
    }
}