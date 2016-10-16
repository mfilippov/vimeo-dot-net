using System.Collections.Generic;
using VimeoDotNet.Parameters;

namespace VimeoDotNet.Net
{
    /// <summary>
    /// IApiRequestFactory
    /// </summary>
    public interface IApiRequestFactory
    {
        /// <summary>
        /// Return api request
        /// </summary>
        /// <returns>Api request</returns>
        IApiRequest GetApiRequest();
        /// <summary>
        /// Return api request by AccessToken
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <returns>Api request</returns>
        IApiRequest GetApiRequest(string accessToken);
        /// <summary>
        /// Return api request by ClientId and ClientSecret
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">ClientSecret</param>
        /// <returns>Api request</returns>
        IApiRequest GetApiRequest(string clientId, string clientSecret);

		/// <summary>
		/// Performs basic error checking on parameters and then generates an IApiRequest bound with the provided values. Will throw exception if invalid parameters provided.
		/// </summary>
		/// <param name="accessToken">API AccessToken. Cannot be null or empty.</param>
		/// <param name="method">HttpMethod of the request.</param>
		/// <param name="endpoint">Url of the API endpoint being called. Can contain substitution segments - ex: /user/{userId}/.</param>
		/// <param name="urlSubstitutions">Dictionary containing url parameter keys and values. Continuing above example, key would be "userId", value would be "12345".</param>
		/// <param name="additionalParameters">IParameterProvider that returns any other parameters the API method accepts. Can be null for no parameters, or use ParameterDictionary if typed provider not available.</param>
		/// <returns>A ready to execute IApiRequest.</returns>
		IApiRequest AuthorizedRequest(string accessToken, RestSharp.Method method, string endpoint, IDictionary<string,string> urlSubstitutions = null, IParameterProvider additionalParameters = null);
    }
}