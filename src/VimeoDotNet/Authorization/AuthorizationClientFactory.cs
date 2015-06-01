namespace VimeoDotNet.Authorization
{
    public class AuthorizationClientFactory : IAuthorizationClientFactory
    {
        public IAuthorizationClient GetAuthorizationClient(string clientId, string clientSecret)
        {
            return new AuthorizationClient(clientId, clientSecret);
        }
    }
}