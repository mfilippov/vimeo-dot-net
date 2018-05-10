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
        /// <inheritdoc />
        public async Task<Video> GetVideoAsync(long clipId, string[] fields = null)
        {
            try
            {
                var request = GenerateVideosRequest(clipId: clipId, fields: fields);
                var response = await request.ExecuteRequestAsync<Video>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving account video.", HttpStatusCode.NotFound);

                return response.StatusCode == HttpStatusCode.NotFound ? null : response.Content;
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

        /// <inheritdoc />
        public async Task<Paginated<Video>> GetVideosAsync(UserId userId, int? page, int? perPage, string query = null,
            string[] fields = null)
        {
            try
            {
                var request = GenerateVideosRequest(userId, page: page, perPage: perPage, query: query,
                    fields: fields);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user videos.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new Paginated<Video>
                    {
                        Data = new List<Video>(),
                        Page = 0,
                        Total = 0
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

        /// <inheritdoc />
        public async Task DeleteVideoAsync(long clipId)
        {
            try
            {
                var request = GenerateVideoDeleteRequest(clipId);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
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

        /// <inheritdoc />
        public async Task<Paginated<Video>> GetAlbumVideosAsync(UserId userId, long albumId, int? page = null,
            int? perPage = null, string sort = null, string direction = null, string[] fields = null)
        {
            try
            {
                var request = GenerateAlbumVideosRequest(albumId, page: page, perPage: perPage, sort: sort,
                    direction: direction, fields: fields);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving account album videos.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new Paginated<Video>
                    {
                        Data = new List<Video>(),
                        Page = 0,
                        Total = 0
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

        /// <inheritdoc />
        public async Task UpdateVideoMetadataAsync(long clipId, VideoUpdateMetadata metaData)
        {
            try
            {
                var request = GenerateVideoPatchRequest(clipId, metaData);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
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

        /// <inheritdoc />
        public async Task UpdateVideoAllowedDomainAsync(long clipId, string domain)
        {
            try
            {
                var request = GenerateVideoAllowedDomainPatchRequest(clipId, domain);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
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

        private IApiRequest GenerateVideosRequest(UserId userId = null, long? clipId = null, int? page = null,
            int? perPage = null, string query = null, string[] fields = null)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint;
            if (userId == null)
            {
                endpoint = clipId.HasValue ? Endpoints.Video : Endpoints.Videos;
            }
            else
            {
                if (userId == UserId.Me)
                {
                    endpoint = clipId.HasValue
                        ? Endpoints.GetCurrentUserEndpoint(Endpoints.UserVideo)
                        : Endpoints.GetCurrentUserEndpoint(Endpoints.UserVideos);
                }
                else
                {
                    endpoint = clipId.HasValue ? Endpoints.UserVideo : Endpoints.UserVideos;
                }
            }

            request.Method = HttpMethod.Get;
            request.Path = endpoint;

            if (userId != null && userId != UserId.Me)
            {
                request.UrlSegments.Add("userId", userId.ToString());
            }

            if (clipId.HasValue)
            {
                request.UrlSegments.Add("clipId", clipId.ToString());
            }

            if (fields != null)
            {
                foreach (var field in fields)
                {
                    request.Fields.Add(field);
                }
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

        private IApiRequest GenerateAlbumVideosRequest(long albumId, long? userId = null, long? clipId = null,
            int? page = null, int? perPage = null, string sort = null, string direction = null, string[] fields = null)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            var endpoint = clipId.HasValue ? Endpoints.UserAlbumVideo : Endpoints.UserAlbumVideos;
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

            if (fields != null)
            {
                foreach (var field in fields)
                {
                    request.Fields.Add(field);
                }
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

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Delete;
            request.Path = Endpoints.Video;

            request.UrlSegments.Add("clipId", clipId.ToString());

            return request;
        }

        private IApiRequest GenerateVideoPatchRequest(long clipId, VideoUpdateMetadata metaData)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
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

            if (metaData.Privacy != null)
            {
                parameters["privacy.view"] = metaData.Privacy.ToString().ToLower();
            }

            if (metaData.Privacy == VideoPrivacyEnum.Password)
            {
                parameters["password"] = metaData.Password;
            }

            if (metaData.EmbedPrivacy != null)
            {
                parameters["privacy.embed"] = metaData.EmbedPrivacy.ToString().ToLower();
            }

            if (metaData.Comments != null)
            {
                parameters["privacy.comments"] = metaData.Comments.ToString().ToLower();
            }

            if (metaData.ReviewLinkEnabled.HasValue)
            {
                parameters["review_link"] = metaData.ReviewLinkEnabled.Value ? "true" : "false";
            }

            if (metaData.AllowDownloadVideo.HasValue)
            {
                parameters["privacy.download"] = metaData.AllowDownloadVideo.Value ? "true" : "false";
            }

            if (metaData.AllowAddToAlbumChannelGroup.HasValue)
            {
                parameters["privacy.add"] = metaData.AllowAddToAlbumChannelGroup.Value ? "true" : "false";
            }

            request.Body = new FormUrlEncodedContent(parameters);

            return request;
        }

        private IApiRequest GenerateVideoAllowedDomainPatchRequest(long clipId, string domain)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Put;
            request.Path = Endpoints.VideoAllowedDomain;

            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("domain", domain);

            return request;
        }

        /// <inheritdoc />
        public async Task<Picture> GetPictureAsync(long clipId, long pictureId)
        {
            try
            {
                ThrowIfUnauthorized();
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Get;
                request.Path = Endpoints.Picture;
                request.UrlSegments.Add("clipId", clipId.ToString());
                request.UrlSegments.Add("pictureId", clipId.ToString());

                var response = await request.ExecuteRequestAsync<Picture>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving video picture.", HttpStatusCode.NotFound);

                return response.StatusCode == HttpStatusCode.NotFound ? null : response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error retrieving video picture.", ex);
            }
        }

        /// <inheritdoc />
        public async Task<Paginated<Picture>> GetPicturesAsync(long clipId)
        {
            try
            {
                ThrowIfUnauthorized();
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Get;
                request.Path = Endpoints.Pictures;
                request.UrlSegments.Add("clipId", clipId.ToString());

                var response = await request.ExecuteRequestAsync<Paginated<Picture>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving video picture.", HttpStatusCode.NotFound);

                return response.StatusCode == HttpStatusCode.NotFound ? null : response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error retrieving video picture.", ex);
            }
        }

        /// <summary>
        /// upload picture asynchronously
        /// </summary>
        /// <param name="fileContent">fileContent</param>
        /// <param name="clipId">Clip Id</param>
        /// <returns>upload pic </returns>
        private async Task<Picture> UploadPictureAsync(IBinaryContent fileContent, long clipId)
        {
            try
            {
                if (!fileContent.Data.CanRead)
                {
                    throw new ArgumentException("fileContent should be readable");
                }

                if (fileContent.Data.CanSeek && fileContent.Data.Position > 0)
                {
                    fileContent.Data.Position = 0;
                }

                ThrowIfUnauthorized();
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Post;
                request.Path = Endpoints.Pictures;
                request.UrlSegments.Add("clipId", clipId.ToString());

                request.Body = new ByteArrayContent(await fileContent.ReadAllAsync().ConfigureAwait(false));

                var response = await request.ExecuteRequestAsync<Picture>().ConfigureAwait(false);

                CheckStatusCodeError(null, response, "Error generating upload ticket to replace video.");

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error Uploading picture.", null, ex);
            }
        }


        /// <summary>
        /// set thumbnail picture asynchronously
        /// </summary>
        /// <param name="link">link</param>
        /// <returns>Set thumbnail pic </returns>
        private async Task SetThumbnailActiveAsync(string link)
        {
            try
            {
                ThrowIfUnauthorized();
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = new HttpMethod("PATCH");
                request.Path = link;
                request.Query.Add("active", "true");

                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);

                CheckStatusCodeError(null, response, "Error Setting thumbnail image active.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error Setting thumbnail image active.", null, ex);
            }
        }
    }
}