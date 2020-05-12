using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task AllowEmbedVideoOnDomainAsync(long clipId, string domain)
        {
            try
            {
                var request = GenerateVideoAllowedDomainRequest(clipId, domain, allow: true);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error allowing domain for embedding video.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error allowing domain for embedding video.", ex);
            }
        }

        public async Task DisallowEmbedVideoOnDomainAsync(long clipId, string domain)
        {
            try
            {
                var request = GenerateVideoAllowedDomainRequest(clipId, domain, allow: false);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error disallowing domain for embedding video.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error disallowing domain for embedding video.", ex);
            }
        }

        public async Task<Paginated<DomainForEmbedding>> GetAllowedDomainsForEmbeddingVideoAsync(long clipId)
        {
            try
            {
                var request = GenerateVideoAllowedDomainsRequest(clipId);
                var response = await request.ExecuteRequestAsync<Paginated<DomainForEmbedding>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving allowed domains for embedding video.");
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error retrieving allowed domain for embedding video.", ex);
            }
        }

        /// <inheritdoc />
        public Task UpdateVideoAllowedDomainAsync(long clipId, string domain)
        {
            return AllowEmbedVideoOnDomainAsync(clipId, domain);
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

        private IApiRequest GenerateVideoAllowedDomainRequest(long clipId, string domain, bool allow)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = allow ? HttpMethod.Put : HttpMethod.Delete;
            request.Path = Endpoints.VideoAllowedDomain;

            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("domain", domain);

            return request;
        }

        private IApiRequest GenerateVideoAllowedDomainsRequest(long clipId)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Get;
            request.Path = Endpoints.VideoAllowedDomains;

            request.UrlSegments.Add("clipId", clipId.ToString());

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
                request.UrlSegments.Add("pictureId", pictureId.ToString());

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
        private async Task<string> UploadPictureAsync(IBinaryContent fileContent, long clipId)
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

                // Get the URI of the thumbnail
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Get;
                request.Path = Endpoints.Video;
                request.UrlSegments.Add("clipId", clipId.ToString());


                var response = await request.ExecuteRequestAsync<Video>().ConfigureAwait(false);
                CheckStatusCodeError(null, response, "Error getting video settings.");

                // Get the upload link for the thumbnail
                var postRequest = _apiRequestFactory.AuthorizedRequest(
                                     AccessToken,
                                     HttpMethod.Post,
                                     response.Content.Metadata.Connections.Pictures.Uri,
                                     null
                                    );
                
                var postResponse = await postRequest.ExecuteRequestAsync().ConfigureAwait(false);
                CheckStatusCodeError(null, postResponse, "Error posting thumbnail placeholder.");
                JObject.Parse(postResponse.Text).TryGetValue("link", out var link);
                JObject.Parse(postResponse.Text).TryGetValue("uri", out var uri);

                // Upload the thumbnail image file
                var putRequest = new NonApiRequest
                {
                    Path = link.ToString(),
                    Method = HttpMethod.Put,
                    Body = new ByteArrayContent(await fileContent.ReadAllAsync().ConfigureAwait(false))
                };
                var putResponse = await putRequest.ExecuteRequestAsync().ConfigureAwait(false);
                CheckStatusCodeError(null, putResponse, "Error putting thumbnail data.");

                return uri.ToString();
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
                var parameters = new Dictionary<string, string>
                {
                    {"active" ,"true"}
                };
                request.Body = new FormUrlEncodedContent(parameters);

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

        public async Task AssignEmbedPresetToVideoAsync(long clipId, long presetId)
        {
            try
            {
                var request = GenerateVideoPresetRequest(clipId, presetId, assign: true);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error assigning embed preset to video.");
            }
            catch (VimeoApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VimeoApiException("Error assigning embed preset to video.", ex);
            }
        }

        public async Task UnassignEmbedPresetFromVideoAsync(long clipId, long presetId)
        {
            try
            {
                var request = GenerateVideoPresetRequest(clipId, presetId, assign: false);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                UpdateRateLimit(response);

                var presetNotFound = response.StatusCode == HttpStatusCode.NotFound && response.Text.Contains("preset");
                if (presetNotFound)
                {
                    // Ignore if preset not found
                    CheckStatusCodeError(response, "Error unassigning embed preset from video.", HttpStatusCode.NotFound);
                }
                else
                {
                    CheckStatusCodeError(response, "Error unassigning embed preset from video.");
                }
            }
            catch (VimeoApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VimeoApiException("Error unassigning embed preset from video.", ex);
            }
        }

        private IApiRequest GenerateVideoPresetRequest(long clipId, long presetId, bool assign)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = assign ? HttpMethod.Put : HttpMethod.Delete;
            request.Path = Endpoints.VideoPreset;

            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("presetId", presetId.ToString());

            return request;
        }
    }
}