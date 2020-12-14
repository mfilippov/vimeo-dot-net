using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        public async Task<Folder> CreateVideoFolder(UserId userId, string name)
        {
            try
            {
                // Get the URI of the thumbnail
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Post;

                string endpoint;
                if (userId == UserId.Me)
                {
                    endpoint = Endpoints.GetCurrentUserEndpoint(Endpoints.UserFolders);
                }
                else
                {
                    endpoint = Endpoints.UserFolders;
                }

                if (userId != null && userId != UserId.Me)
                {
                    request.UrlSegments.Add("userId", userId.ToString());
                }

                request.Path = endpoint;

                var parameters = new Dictionary<string, string>
                {
                    ["name"] = name
                };
                
                request.Body = new FormUrlEncodedContent(parameters);

                var response = await request.ExecuteRequestAsync<Folder>().ConfigureAwait(false);
                CheckStatusCodeError(null, response, "Error creating folder.");

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error creating folder.", null, ex);
            }
        }
        
        public async Task<Paginated<Folder>> GetUserFolders(UserId userId, int? page, int? perPage, string query = null, string[] fields = null)
        {
            try
            {
                var request = GenerateFoldersRequest(userId, page: page, perPage: perPage, query: query, fields: fields);
                var response = await request.ExecuteRequestAsync<Paginated<Folder>>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving user folders.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new Paginated<Folder>
                    {
                        Data = new List<Folder>(),
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

                throw new VimeoApiException("Error retrieving user folders.", ex);
            }
        }

        private IApiRequest GenerateFoldersRequest(UserId userId = null, int? page = null, int? perPage = null, string query = null, string[] fields = null)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = Endpoints.GetCurrentUserEndpoint(Endpoints.UserFolders);

            request.Method = HttpMethod.Get;
            request.Path = endpoint;

            if (userId != null && userId != UserId.Me)
            {
                request.UrlSegments.Add("userId", userId.ToString());
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
    }


}
