using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Models;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    /// <summary>
    /// Class VimeoClient.
    /// Implements the <see cref="VimeoDotNet.IVimeoClient" />
    /// </summary>
    /// <seealso cref="VimeoDotNet.IVimeoClient" />
    public partial class VimeoClient
    {
        /// <inheritdoc />
        public async Task<Album> GetAlbumAsync(UserId userId, long albumId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                userId.IsMe ? Endpoints.MeAlbum : Endpoints.UserAlbum,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()}
                }
            );

            return await ExecuteApiRequest<Album>(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Paginated<Album>> GetAlbumsAsync(UserId userId, GetAlbumsParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                userId.IsMe ? Endpoints.MeAlbums : Endpoints.UserAlbums,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()}
                },
                parameters
            );

            return await ExecuteApiRequest<Paginated<Album>>(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Album> CreateAlbumAsync(UserId userId, EditAlbumParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Post,
                 userId.IsMe ? Endpoints.MeAlbums : Endpoints.UserAlbums,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()}
                },
                parameters
            );

            return await ExecuteApiRequest<Album>(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Album> UpdateAlbumAsync(UserId userId, long albumId, EditAlbumParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                new HttpMethod("PATCH"),
                userId.IsMe ? Endpoints.MeAlbum : Endpoints.UserAlbum,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()}
                },
                parameters
            );

            return await ExecuteApiRequest<Album>(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAlbumAsync(UserId userId, long albumId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Delete,
                userId.IsMe ? Endpoints.MeAlbum : Endpoints.UserAlbum,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()}
                }
            );

            return await ExecuteApiRequest(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> AddToAlbumAsync(UserId userId, long albumId, long clipId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Put,
                userId == UserId.Me ? Endpoints.MeAlbumVideo : Endpoints.UserAlbumVideo,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()},
                    {"clipId", clipId.ToString()}
                }
            );

            return await ExecuteApiRequest(request).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveFromAlbumAsync(UserId userId, long albumId, long clipId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Delete,
                Endpoints.UserAlbumVideo,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()},
                    {"clipId", clipId.ToString()}
                }
            );

            return await ExecuteApiRequest(request).ConfigureAwait(false);
        }
    }
}