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
        public async Task<Channel> CreateChannelAsync(EditChannelParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Post,
                Endpoints.Channels,
                null,
                parameters
            );

            return await ExecuteApiRequest<Channel>(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteChannelAsync(long channelId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Delete,
                Endpoints.Channel,
                new Dictionary<string, string>
                {
                    {"channelId", channelId.ToString()}
                }
            );

            return await ExecuteApiRequest(request).ConfigureAwait(false);
        }

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
        public async Task<Channel> GetChannelAsync(long channelId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                Endpoints.Channel,
                new Dictionary<string, string>
                {
                    {"channelId", channelId.ToString()},
                }
            );

            return await ExecuteApiRequest<Channel>(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Paginated<Channel>> GetChannelsAsync(GetChannelsParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                Endpoints.Channels,
                null,
                parameters
            );

            return await ExecuteApiRequest<Paginated<Channel>>(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Paginated<Channel>> GetUserChannelsAsync(GetChannelsParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                Endpoints.MeChannels,
                null,
                parameters
            );

            return await ExecuteApiRequest<Paginated<Channel>>(request).ConfigureAwait(false);
        }
    }
}