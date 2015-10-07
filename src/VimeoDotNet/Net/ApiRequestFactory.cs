using System;
using System.Collections.Generic;
using RestSharp;

namespace VimeoDotNet.Net
{
    public class ApiRequestFactory : IApiRequestFactory
    {
        public IApiRequest GetApiRequest()
        {
            return new ApiRequest();
        }

        public IApiRequest GetApiRequest(string clientId, string clientSecret)
        {
            return new ApiRequest(clientId, clientSecret);
        }

        public IApiRequest GetApiRequest(string accessToken)
        {
            return new ApiRequest(accessToken);
        }

		public IApiRequest AuthorizedRequest(string accessToken, Method method, string endpoint, IDictionary<string, string> urlSubstitutions = null, IDictionary<string, string> queryParameters = null)
		{
			VerifyAccessToken(accessToken);

			ApiRequest request = new ApiRequest(accessToken);
			request.Method = method;
			request.Path = endpoint;

			if (urlSubstitutions != null)
			{
				foreach (var item in urlSubstitutions)
				{
					request.UrlSegments.Add(item);
				}
			}
			
			if (queryParameters != null)
			{
				foreach (var queryParameter in queryParameters)
				{
					request.Query.Add(queryParameter);
				}
			}

			return request;
		}

		public void VerifyAccessToken(string accessToken)
		{
			if (string.IsNullOrWhiteSpace(accessToken))
			{
				throw new InvalidOperationException("Please authenticate via OAuth to obtain an access token.");
			}
		}
    }
}