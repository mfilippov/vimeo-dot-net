using System.Net.Http;

namespace VimeoDotNet.Constants
{
    internal static class Request
    {
        public const int DefaultHttpPort = 80;
        public const int DefaultHttpsPort = 443;
        public const string DefaultHostName = "api.vimeo.com";
        public const string DefaultProtocol = ProtocolHttps;
        public const string ProtocolHttp = "http";
        public const string ProtocolHttps = "https";
        public static int MockPort = 0;
        public static readonly HttpMethod DefaultMethod = HttpMethod.Get;
        public static string MockHostName = null;
        public static string MockProtocol = null;
    }
}