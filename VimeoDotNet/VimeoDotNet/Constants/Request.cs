using RestSharp;
using System;

namespace VimeoDotNet.Constants
{
    public static class Request
    {
        public const string DefaultProtocol = "https";
        public const string DefaultHostName = "api.vimeo.com";
        public const int DefaultHttpPort = 80;
        public const int DefaultHttpsPort = 443;
        public const Method DefaultMethod = Method.GET;
    }
}
