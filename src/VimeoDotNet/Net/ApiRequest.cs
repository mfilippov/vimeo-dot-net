using System;
using System.Collections.Generic;
using System.IO.Pipes;
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
        protected static readonly HttpClient Client = new HttpClient();

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

        //        /// <summary>
        //        /// Set request body
        //        /// </summary>
        //        /// <param name="request">Request</param>
        //        protected void SetBody(HttpRequestMessage request)
        //        {
        //            string mediaType;
        //            
        //            if (!Headers.ContainsKey(Request.HeaderContentType) &&
        //                (Method == HttpMethod.Post || Method.Method == "PATCH" || Method == HttpMethod.Put))
        //            {
        //                mediaType = "application/x-www-form-urlencoded";
        //            }
        //            else
        //            {
        //                mediaType = Headers[Request.HeaderContentRange];
        //            }
        //            
        //            if (Body != null)
        //            {
        //                request.Content = new StringContent(JsonConvert.SerializeObject(Body), Encoding.UTF8, mediaType);
        //            }
        //            else if (BinaryContent != null)
        //            {
        //                request.Content = new ByteArrayContent(BinaryContent, );
        //            }
        //            
        //            foreach (var header in Headers)
        //            {
        //                if (header.Key == Request.HeaderContentType 
        //                    || header.Key == Request.HeaderAccepts 
        //                    || header.Key == Request.HeaderContentLength)
        //                    continue;
        //                request.Headers.Add(header.Key, header.Value);
        //            }
        //        }
        //        
        //        /// <summary>
        //        /// Set json response
        //        /// </summary>
        //        /// <param name="request">Request</param>
        //        protected void SetJsonResponse(HttpRequestMessage request)
        //        {
        //            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        //            //request.DateFormat = "yyyy-MM-ddTHH:mm:sszzz";
        //        }
        //        
        //        

        //        /// <summary>
        //        /// Retrun authenticator
        //        /// </summary>
        //        /// <returns>Authenticator</returns>
        //        protected void GetAuthenticator(HttpRequestMessage request)
        //        {
        //            if (!string.IsNullOrWhiteSpace(AccessToken))
        //            {
        //                return GetBearerAuthenticator();
        //            }
        //            if (!string.IsNullOrWhiteSpace(ClientId) && !string.IsNullOrWhiteSpace(ClientSecret))
        //            {
        //                return GetBasicAuthenticator();
        //            }
        //            return null;
        //        }
        ////        /// <summary>
        //        /// Retrun bearer authenticator
        //        /// </summary>
        //        /// <returns>Bearer authenticator</returns>
        //        protected IAuthenticator GetBearerAuthenticator()
        //        {
        //            return new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer");
        //        }
        //        /// <summary>
        //        /// Retrun basic authenticator
        //        /// </summary>
        //        /// <returns>Basic authenticator</returns>
        //        protected IAuthenticator GetBasicAuthenticator()
        //        {
        //            string token = string.Format("{0}:{1}", ClientId, ClientSecret);
        //            byte[] tokenBytes = Encoding.ASCII.GetBytes(token);
        //            string encoded = Convert.ToBase64String(tokenBytes);
        //
        //            return new OAuth2AuthorizationRequestHeaderAuthenticator(encoded, "Basic");
        //        }
        //        /// <summary>
        //        /// Set request url segment
        //        /// </summary>
        //        /// <param name="request">Request</param>
        //        protected void SetUrlSegments(IRestRequest request)
        //        {
        //            foreach (var segment in UrlSegments)
        //            {
        //                request.AddUrlSegment(segment.Key, segment.Value);
        //            }
        //        }
        //
        //        /// <summary>
        //        /// Set request query params
        //        /// </summary>
        //        /// <param name="request">Request</param>
        //        protected void SetQueryParams(IRestRequest request)
        //        {
        //            foreach (var qsParam in Query)
        //            {
        //                request.AddParameter(qsParam.Key, qsParam.Value);
        //            }
        //        }

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
            if (_fields.Count > 0)
            {
                Query.Add("fields", string.Concat(",", _fields));
            }
            sb.Append(path);
            if (Query.Keys.Count == 0)
                return sb.ToString();
            sb.Append("?");
            sb.Append(string.Join("&", Query.Select(q => $"{q.Key}={q.Value}")));
            return sb.ToString();
        }

        #endregion
    }
}