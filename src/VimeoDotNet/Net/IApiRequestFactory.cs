using System.Collections.Generic;

namespace VimeoDotNet.Net
{
    public interface IApiRequestFactory
    {
        IApiRequest GetApiRequest();
        IApiRequest GetApiRequest(string accessToken);
        IApiRequest GetApiRequest(string clientId, string clientSecret);

		IApiRequest AuthorizedRequest(string accessToken, RestSharp.Method method, string endpoint, IDictionary<string,string> urlSubstitutions = null, IDictionary<string, string> queryParameters = null);
    }
}