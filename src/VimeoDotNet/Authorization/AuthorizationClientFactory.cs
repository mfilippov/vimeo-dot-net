namespace VimeoDotNet.Authorization
{
    /// <summary>
    /// Authorization client factory
    /// </summary>
    public class AuthorizationClientFactory : IAuthorizationClientFactory
    {
        /// <summary>
        /// Return authorization client
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">ClientSecret</param>
        /// <returns>Authorization client</returns>
        public IAuthorizationClient GetAuthorizationClient(string clientId, string clientSecret)
        {
            return new AuthorizationClient(clientId, clientSecret);
        }
    }
}