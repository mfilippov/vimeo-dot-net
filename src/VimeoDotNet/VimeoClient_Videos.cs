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
    /// <summary>
    /// Class VimeoClient.
    /// Implements the <see cref="VimeoDotNet.IVimeoClient" />
    /// </summary>
    /// <seealso cref="VimeoDotNet.IVimeoClient" />
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
                    direction: direction, fields: fields, userId: userId);
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

        /// <summary>
        /// Allow embed video on domain as an asynchronous operation.
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="domain">Domain</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error allowing domain for embedding video.</exception>
        /// <seealso cref="DisallowEmbedVideoOnDomainAsync(long, string)" />
        /// <seealso cref="GetAllowedDomainsForEmbeddingVideoAsync(long)" />
        /// <remarks>The call is valid only when video embed privacy is set to
        /// <see cref="VideoEmbedPrivacyEnum.Whitelist" />.
        /// Use <see cref="UpdateVideoMetadataAsync(long, VideoUpdateMetadata)" /> and
        /// <see cref="VideoUpdateMetadata.EmbedPrivacy" /> property to change this setting.</remarks>
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

        /// <summary>
        /// Disallow embed video on domain as an asynchronous operation.
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="domain">Domain</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error disallowing domain for embedding video.</exception>
        /// <seealso cref="AllowEmbedVideoOnDomainAsync(long, string)" />
        /// <seealso cref="GetAllowedDomainsForEmbeddingVideoAsync(long)" />
        /// <remarks>The call is valid only when video embed privacy is set to
        /// <see cref="VideoEmbedPrivacyEnum.Whitelist" />.
        /// Use <see cref="UpdateVideoMetadataAsync(long, VideoUpdateMetadata)" /> and
        /// <see cref="VideoUpdateMetadata.EmbedPrivacy" /> property to change this setting.</remarks>
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

        /// <summary>
        /// Get allowed domains for embedding video as an asynchronous operation.
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <returns>A Task&lt;Paginated`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error retrieving allowed domain for embedding video.</exception>
        /// <seealso cref="AllowEmbedVideoOnDomainAsync(long, string)" />
        /// <seealso cref="DisallowEmbedVideoOnDomainAsync(long, string)" />
        /// <remarks>The call is valid only when video embed privacy is set to
        /// <see cref="VideoEmbedPrivacyEnum.Whitelist" />.
        /// Use <see cref="UpdateVideoMetadataAsync(long, VideoUpdateMetadata)" /> and
        /// <see cref="VideoUpdateMetadata.EmbedPrivacy" /> property to change this setting.</remarks>
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

        /// <summary>
        /// Generates the videos request.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="page">The page.</param>
        /// <param name="perPage">The per page.</param>
        /// <param name="query">The query.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>IApiRequest.</returns>
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

        /// <summary>
        /// Generates the album videos request.
        /// </summary>
        /// <param name="albumId">The album identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="page">The page.</param>
        /// <param name="perPage">The per page.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateAlbumVideosRequest(long albumId, UserId userId = null, long? clipId = null,
            int? page = null, int? perPage = null, string sort = null, string direction = null, string[] fields = null)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            var endpoint = clipId.HasValue ? Endpoints.UserAlbumVideo : Endpoints.UserAlbumVideos;
            request.Method = HttpMethod.Get;
            request.Path = userId == UserId.Me ? Endpoints.GetCurrentUserEndpoint(endpoint) : endpoint;

            request.UrlSegments.Add("albumId", albumId.ToString());
            if (userId != null)
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

        /// <summary>
        /// Generates the video delete request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateVideoDeleteRequest(long clipId)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Delete;
            request.Path = Endpoints.Video;

            request.UrlSegments.Add("clipId", clipId.ToString());

            return request;
        }

        /// <summary>
        /// Generates the video patch request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateVideoPatchRequest(long clipId, VideoUpdateMetadata metaData)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = new HttpMethod("PATCH");
            request.Path = Endpoints.Video;

            request.UrlSegments.Add("clipId", clipId.ToString());
            var parameters = metaData.GetParameterValues();

            request.Body = new FormUrlEncodedContent(parameters);

            return request;
        }

        /// <summary>
        /// Generates the video allowed domain request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="allow">if set to <c>true</c> [allow].</param>
        /// <returns>IApiRequest.</returns>
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

        /// <summary>
        /// Generates the video allowed domains request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <returns>IApiRequest.</returns>
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
        /// <returns>upload pic</returns>
        /// <exception cref="System.ArgumentException">fileContent should be readable</exception>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error getting thumbnail link or uri.</exception>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoUploadException">Error Uploading picture. - null</exception>
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
                if (link == null || uri == null)
                {
                    throw new VimeoApiException("Error getting thumbnail link or uri.");
                }

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
        /// Set a Video Thumbnail by a time code
        /// </summary>
        /// <param name="timeOffset">Time offset for the thumbnail in seconds</param>
        /// <param name="clipId">Clip Id</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoUploadException">Error setting thumbnail. - null</exception>
        public async Task SetThumbnailAsync(long timeOffset, long clipId)
        {
            try
            {
                // Get the URI of the thumbnail
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Post;
                request.Path = Endpoints.Pictures;
                request.UrlSegments.Add("clipId", clipId.ToString());
                var parameters = new Dictionary<string, string>();
                parameters.Add("active", "true");
                parameters.Add("time", timeOffset.ToString());

                request.Body = new FormUrlEncodedContent(parameters);

                var response = await request.ExecuteRequestAsync<Video>().ConfigureAwait(false);
                CheckStatusCodeError(null, response, "Error setting thumbnail.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error setting thumbnail.", null, ex);
            }
        }


        /// <summary>
        /// set thumbnail picture asynchronously
        /// </summary>
        /// <param name="link">link</param>
        /// <returns>Set thumbnail pic</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoUploadException">Error Setting thumbnail image active. - null</exception>
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
                    {"active", "true"}
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

        /// <summary>
        /// Assign embed preset to video as an asynchronous operation.
        /// </summary>
        /// <param name="clipId">Clip ID</param>
        /// <param name="presetId">Preset ID</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error assigning embed preset to video.</exception>
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

        /// <summary>
        /// Unassign embed preset from video as an asynchronous operation.
        /// </summary>
        /// <param name="clipId">Clip ID</param>
        /// <param name="presetId">Preset ID</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error unassigning embed preset from video.</exception>
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
                    CheckStatusCodeError(response, "Error unassigning embed preset from video.",
                        HttpStatusCode.NotFound);
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

        /// <summary>
        /// Generates the video preset request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="presetId">The preset identifier.</param>
        /// <param name="assign">if set to <c>true</c> [assign].</param>
        /// <returns>IApiRequest.</returns>
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

        /// <summary>
        /// Moves a video to a folder
        /// </summary>
        /// <param name="projectId">Folder Id (called project in Vimeo)</param>
        /// <param name="clipId">Clip Id</param>
        /// <returns>Task.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoUploadException">Error moving  video to folder. - null</exception>
        public async Task MoveVideoToFolder(long projectId, long clipId)
        {
            try
            {
                // Get the URI of the thumbnail
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Put;
                request.Path = Endpoints.MeProjectVideo;
                request.UrlSegments.Add("clipId", clipId.ToString());
                request.UrlSegments.Add("projectId", projectId.ToString());

                var response = await request.ExecuteRequestAsync<Video>().ConfigureAwait(false);
                CheckStatusCodeError(null, response, "Error moving  video to folder.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error moving  video to folder.", null, ex);
            }
        }

        /// <inheritdoc />
        public async Task<Paginated<Video>> GetAllVideosFromFolderAsync(long projectId, UserId userId,
            long? clipId = null, int? page = null,
            int? perPage = null, string query = null,string direction = null, string[] fields = null)
        {
            try
            {
                var request = GenerateVideosFolderRequest(projectId, userId, clipId: clipId, query: query, page: page,
                    perPage: perPage,
                    direction: direction, fields: fields);
                var response = await request.ExecuteRequestAsync<Paginated<Video>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving account folder videos.", HttpStatusCode.NotFound);

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

                throw new VimeoApiException("Error retrieving account folder videos.", ex);
            }
        }

        /// <summary>
        /// Generates the videos folder request.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="page">The page.</param>
        /// <param name="perPage">The per page.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="query">The query.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateVideosFolderRequest(long projectId, UserId userId, long? clipId = null,
            int? page = null,
            int? perPage = null, string sort = null, string direction = null, string query = null,
            string[] fields = null)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint;

            if (userId == UserId.Me)
            {
                endpoint = clipId.HasValue
                    ? Endpoints.GetCurrentUserEndpoint(Endpoints.ProjectVideo)
                    : Endpoints.GetCurrentUserEndpoint(Endpoints.ProjectVideos);
            }
            else
            {
                endpoint = clipId.HasValue ? Endpoints.ProjectVideo : Endpoints.ProjectVideos;
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

            request.UrlSegments.Add("projectId", projectId.ToString());

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

            if (!string.IsNullOrEmpty(query))
            {
                request.Query.Add("query", query);
            }

            return request;
        }

        /// <summary>
        /// Delete thumbnail video as an asynchronous operation.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="pictureId">The picture identifier.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoApiException">Error deleting user video thumbnail metadata.</exception>
        public async Task DeleteThumbnailVideoAsync(long clipId, long pictureId)
        {
            try
            {
                var request = GenerateThumbnailDeleteRequest(clipId, pictureId);
                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error deleting thumbnail.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error deleting user video thumbnail metadata.", ex);
            }
        }

        /// <summary>
        /// Generates the thumbnail delete request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="pictureId">The picture identifier.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateThumbnailDeleteRequest(long clipId, long pictureId)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Delete;
            request.Path = Endpoints.Thumbnail;

            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("pictureId", pictureId.ToString());

            return request;
        }
    }
}
