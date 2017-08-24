using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        /// <summary>
        /// Delete video asynchronously
        /// </summary>
        /// <param name="clipId">CliepId</param>
        public async Task DeleteVideoAsync(long clipId)
        {
            try
            {
                var request = GenerateVideoDeleteRequest(clipId);
                var response = await request.ExecuteRequestAsync();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error deleting video.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error updating user video metadata.", ex);
            }
        }

        /// <summary>
        /// Get video from album by AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Video</returns>
        public async Task<Video> GetAlbumVideoAsync(long albumId, long clipId)
        {
            try
            {
                var request = GenerateAlbumVideosRequest(albumId, clipId: clipId);
                var response = await request.ExecuteRequestAsync<Video>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user album video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving user album video.", ex);
            }
        }

        // Added 28/07/2015
        /// <summary>
        /// Get videos by AlbumId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="page">The page number to show.</param>
        /// <param name="perPage">Number of items to show on each page. Max 50.</param>
        /// <param name="sort">The default sort order of an Album's videos</param>
        /// <param name="direction">The direction that the results are sorted.</param>
        /// <returns>Paginated videos</returns>
        public async Task<Paginated<Video>> GetAlbumVideosAsync(long albumId, int? page, int? perPage, string sort = null, string direction = null)
        {
            try
            {
                var request = GenerateAlbumVideosRequest(albumId, page: page, perPage: perPage, sort: sort, direction: direction);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving account album videos.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new Paginated<Video>
                    {
                        data = new List<Video>(),
                        page = 0,
                        total = 0
                    };
                }
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving account album videos.", ex);
            }
        }

        /// <summary>
        /// Get video from album by AlbumId and UserId and ClipId asynchronously
        /// </summary>
        /// <param name="userId">AlbumId</param>
        /// <param name="albumId">UserId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Video</returns>
        public async Task<Video> GetUserAlbumVideoAsync(long userId, long albumId, long clipId)
        {
            try
            {
                var request = GenerateAlbumVideosRequest(albumId, userId, clipId);
                var response = await request.ExecuteRequestAsync<Video>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user album video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving user album video.", ex);
            }
        }

        /// <summary>
        /// Get videos from album by AlbumId and UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Paginated videos</returns>
        public async Task<Paginated<Video>> GetUserAlbumVideosAsync(long userId, long albumId)
        {
            try
            {
                var request = GenerateAlbumVideosRequest(albumId, userId);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user album videos.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new Paginated<Video>
                    {
                        data = new List<Video>(),
                        page = 0,
                        total = 0
                    };
                }
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving user album videos.", ex);
            }
        }

        /// <summary>
        /// Get video by ClipId for UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Video</returns>
        public async Task<Video> GetUserVideoAsync(long userId, long clipId)
        {
            try
            {
                var request = GenerateVideosRequest(userId, clipId);
                var response = await request.ExecuteRequestAsync<Video>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving user video.", ex);
            }
        }

        /// <summary>
        /// Get videos by UserId and query asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="query">Search query</param>
        /// <returns>Paginated videos</returns>
        public async Task<Paginated<Video>> GetUserVideosAsync(long userId, string query = null)
        {
            return await GetUserVideosAsync(userId, null, null, query);
        }

        /// <summary>
        /// Get videos by UserId and query and page parameters asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="perPage">Number of items to show on each page. Max 50</param>
        /// <param name="query">Search query</param>
        /// <param name="page">The page number to show</param>
        /// <returns>Paginated videos</returns>
        public async Task<Paginated<Video>> GetUserVideosAsync(long userId, int? page, int? perPage, string query = null)
        {
            try
            {
                var request = GenerateVideosRequest(userId: userId, page: page, perPage: perPage, query: query);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user videos.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new Paginated<Video>
                    {
                        data = new List<Video>(),
                        page = 0,
                        total = 0
                    };
                }
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving user videos.", ex);
            }
        }

        /// <summary>
        /// Get video by ClipId asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <returns>Video</returns>
        public async Task<Video> GetVideoAsync(long clipId)
        {
            try
            {
                var request = GenerateVideosRequest(clipId: clipId);
                var response = await request.ExecuteRequestAsync<Video>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving account video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving account video.", ex);
            }
        }

        /// <summary>
        /// Get paginated video for current account asynchronously
        /// </summary>
        /// <returns>Paginated videos</returns>
        public async Task<Paginated<Video>> GetVideosAsync(int? page = null, int? perPage = null)
        {
            try
            {
                var request = GenerateVideosRequest(page: page, perPage: perPage);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving account videos.");

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error retrieving account videos.", ex);
            }
        }

        /// <summary>
        /// Update video metadata by ClipId asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="metaData">New video metadata</param>
        public async Task UpdateVideoMetadataAsync(long clipId, VideoUpdateMetadata metaData)
        {
            try
            {
                var request = GenerateVideoPatchRequest(clipId, metaData);
                var response = await request.ExecuteRequestAsync();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error updating user video metadata.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error updating user video metadata.", ex);
            }
        }

        /// <summary>
        /// Update allowed domain for clip asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="domain">Domain</param>
        public async Task UpdateVideoAllowedDomainAsync(long clipId, string domain)
        {
            try
            {
                var request = GenerateVideoAllowedDomainPatchRequest(clipId, domain);
                var response = await request.ExecuteRequestAsync();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error updating user video allowed domain.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error updating user video metadata.", ex);
            }
        }

        private IApiRequest GenerateVideosRequest(long? userId = null, long? clipId = null, int? page = null, int? perPage = null, string query = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = userId.HasValue
                ? clipId.HasValue ? Endpoints.UserVideo : Endpoints.UserVideos
                : clipId.HasValue ? Endpoints.Video : Endpoints.Videos;
            request.Method = HttpMethod.Get;
            request.Path = endpoint;

            if (userId.HasValue)
            {
                request.UrlSegments.Add("userId", userId.ToString());
            }
            if (clipId.HasValue)
            {
                request.UrlSegments.Add("clipId", clipId.ToString());
            }
            if (page.HasValue)
            {
                request.Query.Add("page", page.ToString());
            }
            if (perPage.HasValue)
            {
                request.Query.Add("per_page", perPage.ToString());
            }
            if (!string.IsNullOrEmpty(query))
            {
                request.Query.Add("query", query);
            }

            return request;
        }

        private IApiRequest GenerateAlbumVideosRequest(long albumId, long? userId = null, long? clipId = null, int? page = null, int? perPage = null, string sort = null, string direction = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = clipId.HasValue ? Endpoints.UserAlbumVideo : Endpoints.UserAlbumVideos;
            request.Method = HttpMethod.Get;
            request.Path = userId.HasValue ? endpoint : Endpoints.GetCurrentUserEndpoint(endpoint);

            request.UrlSegments.Add("albumId", albumId.ToString());
            if (userId.HasValue)
            {
                request.UrlSegments.Add("userId", userId.ToString());
            }
            if (clipId.HasValue)
            {
                request.UrlSegments.Add("clipId", clipId.ToString());
            }
            if (page.HasValue)
            {
                request.Query.Add("page", page.ToString());
            }
            if (perPage.HasValue)
            {
                request.Query.Add("per_page", perPage.ToString());
            }
            if (!string.IsNullOrEmpty(sort))
            {
                request.Query.Add("sort", sort);
            }
            if (!string.IsNullOrEmpty(direction))
            {
                request.Query.Add("direction", direction);
            }

            return request;
        }

        private IApiRequest GenerateVideoDeleteRequest(long clipId)
        {
            ThrowIfUnauthorized();

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Delete;
            request.Path = Endpoints.Video;

            request.UrlSegments.Add("clipId", clipId.ToString());

            return request;
        }

        private IApiRequest GenerateVideoPatchRequest(long clipId, VideoUpdateMetadata metaData)
        {
            ThrowIfUnauthorized();

            var request = ApiRequestFactory.GetApiRequest(AccessToken);
            request.Method = new HttpMethod("PATCH");
            request.Path = Endpoints.Video;

            request.UrlSegments.Add("clipId", clipId.ToString());
            var parameters = new Dictionary<string, string>();
            if (metaData.Name != null)
            {
                parameters["name"] = metaData.Name.Trim();
            }
            if (metaData.Description != null)
            {
                parameters["description"] = metaData.Description.Trim();
            }
            if (metaData.Privacy != VideoPrivacyEnum.Unknown)
            {
                parameters["privacy.view"] = metaData.Privacy.ToString().ToLower();
            }
            if (metaData.Privacy == VideoPrivacyEnum.Password)
            {
                parameters["password"] = metaData.Password;
            }
            if (metaData.EmbedPrivacy != VideoEmbedPrivacyEnum.Unknown)
            {
                parameters["privacy.embed"] = metaData.EmbedPrivacy.ToString().ToLower();
            }
            if (metaData.Comments != VideoCommentsEnum.Unknown)
            {
                parameters["privacy.comments"] = metaData.Comments.ToString().ToLower();
            }
            parameters["review_link"] = metaData.ReviewLinkEnabled.ToString().ToLower();
            parameters["privacy.download"] = metaData.AllowDownloadVideo ? "true" : "false";
            parameters["privacy.add"] = metaData.AllowAddToAlbumChannelGroup ? "true" : "false";
            request.Body = new FormUrlEncodedContent(parameters);

            return request;
        }

        private IApiRequest GenerateVideoAllowedDomainPatchRequest(long clipId, string domain)
        {
            ThrowIfUnauthorized();

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Put;
            request.Path = Endpoints.VideoAllowedDomain;

            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("domain", domain);

            return request;
        }
    }
}