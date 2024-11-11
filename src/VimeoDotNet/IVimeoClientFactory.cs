namespace VimeoDotNet
{
    /// <summary>
    /// Vimeo API client factory
    /// </summary>
    public interface IVimeoClientFactory
    {
        /// <summary>
        /// Return client by access token
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns>VimeoClient</returns>
        IVimeoClient GetVimeoClient(string accessToken);

        /// <summary>
        /// Return client based on ClientId and SecretId
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">SecretId</param>
        /// <returns>VimeoClient</returns>
        IVimeoClient GetVimeoClient(string clientId, string clientSecret);
    }
}