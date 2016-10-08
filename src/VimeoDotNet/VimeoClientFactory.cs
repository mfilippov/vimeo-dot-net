using VimeoDotNet.Net;

namespace VimeoDotNet.Authorization
{
    /// <summary>
    /// Vimeo client factory
    /// </summary>
    public class VimeoClientFactory : IVimeoClientFactory
    {
        #region Fields
        /// <summary>
        /// Api request factory
        /// </summary>
        protected IApiRequestFactory ApiRequestFactory;
        /// <summary>
        /// Auth client factory
        /// </summary>
        protected IAuthorizationClientFactory AuthClientFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new Vimeo client factory
        /// </summary>
        public VimeoClientFactory()
        {
            AuthClientFactory = new AuthorizationClientFactory();
            ApiRequestFactory = new ApiRequestFactory();
        }

        /// <summary>
        /// IOC Constructor for use with IVimeoClientFactory
        /// </summary>
        /// <param name="authClientFactory">The IAuthorizationClientFactory</param>
        /// <param name="apiRequestFactory">The IApiRequestFactory</param>
        public VimeoClientFactory(IAuthorizationClientFactory authClientFactory, IApiRequestFactory apiRequestFactory)
        {
            AuthClientFactory = authClientFactory;
            ApiRequestFactory = apiRequestFactory;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Return client based on ClientId and SecretId
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="clientSecret">SecretId</param>
        /// <returns>VimeoClient</returns>
        public IVimeoClient GetVimeoClient(string clientId, string clientSecret)
        {
            return new VimeoClient(AuthClientFactory, ApiRequestFactory, clientId, clientSecret);
        }

        /// <summary>
        /// Return client by access token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns>VimeoClient</returns>
        public IVimeoClient GetVimeoClient(string accessToken)
        {
            return new VimeoClient(AuthClientFactory, ApiRequestFactory, accessToken);
        }

        #endregion
    }
}