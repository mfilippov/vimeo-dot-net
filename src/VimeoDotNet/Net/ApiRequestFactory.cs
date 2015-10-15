using System;
using System.Collections.Generic;
using RestSharp;
using VimeoDotNet.Parameters;

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

		public IApiRequest AuthorizedRequest(string accessToken, Method method, string endpoint, IDictionary<string, string> urlSubstitutions = null, IParameterProvider additionalParameters = null)
		{
			// Verify the provided parameters at least have a chance of succeeding, otherwise, exit early via exception...
			VerifyAccessToken(accessToken);
			VerifyParameters(additionalParameters);

			// Generate a basic request...
			ApiRequest request = new ApiRequest(accessToken);
			request.Method = method;
			request.Path = endpoint;

			// Add url parameters if they are present...
			if (urlSubstitutions != null)
			{
				foreach (var item in urlSubstitutions)
				{
					request.UrlSegments.Add(item);
				}
			}

			// Add query or body parameters if present...
			if (additionalParameters != null)
			{
				foreach (var parameter in additionalParameters.GetParameterValues())
				{
					request.Query.Add(parameter);
				}
			}

			return request;
		}

		/// <summary>
		/// Validates existence of an accessToken and throws an exception if one is not provided.
		/// </summary>
		/// <param name="accessToken">AccessToken being validated.</param>
		private void VerifyAccessToken(string accessToken)
		{
			if (string.IsNullOrWhiteSpace(accessToken))
			{
				throw new InvalidOperationException("Please authenticate via OAuth to obtain an access token.");
			}
		}

		/// <summary>
		/// Validates an IParameterProvider and throws an exception if an error is present.
		/// </summary>
		/// <param name="parameters">IParameterProvider being validated.</param>
		private void VerifyParameters(IParameterProvider parameters)
		{
			// if there are no parameters, there are no errors
			if (parameters == null) { return; }

			// otherwise check provider for an error message
			string errorMessage = parameters.ValidationError();
			if (String.IsNullOrEmpty(errorMessage))
			{
				return;
			}
			else
			{
				throw new InvalidOperationException(String.Format("API Argument Error: {0}", errorMessage));
			}
		}
    }
}