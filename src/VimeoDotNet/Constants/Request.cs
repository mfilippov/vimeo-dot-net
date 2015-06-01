using RestSharp;

namespace VimeoDotNet.Constants
{
    public static class Request
    {
        public const string ProtocolHttps = "https";

        public const string HeaderContentType = "Content-Type";
        public const string HeaderContentLength = "Content-Length";
        public const string HeaderContentRange = "Content-Range";
        public const string HeaderAccepts = "Accept";

        public const string DefaultProtocol = ProtocolHttps;
        public const string DefaultHostName = "api.vimeo.com";
        public const int DefaultHttpPort = 80;
        public const int DefaultHttpsPort = 443;
        public const Method DefaultMethod = Method.GET;
    }
}