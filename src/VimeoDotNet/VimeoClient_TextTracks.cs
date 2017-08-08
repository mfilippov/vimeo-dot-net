using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Get text tracks asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <returns>Return text tracks</returns>
        ///
        public async Task<TextTracks> GetTextTracksAsync(long videoId)
        {
            try
            {
                var request = GenerateTextTracksRequest(videoId);
                var response = await request.ExecuteRequestAsync<TextTracks>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving text tracks for video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Content;
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

        /// <summary>
        /// Get text track asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <param name="trackId">TrackId</param>
        /// <returns>Return text track</returns>
        public async Task<TextTrack> GetTextTrackAsync(long videoId, long trackId)
        {
            try
            {
                var request = GenerateTextTracksRequest(videoId, trackId);
                var response = await request.ExecuteRequestAsync<TextTrack>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving text track for video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Content;
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

        /// <summary>
        /// Upload new text track file asynchronously
        /// </summary>
        /// <param name="fileContent">File content</param>
        /// <param name="videoId">VideoId</param>
        /// <param name="track">Track</param>
        /// <returns>New text track</returns>
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
            IApiRequest request = ApiRequestFactory.GetApiRequest();
            request.Method = HttpMethod.Put;
            request.ExcludeAuthorizationHeader = true;
            request.Path = ticket.link;
            
            request.Body = new ByteArrayContent(await fileContent.ReadAllAsync());

            var response = await request.ExecuteRequestAsync();
            CheckStatusCodeError(null, response, "Error uploading text track file.", HttpStatusCode.BadRequest);

            return ticket;
        }

        /// <summary>
        /// Update text track asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <param name="trackId">TrackId</param>
        /// <param name="track">TextTrack</param>
        /// <returns>Updated text track</returns>
        public async Task<TextTrack> UpdateTextTrackAsync(long videoId, long trackId, TextTrack track)
        {
            try
            {
                var request = GenerateUpdateTextTrackRequest(videoId, trackId, track);
                var response = await request.ExecuteRequestAsync<TextTrack>();
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error updating text track for video.", HttpStatusCode.NotFound);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                return response.Content;
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

        /// <summary>
        /// Delete text track asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <param name="trackId">TrackId</param>
        /// <returns></returns>
        public async Task DeleteTextTrackAsync(long videoId, long trackId)
        {
            try
            {
                var request = GenerateDeleteTextTrackRequest(videoId, trackId);
                var response = await request.ExecuteRequestAsync<TextTrack>();
                UpdateRateLimit(response);
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
                var request = GenerateUploadTextTrackTicketRequest(clipId, track);
                var response = await request.ExecuteRequestAsync<TextTrack>();
                UpdateRateLimit(response);
                CheckStatusCodeError(null, response, "Error generating upload text track ticket.");

                return response.Content;
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

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Post;
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

            var request = ApiRequestFactory.GetApiRequest(AccessToken);
            request.Method = new HttpMethod("PATCH");
            request.Path = Endpoints.TextTrack;
            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("trackId", trackId.ToString());

            if (track == null) 
                return request;
            var parameters = new Dictionary<string, string>
            {
                ["active"] = track.active.ToString().ToLower()
            };
            if (track.name != null)
            {
                parameters["name"] = track.name;
            }
            if (track.language != null)
            {
                parameters["language"] =track.language;
            }
            if (track.type != null)
            {
                parameters["type"] = track.type;
            }
            request.Body = new FormUrlEncodedContent(parameters);

            return request;
        }

        private IApiRequest GenerateTextTracksRequest(long clipId, long? trackId = null)
        {
            ThrowIfUnauthorized();

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = trackId.HasValue ? Endpoints.TextTrack : Endpoints.TextTracks;
            request.Method = HttpMethod.Get;
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

            IApiRequest request = ApiRequestFactory.GetApiRequest(AccessToken);
            string endpoint = Endpoints.TextTrack;
            request.Method = HttpMethod.Delete;
            request.Path = endpoint;

            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("trackId", trackId.ToString());

            return request;
        }
    }
}