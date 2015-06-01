namespace VimeoDotNet.Authorization
{
    public interface IAuthorizationClientFactory
    {
        IAuthorizationClient GetAuthorizationClient(string clientId, string clientSecret);
    }
}