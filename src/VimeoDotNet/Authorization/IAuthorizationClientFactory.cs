namespace VimeoDotNet.Authorization
{
    /// <summary>
    /// IAuthorizationClientFactory
    /// </summary>
    public interface IAuthorizationClientFactory
    {
        /// <summary>
        /// Return authorization client
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">ClientSecret</param>
        /// <returns>Authorization client</returns>
        IAuthorizationClient GetAuthorizationClient(string clientId, string clientSecret);
    }
}