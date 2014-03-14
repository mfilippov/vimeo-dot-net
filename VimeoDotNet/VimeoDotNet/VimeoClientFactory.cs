using RestSharp;
using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet.Authorization
{
    public class VimeoClientFactory : VimeoDotNet.IVimeoClientFactory
    {

        #region Fields

        protected IAuthorizationClientFactory _authClientFactory;
        protected IApiRequestFactory _apiRequestFactory;

        #endregion

        #region Constructors

        public VimeoClientFactory()
        {
            _authClientFactory = new AuthorizationClientFactory();
            _apiRequestFactory = new ApiRequestFactory();
        }

        /// <summary>
        /// IOC Constructor for use with IVimeoClientFactory
        /// </summary>
        /// <param name="authClientFactory">The IAuthorizationClientFactory</param>
        /// <param name="apiRequestFactory">The IApiRequestFactory</param>
        public VimeoClientFactory(IAuthorizationClientFactory authClientFactory, IApiRequestFactory apiRequestFactory) {
            _authClientFactory = authClientFactory;
            _apiRequestFactory = apiRequestFactory;
        }

        #endregion

        #region Public Functions

        public IVimeoClient GetVimeoClient(string clientId, string clientSecret)
        {
            return new VimeoClient(_authClientFactory, _apiRequestFactory, clientId, clientSecret);
        }

        public IVimeoClient GetVimeoClient(string accessToken)
        {
            return new VimeoClient(_authClientFactory, _apiRequestFactory, accessToken);
        }

        #endregion
    }
}
