using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        /// <inheritdoc />
        public async Task<EmbedPresets> GetEmbedPresetAsync(UserId userId, long presetId, string[] fields = null)
        {
            try
            {
                var request = GenerateEmbedPresetsRequest(userId: userId, presetId: presetId, fields: fields);
                var response = await request.ExecuteRequestAsync<EmbedPresets>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user embed preset.", HttpStatusCode.NotFound);

                return response.StatusCode == HttpStatusCode.NotFound ? null : response.Content;
            }
            catch (VimeoApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VimeoApiException("Error retrieving user embed preset.", ex);
            }
        }

        /// <inheritdoc />
        public async Task<Paginated<EmbedPresets>> GetEmbedPresetsAsync(UserId userId, int? page = null, int? perPage = null, string[] fields = null)
        {
            try
            {
                var request = GenerateEmbedPresetsRequest(userId: userId, page: page, perPage: perPage, fields: fields);
                var response = await request.ExecuteRequestAsync<Paginated<EmbedPresets>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user embed presets.", HttpStatusCode.NotFound);

                return response.StatusCode == HttpStatusCode.NotFound ? null : response.Content;
            }
            catch (VimeoApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VimeoApiException("Error retrieving user embed presets.", ex);
            }
        }

        private IApiRequest GenerateEmbedPresetsRequest(UserId userId, long? presetId = null, int? page = null,
            int? perPage = null, string[] fields = null)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint;
            if (userId?.IsMe == true)
            {
                endpoint = presetId.HasValue
                    ? Endpoints.GetCurrentUserEndpoint(Endpoints.UserPreset)
                    : Endpoints.GetCurrentUserEndpoint(Endpoints.UserPresets);
            }
            else
            {
                endpoint = presetId.HasValue ? Endpoints.UserPreset : Endpoints.UserPresets;
            }

            request.Method = HttpMethod.Get;
            request.Path = endpoint;

            if (userId?.IsMe == false)
            {
                request.UrlSegments.Add("userId", userId.ToString());
            }

            if (presetId.HasValue)
            {
                request.UrlSegments.Add("presetId", presetId.ToString());
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

            return request;
        }
    }
}