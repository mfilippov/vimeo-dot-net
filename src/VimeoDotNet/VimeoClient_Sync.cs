using System;
using System.Runtime.ExceptionServices;
using VimeoDotNet.Extensions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of Vimeo API
    /// </summary>
    public partial class VimeoClient
    {
        #region User authentication

        /// <inheritdoc />
        /// <summary>
        /// Exchange the code for an access token
        /// </summary>
        /// <param name="authorizationCode">A string token you must exchange for your access token</param>
        /// <param name="redirectUrl">This field is required, and must match one of your application’s
        /// redirect URI’s</param>
        /// <returns>AccessTokenResponse</returns>
        [Obsolete("Use async API instead sync wrapper")]
        public AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl)
        {
            return OAuth2Client.GetAccessTokenAsync(authorizationCode, redirectUrl).RunSynchronouslyWithCurrentCulture();
        }
        #endregion

        #region Account information

        /// <inheritdoc />
        /// <summary>
        /// Get user information
        /// </summary>
        /// <returns>User information</returns>
        [Obsolete("Use async API instead sync wrapper")]
        public User GetAccountInformation()
        {
            try
            {
                return GetAccountInformationAsync().RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
            return null;
        }

        /// <inheritdoc />
        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="parameters">User parameters</param>
        /// <returns>User information</returns>
        [Obsolete("Use async API instead sync wrapper")]
        public User UpdateAccountInformation(EditUserParameters parameters)
        {
            try
            {
                return UpdateAccountInformationAsync(parameters).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
            return null;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get user information
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>User information object</returns>
        [Obsolete("Use async API instead sync wrapper")]
        public User GetUserInformation(long userId)
        {
            try
            {
                return GetUserInformationAsync(userId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
            return null;
        }

        #endregion

    }
}