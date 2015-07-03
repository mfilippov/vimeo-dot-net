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

        public async Task<Video> GetAlbumVideoAsync(long albumId, long clipId, string fieldsCsv = null)
        {
            try
            {
                IApiRequest request = GenerateAlbumVideosRequest(userId: null, albumId: albumId, clipId: clipId, fieldsCsv: fieldsCsv);
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

        public async Task<Paginated<Video>> GetAlbumVideosAsync(long albumId, int? page = null, int? perPage = null, string fieldsCsv = null)
        {
            try
            {
                IApiRequest request = GenerateAlbumVideosRequest(userId: null, albumId: albumId, page: page, perPage: perPage, fieldsCsv: fieldsCsv);
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

        public async Task<Video> GetUserAlbumVideoAsync(long userId, long albumId, long clipId, string fieldsCsv = null)
        {
            try
            {
                IApiRequest request = GenerateAlbumVideosRequest(userId, albumId, clipId: clipId);
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

        public async Task<Paginated<Video>> GetUserAlbumVideosAsync(long userId, long albumId, int? page = null, int? perPage = null, string fieldsCsv = null)
        {
            try
            {
                IApiRequest request = GenerateAlbumVideosRequest(userId, albumId, page: page, perPage: perPage, fieldsCsv: fieldsCsv);
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


        public async Task AddVideoToAlbumAsync(long? userId, long albumId, long clipId)
        {
            string errMsg = "Error adding video to album.";
            try
            {
                IApiRequest request = GenerateAlbumAddVideoRequest(userId, albumId, clipId);
                IRestResponse response = await request.ExecuteRequestAsync();
                CheckStatusCodeError(response, errMsg, HttpStatusCode.NotFound);

            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException(errMsg, ex);
            }
        }
        public async Task RemoveVideoFromAlbumAsynch(long? userId, long albumId, long clipId)
        {
            string errMsg = "Error adding video to album.";
            try
            {
                IApiRequest request = GenerateAlbumRemoveVideoRequest(userId, albumId, clipId);
                IRestResponse response = await request.ExecuteRequestAsync();
                CheckStatusCodeError(response, errMsg, HttpStatusCode.NotFound);

            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException(errMsg, ex);
            }
        }






        public async Task<Video> GetUserVideoAsync(long userId, long clipId, string fieldsCsv = null)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(userId, clipId, fieldsCsv: fieldsCsv);
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

        public async Task<Paginated<Video>> GetUserVideosAsync(long userId, string fieldsCsv = null)
        {
            return await GetUserVideosAsync(userId, null, null);
        }

        public async Task<Paginated<Video>> GetUserVideosAsync(long userId, int? page, int? perPage, string fieldsCsv = null)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(userId: userId, page: page, perPage: perPage, fieldsCsv: fieldsCsv);
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

        public async Task<Video> GetVideoAsync(long clipId, string fieldsCsv = null)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(clipId: clipId, fieldsCsv: fieldsCsv);
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

        public async Task<Paginated<Video>> GetVideosAsync(int? page = null, int? perPage = null, string fieldsCsv = null)
        {
            try
            {
                IApiRequest request = GenerateVideosRequest(page: page, perPage: perPage, fieldsCsv: fieldsCsv);
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

        private IApiRequest GenerateVideosRequest(long? userId = null, long? clipId = null, int? page = null, int? perPage = null, string fieldsCsv = null)
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
            if(!string.IsNullOrWhiteSpace(fieldsCsv))
            {
                request.Query.Add("fields", fieldsCsv);
            }

            return request;
        }



        private IApiRequest GenerateAlbumRequest(long? userId = null, long? albumId = null, int? page = null, int? perPage = null, string fieldsCsv = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = userId.HasValue
                ? albumId.HasValue ? Endpoints.UserAlbum : Endpoints.UserAlbums
                : albumId.HasValue ? Endpoints.Album : Endpoints.Albums;
            request.Method = Method.GET;
            request.Path = endpoint;

            if (userId.HasValue)
            {
                request.UrlSegments.Add("userId", userId.ToString());
            }
            if (albumId.HasValue)
            {
                request.UrlSegments.Add("clipId", albumId.ToString());
            }
            if (page.HasValue)
            {
                request.Query.Add("page", page.ToString());
            }
            if (perPage.HasValue)
            {
                request.Query.Add("per_page", perPage.ToString());
            }
            if (!string.IsNullOrWhiteSpace(fieldsCsv))
            {
                request.Query.Add("fields", fieldsCsv);
            }

            return request;
        }





        private IApiRequest GenerateAlbumVideosRequest(long? userId, long albumId, int? page = null, int? perPage = null, long? clipId = null, string fieldsCsv = null)
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
            if (page.HasValue)
            {
                request.Query.Add("page", page.ToString());
            }
            if (perPage.HasValue)
            {
                request.Query.Add("per_page", perPage.ToString());
            }
            if (!string.IsNullOrWhiteSpace(fieldsCsv))
            {
                request.Query.Add("fields", fieldsCsv);
            }

            return request;
        }




        private IApiRequest GenerateAlbumAddVideoRequest(long? userId, long albumId, long clipId)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = Endpoints.UserAlbumVideo;
            request.Method = Method.PUT;
            request.Path = userId.HasValue ? endpoint : Endpoints.GetCurrentUserEndpoint(endpoint);

            request.UrlSegments.Add("albumId", albumId.ToString());
            if (userId.HasValue)
            {
                request.UrlSegments.Add("userId", userId.ToString());
            }
            request.UrlSegments.Add("clipId", clipId.ToString());

            return request;
        }


        private IApiRequest GenerateAlbumRemoveVideoRequest(long? userId, long albumId , long clipId)
        {
            IApiRequest request = GenerateAlbumAddVideoRequest(userId, albumId, clipId);
            request.Method = Method.DELETE;

            return request;
        }


        public async Task<Paginated<Album>> GetAlbumsAsync(long? userId, int? page = null, int? perPage = null, string fieldsCsv = null)
        {
            string errMsg = "Error retrieving albums";

            try
            {
                IApiRequest request = GenerateAlbumRequest(userId:userId, page:page, perPage:perPage, fieldsCsv: fieldsCsv);

                IRestResponse<Paginated<Album>> response = await request.ExecuteRequestAsync<Paginated<Album>>();
                CheckStatusCodeError(response, errMsg);

                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException(errMsg, ex);
            }
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