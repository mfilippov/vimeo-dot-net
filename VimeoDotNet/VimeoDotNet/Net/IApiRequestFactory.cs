namespace VimeoDotNet.Net
{
    public interface IApiRequestFactory
    {
        IApiRequest GetApiRequest();
        IApiRequest GetApiRequest(string accessToken);
        IApiRequest GetApiRequest(string clientId, string clientSecret);
    }
}