using System.Net.Http;

namespace VimeoDotNet.Constants
{
    /// <summary>
    /// Class Request.
    /// </summary>
    internal static class Request
    {
        /// <summary>
        /// The default HTTP port
        /// </summary>
        public const int DefaultHttpPort = 80;
        /// <summary>
        /// The default HTTPS port
        /// </summary>
        public const int DefaultHttpsPort = 443;
        /// <summary>
        /// The default host name
        /// </summary>
        public const string DefaultHostName = "api.vimeo.com";
        /// <summary>
        /// The default protocol
        /// </summary>
        public const string DefaultProtocol = ProtocolHttps;
        /// <summary>
        /// The protocol HTTP
        /// </summary>
        public const string ProtocolHttp = "http";
        /// <summary>
        /// The protocol HTTPS
        /// </summary>
        public const string ProtocolHttps = "https";
        /// <summary>
        /// The mock port
        /// </summary>
        public static int MockPort = 0;
        /// <summary>
        /// The default method
        /// </summary>
        public static readonly HttpMethod DefaultMethod = HttpMethod.Get;
        /// <summary>
        /// The mock host name
        /// </summary>
        public static string MockHostName = null;
        /// <summary>
        /// The mock protocol
        /// </summary>
        public static string MockProtocol = null;
    }
}