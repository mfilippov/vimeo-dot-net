using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
        public async Task<TextTracks> GetTextTracksAsync(long clipId)
        {
            try
            {
                IApiRequest request = GenerateTextTracksRequest(clipId);
                IRestResponse<TextTracks> response = await request.ExecuteRequestAsync<TextTracks>();
                CheckStatusCodeError(response, "Error retrieving text tracks for video.", HttpStatusCode.NotFound);

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
                throw new VimeoApiException("Error retrieving text tracks for video.", ex);
            }
        }

        public async Task<TextTrack> GetTextTrackAsync(long clipId, long trackId)
        {
            try
            {
                IApiRequest request = GenerateTextTracksRequest(clipId, trackId);
                IRestResponse<TextTrack> response = await request.ExecuteRequestAsync<TextTrack>();
                CheckStatusCodeError(response, "Error retrieving text track for video.", HttpStatusCode.NotFound);

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
                throw new VimeoApiException("Error retrieving text track for video.", ex);
            }
        }

        public async Task<TextTrack> UploadTextTrackFileAsync(IBinaryContent fileContent, long videoId, TextTrack track)
        {
            if (!fileContent.Data.CanRead)
            {
                throw new ArgumentException("fileContent should be readable");
            }
            if (fileContent.Data.CanSeek && fileContent.Data.Position > 0)
            {
                fileContent.Data.Position = 0;
            }

            TextTrack ticket = await GetUploadTextTrackTicketAsync(videoId, track);

            IApiRequest request = _apiRequestFactory.GetApiRequest();
            request.Method = Method.PUT;
            request.ExcludeAuthorizationHeader = true;
            request.Path = ticket.link;
            request.Headers.Add(Request.HeaderContentType, fileContent.ContentType);
            request.Headers.Add(Request.HeaderContentLength, fileContent.Data.Length.ToString());
            request.BinaryContent = await fileContent.ReadAllAsync();

            IRestResponse response = await request.ExecuteRequestAsync();
            CheckStatusCodeError(null, response, "Error uploading text track file.", HttpStatusCode.BadRequest);

            return ticket;
        }

        public async Task<TextTrack> UpdateTextTrackAsync(long clipId, long trackId, TextTrack track)
        {
            try
            {
                IApiRequest request = GenerateUpdateTextTrackRequest(clipId, trackId, track);
                IRestResponse<TextTrack> response = await request.ExecuteRequestAsync<TextTrack>();
                CheckStatusCodeError(response, "Error updating text track for video.", HttpStatusCode.NotFound);

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
                throw new VimeoApiException("Error updating text track for video.", ex);
            }
        }

        public async Task DeleteTextTrackAsync(long clipId, long trackId)
        {
            try
            {
                IApiRequest request = GenerateDeleteTextTrackRequest(clipId, trackId);
                IRestResponse<TextTrack> response = await request.ExecuteRequestAsync<TextTrack>();
                CheckStatusCodeError(response, "Error updating text track for video.", HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoApiException("Error updating text track for video.", ex);
            }
        }

        private async Task<TextTrack> GetUploadTextTrackTicketAsync(long clipId, TextTrack track)
        {
            try
            {
                IApiRequest request = GenerateUploadTextTrackTicketRequest(clipId, track);

                IRestResponse<TextTrack> response = await request.ExecuteRequestAsync<TextTrack>();
                CheckStatusCodeError(null, response, "Error generating upload text track ticket.");

                return response.Data;
            }
            catch (Exception ex)
            {
                if (ex is VimeoApiException)
                {
                    throw;
                }
                throw new VimeoUploadException("Error generating upload text track ticket.", null, ex);
            }
        }

        private IApiRequest GenerateUploadTextTrackTicketRequest(long clipId, TextTrack track)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.POST;
            request.Path = Endpoints.TextTracks;
            request.UrlSegments.Add("clipId", clipId.ToString());

            if (track != null)
            {
                request.Query.Add("active", track.active.ToString().ToLower());
                if (track.name != null)
                {
                    request.Query.Add("name", track.name);
                }
                if (track.language != null)
                {
                    request.Query.Add("language", track.language);
                }
                if (track.type != null)
                {
                    request.Query.Add("type", track.type);
                }
            }

            return request;
        }

        private IApiRequest GenerateUpdateTextTrackRequest(long clipId, long trackId, TextTrack track)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = Method.PATCH;
            request.Path = Endpoints.TextTrack;
            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("trackId", trackId.ToString());

            if (track != null)
            {
                request.Query.Add("active", track.active.ToString().ToLower());
                if (track.name != null)
                {
                    request.Query.Add("name", track.name);
                }
                if (track.language != null)
                {
                    request.Query.Add("language", track.language);
                }
                if (track.type != null)
                {
                    request.Query.Add("type", track.type);
                }
            }

            return request;
        }

        private IApiRequest GenerateTextTracksRequest(long clipId, long? trackId = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = trackId.HasValue ? Endpoints.TextTrack : Endpoints.TextTracks;
            request.Method = Method.GET;
            request.Path = endpoint;

            request.UrlSegments.Add("clipId", clipId.ToString());
            if (trackId.HasValue)
            {
                request.UrlSegments.Add("trackId", trackId.ToString());
            }
            return request;
        }

        private IApiRequest GenerateDeleteTextTrackRequest(long clipId, long trackId)
        {
            ThrowIfUnauthorized();

            IApiRequest request = _apiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = Endpoints.TextTrack;
            request.Method = Method.DELETE;
            request.Path = endpoint;

            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("trackId", trackId.ToString());

            return request;
        }
    }
}