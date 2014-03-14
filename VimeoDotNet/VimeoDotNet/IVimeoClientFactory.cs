using System;

namespace VimeoDotNet
{
    public interface IVimeoClientFactory
    {
        IVimeoClient GetVimeoClient(string accessToken);
        IVimeoClient GetVimeoClient(string clientId, string clientSecret);
    }
}
