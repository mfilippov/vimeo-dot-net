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
        public async Task<Folder> CreateFolder(UserId userId, string name, string parentFolderUri = null)
        {
            try
            {
                // Get the URI of the thumbnail
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Post;

                SetEndPoint(userId, request);

                var parameters = new Dictionary<string, string>
                {
                    ["name"] = name
                };

                if (string.IsNullOrWhiteSpace(parentFolderUri) == false)
                    parameters.Add("parent_folder_uri", parentFolderUri);

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
        
        public async Task DeleteFolder(UserId userId, long folderId)
        {
            try
            {
                var request = _apiRequestFactory.GetApiRequest(AccessToken);
                request.Method = HttpMethod.Delete;

                SetEndPoint(userId, request, Endpoints.UserFolder);
                request.UrlSegments.Add("folderId", folderId.ToString());

                var response = await request.ExecuteRequestAsync<Folder>().ConfigureAwait(false);
                CheckStatusCodeError(null, response, "Error deleting folder.");
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoUploadException("Error deleting folder.", null, ex);
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
                    return new Paginated<Folder> { Data = new List<Folder>() };
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

        public async Task<Paginated<Item>> GetFolderItems(UserId userId, long folderId, int? page = null, int? perPage = null, string query = null, string[] fields = null)
        {
            try
            {
                var request = GenerateFoldersRequest(userId, page: page, perPage: perPage, query: query, fields: fields, Endpoints.ProjectItems);
                request.UrlSegments.Add("projectId", folderId.ToString());
                IApiResponse<Paginated<Item>> response = await request.ExecuteRequestAsync<Paginated<Item>>().ConfigureAwait(false);
                UpdateRateLimit(response);

                return response.Content;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }

                throw new VimeoApiException("Error retrieving user root items.", ex);
            }
        }

        private IApiRequest GenerateFoldersRequest(UserId userId = null, int? page = null, int? perPage = null, string query = null, string[] fields = null, string url = Endpoints.UserFolders)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Get;

            SetEndPoint(userId, request, url);

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

        private static void SetEndPoint(UserId userId, IApiRequest request, string url = Endpoints.UserFolders)
        {
            string endpoint;
            if (userId == null || userId == UserId.Me)
            {
                endpoint = Endpoints.GetCurrentUserEndpoint(url);
            }
            else
            {
                endpoint = url;
                request.UrlSegments.Add("userId", userId.ToString());
            }

            request.Path = endpoint;
        }
    }
}
