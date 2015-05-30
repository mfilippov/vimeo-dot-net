using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient : IVimeoClient
    {
        public async Task DeleteVideoAsync(long clipId)
        {
            try
            {
                IApiRequest request = GenerateVideoDeleteRequest(clipId);
                IRestResponse response = await request.ExecuteRequestAsync();
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

        public async Task<Video> GetAlbumVideoAsync(long albumId, long clipId)
        {
            try
            {
                IApiRequest request = GenerateAlbumVideosRequest(albumId, clipId: clipId);
                IRestResponse<Video> response = await request.ExecuteRequestAsync<Video>();
                CheckStatusCodeError(response, "Error retrieving user album video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Data;
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

        public async Task<Paginated<Video>> GetAlbumVideosAsync(long albumId)
        {
            try
            {
                IApiRequest request = GenerateAlbumVideosRequest(albumId);
                IRestResponse<Paginated<Video>> response = await request.ExecuteRequestAsync<Paginated<Video>>();
                CheckStatusCodeError(response, "Error retrieving account album videos.");

                return response.Data;
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

        public async Task<Video> GetUserAlbumVideoAsync(long userId, long albumId, long clipId)
        {
            try
            {
                IApiRequest request = GenerateAlbumVideosRequest(albumId, userId, clipId);
                IRestResponse<Video> response = await request.ExecuteRequestAsync<Video>();
                CheckStatusCodeError(response, "Error retrieving user album video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Data;
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

        public async Task<Paginated<Video>> GetUserAlbumVideosAsync(long userId, long albumId)
        {
            try
            {
                IApiRequest request = GenerateAlbumVideosRequest(albumId, userId);
                IRestResponse<Paginated<Video>> response = await request.ExecuteRequestAsync<Paginated<Video>>();
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
                return response.Data;
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

        public async Task<Video> GetUserVideoAsync(long userId, long clipId)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(userId, clipId);
                IRestResponse<Video> response = await request.ExecuteRequestAsync<Video>();
                CheckStatusCodeError(response, "Error retrieving user video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Data;
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

        public async Task<Paginated<Video>> GetUserVideosAsync(long userId)
        {
            return await GetUserVideosAsync(userId, null, null);
        }

        public async Task<Paginated<Video>> GetUserVideosAsync(long userId, int? page, int? perPage)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(userId: userId, page: page, perPage: perPage);
                IRestResponse<Paginated<Video>> response = await request.ExecuteRequestAsync<Paginated<Video>>();
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
                return response.Data;
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

        public async Task<Video> GetVideoAsync(long clipId)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(clipId: clipId);
                IRestResponse<Video> response = await request.ExecuteRequestAsync<Video>();
                CheckStatusCodeError(response, "Error retrieving account video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Data;
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

        public async Task<Paginated<Video>> GetVideosAsync(int? page = null, int? perPage = null)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(page: page, perPage: perPage);
                IRestResponse<Paginated<Video>> response = await request.ExecuteRequestAsync<Paginated<Video>>();
                CheckStatusCodeError(response, "Error retrieving account videos.");

                return response.Data;
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

        public async Task UpdateVideoMetadataAsync(long clipId, VideoUpdateMetadata metaData)
        {
            try
            {
                IApiRequest request = GenerateVideoPatchRequest(clipId, metaData);
                IRestResponse response = await request.ExecuteRequestAsync();
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
        
        private IApiRequest GenerateVideosRequest(long? userId = null, long? clipId = null, int? page = null, int? perPage = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = userId.HasValue
                ? clipId.HasValue ? Endpoints.UserVideo : Endpoints.UserVideos
                : clipId.HasValue ? Endpoints.Video : Endpoints.Videos;
            request.Method = Method.GET;
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

            return request;
        }

        private IApiRequest GenerateAlbumVideosRequest(long albumId, long? userId = null, long? clipId = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = clipId.HasValue ? Endpoints.UserAlbumVideo : Endpoints.UserAlbumVideos;
            request.Method = Method.GET;
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

            return request;
        }

        private IApiRequest GenerateVideoDeleteRequest(long clipId)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.DELETE;
            request.Path = Endpoints.Video;

            request.UrlSegments.Add("clipId", clipId.ToString());

            return request;
        }

        private IApiRequest GenerateVideoPatchRequest(long clipId, VideoUpdateMetadata metaData)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.PATCH;
            request.Path = Endpoints.Video;

            request.UrlSegments.Add("clipId", clipId.ToString());
            if (metaData.Name != null)
            {
                request.Query.Add("name", metaData.Name.Trim());
            }
            if (metaData.Description != null)
            {
                request.Query.Add("description", metaData.Description.Trim());
            }
            if (metaData.Privacy != VideoPrivacyEnum.Unknown)
            {
                request.Query.Add("privacy.view", metaData.Privacy.ToString().ToLower());
            }
            if (metaData.Privacy == VideoPrivacyEnum.Password)
            {
                request.Query.Add("password", metaData.Password);
            }
            if (metaData.EmbedPrivacy != VideoEmbedPrivacyEnum.Unknown)
            {
                request.Query.Add("privacy.embed", metaData.EmbedPrivacy.ToString().ToLower());
            }
            request.Query.Add("review_link", metaData.ReviewLinkEnabled.ToString().ToLower());

            return request;
        }
    }
}