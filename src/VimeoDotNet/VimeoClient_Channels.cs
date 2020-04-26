using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Models;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        /// <inheritdoc />
        public async Task<bool> AddToChannelAsync(long channelId, long clipId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Put,
                Endpoints.ChannelVideo,
                new Dictionary<string, string>
                {
                    {"channelId", channelId.ToString()},
                    {"clipId", clipId.ToString()}
                }
            );

            return await ExecuteApiRequest(request).ConfigureAwait(false);
        }


        /// <inheritdoc />
        public async Task<Paginated<Channel>> GetChannelsAsync(UserId userId, GetChannelsParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                Endpoints.UserChannels,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()}
                },
                parameters
            );

            return await ExecuteApiRequest<Paginated<Channel>>(request).ConfigureAwait(false);
        }
    }
}