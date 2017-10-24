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
        public async Task<Album> GetAlbumAsync(UserId userId, long albumId)
        {
            var request = ApiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                userId.IsMe ? Endpoints.MeAlbum : Endpoints.UserAlbum,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()}
                },
                null
            );

            return await ExecuteApiRequest<Album>(request);
        }
 
        /// <inheritdoc />
        public async Task<Paginated<Album>> GetAlbumsAsync(UserId userId, GetAlbumsParameters parameters = null)
        {
            var request = ApiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                userId.IsMe ? Endpoints.MeAlbums : Endpoints.UserAlbums,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()}
                },
                parameters
            );

            return await ExecuteApiRequest<Paginated<Album>>(request);
        }

        

        /// <inheritdoc />
        public async Task<Album> CreateAlbumAsync(UserId userId, EditAlbumParameters parameters = null)
        {
            var request = ApiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Post,
                Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbums),
                null,
                parameters
            );

            return await ExecuteApiRequest<Album>(request);
        }

        /// <inheritdoc />
        public async Task<Album> UpdateAlbumAsync(UserId userId, long albumId, EditAlbumParameters parameters = null)
        {
            var request = ApiRequestFactory.AuthorizedRequest(
                AccessToken,
                new HttpMethod("PATCH"),
                Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
                new Dictionary<string, string>()
                {
                    {"albumId", albumId.ToString()}
                },
                parameters
            );

            return await ExecuteApiRequest<Album>(request);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAlbumAsync(UserId userId, long albumId)
        {
            var request = ApiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Delete,
                Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
                new Dictionary<string, string>()
                {
                    {"albumId", albumId.ToString()}
                }
            );

            return await ExecuteApiRequest(request);
        }

        /// <inheritdoc />
        public async Task<bool> AddToAlbumAsync(UserId userId, long albumId, long clipId)
        {
            var request = ApiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Put,
                Endpoints.UserAlbumVideo,
                new Dictionary<string, string>()
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()},
                    {"clipId", clipId.ToString()}
                }
            );

            return await ExecuteApiRequest(request);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveFromAlbumAsync(UserId userId, long albumId, long clipId)
        {
            var request = ApiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Delete,
                Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbumVideo),
                new Dictionary<string, string>()
                {
                    {"albumId", albumId.ToString()},
                    {"clipId", clipId.ToString()}
                }
            );

            return await ExecuteApiRequest(request);
        }
    }
}