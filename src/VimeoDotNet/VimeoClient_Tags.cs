using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        /// <inheritdoc />
        public async Task<Tag> AddVideoTagAsync(long clipId, string tag)
        {
            try
            {
                ThrowIfUnauthorized();

                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Put;
                request.Path = Endpoints.VideoTag;
                request.UrlSegments.Add("clipId", clipId.ToString());
                request.UrlSegments.Add("tagId", tag);

                var response = await request.ExecuteRequestAsync<Tag>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Cannot create tag");

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Cannot create tag.", ex);
            }
        }

        /// <inheritdoc />
        public async Task DeleteVideoTagAsync(long clipId, string tag)
        {
            try
            {
                ThrowIfUnauthorized();

                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Delete;
                request.Path = Endpoints.VideoTag;
                request.UrlSegments.Add("clipId", clipId.ToString());
                request.UrlSegments.Add("tagId", tag);

                var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Cannot delete tag");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Cannot delete tag", ex);
            }
        }

        /// <inheritdoc />
        public async Task<Paginated<Tag>> GetVideoTags(long clipId, int? page = null, int? perPage = null)
        {
            try
            {
                ThrowIfUnauthorized();

                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Get;
                request.Path = Endpoints.VideoTags;
                request.UrlSegments.Add("clipId", clipId.ToString());
                if (page != null)
                {
                    request.Query["page"] = page.ToString();
                }

                if (perPage != null)
                {
                    request.Query["per_page"] = perPage.ToString();
                }

                var response = await request.ExecuteRequestAsync<Paginated<Tag>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Cannot get tags");
                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Cannot get tags", ex);
            }
        }

        /// <inheritdoc />
        public async Task<Tag> GetVideoTagAsync(string tag)
        {
            try
            {
                ThrowIfUnauthorized();

                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Get;
                request.Path = Endpoints.Tag;
                request.UrlSegments.Add("tagId", tag);

                var response = await request.ExecuteRequestAsync<Tag>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Cannot get tag");

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Cannot get tag", ex);
            }
        }

        /// <inheritdoc />
        public async Task<Paginated<Video>> GetVideoByTag(string tag, int? page = null,
            int? perPage = null, GetVideoByTagSort ?sort = null, GetVideoByTagDirection? direction = null, string[] fields = null)
        {
            try
            {
                ThrowIfUnauthorized();

                var request = _apiRequestFactory.GetApiRequest(AccessToken);

                request.Method = HttpMethod.Get;
                request.Path = Endpoints.VideosByTag;

                request.UrlSegments.Add("tagId", tag);

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

                if (sort.HasValue)
                {
                    request.Query.Add("sort", sort.Value.GetStringValue());
                }

                if (direction.HasValue)
                {
                    request.Query.Add("direction", direction.Value.GetStringValue());
                }

                var response = await request.ExecuteRequestAsync<Paginated<Video>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving videos by tag", HttpStatusCode.NotFound);

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

                throw new VimeoApiException("Cannot get video by tag", ex);
            }
        }
    }
}