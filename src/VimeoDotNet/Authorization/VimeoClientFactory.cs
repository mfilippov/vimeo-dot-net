using JetBrains.Annotations;
using VimeoDotNet.Net;

namespace VimeoDotNet.Authorization
{
    /// <inheritdoc />
    [PublicAPI]
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

        /// <inheritdoc />
        public IVimeoClient GetVimeoClient(string clientId, string clientSecret)
        {
            return new VimeoClient(AuthClientFactory, ApiRequestFactory, clientId, clientSecret);
        }

        /// <inheritdoc />
        public IVimeoClient GetVimeoClient(string accessToken)
        {
            return new VimeoClient(AuthClientFactory, ApiRequestFactory, accessToken);
        }

        #endregion
    }
}