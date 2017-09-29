using System;
using System.Collections.Generic;
using System.Net.Http;
using VimeoDotNet.Parameters;

namespace VimeoDotNet.Net
{
    /// <inheritdoc />
    public class ApiRequestFactory : IApiRequestFactory
    {
        /// <inheritdoc />
        public IApiRequest GetApiRequest()
        {
            return new ApiRequest();
        }

        /// <inheritdoc />
        public IApiRequest GetApiRequest(string clientId, string clientSecret)
        {
            return new ApiRequest(clientId, clientSecret);
        }

        /// <inheritdoc />
        public IApiRequest GetApiRequest(string accessToken)
        {
            return new ApiRequest(accessToken);
        }

        /// <inheritdoc />
        public IApiRequest AuthorizedRequest(string accessToken, HttpMethod method, string endpoint, IDictionary<string, string> urlSubstitutions = null, IParameterProvider additionalParameters = null)
        {
            // Verify the provided parameters at least have a chance of succeeding, otherwise, exit early via exception...
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException("Please authenticate via OAuth to obtain an access token.");
            }
            VerifyParameters(additionalParameters);

            // Generate a basic request...
            var request = new ApiRequest(accessToken)
            {
                Method = method,
                Path = endpoint
            };

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
                if (method == HttpMethod.Get)
                {
                    foreach (var parameter in additionalParameters.GetParameterValues())
                    {
                        request.Query.Add(parameter);
                    }
                }
                else
                {
                    request.Body = new FormUrlEncodedContent(additionalParameters.GetParameterValues());
                }
            }

            return request;
        }

        /// <summary>
        /// Validates an IParameterProvider and throws an exception if an error is present.
        /// </summary>
        /// <param name="parameters">IParameterProvider being validated.</param>
        private static void VerifyParameters(IParameterProvider parameters)
        {
            // if there are no parameters, there are no errors
            // otherwise check provider for an error message
            var errorMessage = parameters?.ValidationError();
            if (string.IsNullOrEmpty(errorMessage))
                return;
            throw new InvalidOperationException($"API Argument Error: {errorMessage}");
        }
    }
}