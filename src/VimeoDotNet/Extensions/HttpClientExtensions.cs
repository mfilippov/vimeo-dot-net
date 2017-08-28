using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VimeoDotNet.Extensions
{
    /// <summary>
    /// HttpClient extensions
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Send an HTTP PATCH request as an asynchronous implementation
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, HttpRequestMessage requestMessage)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestMessage.RequestUri)
            {
                Content = requestMessage.Content
            };
            
            return await client.SendAsync(request);
        }
    }
}
