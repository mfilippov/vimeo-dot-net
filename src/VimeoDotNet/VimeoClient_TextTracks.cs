using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VimeoDotNet.Constants;
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
        public async Task<TextTracks> GetTextTracksAsync(long videoId)
        {
            try
            {
                var request = GenerateTextTracksRequest(videoId);
                var response = await request.ExecuteRequestAsync<TextTracks>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving text tracks for video.", HttpStatusCode.NotFound);

                return response.StatusCode == HttpStatusCode.NotFound ? null : response.Content;
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

        /// <inheritdoc />
        public async Task<TextTrack> GetTextTrackAsync(long videoId, long trackId)
        {
            try
            {
                var request = GenerateTextTracksRequest(videoId, trackId);
                var response = await request.ExecuteRequestAsync<TextTrack>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error retrieving text track for video.", HttpStatusCode.NotFound);

                return response.StatusCode == HttpStatusCode.NotFound ? null : response.Content;
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

        /// <inheritdoc />
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

            var ticket = await GetUploadTextTrackTicketAsync(videoId, track).ConfigureAwait(false);
            var request = _apiRequestFactory.GetApiRequest();
            request.Method = HttpMethod.Put;
            request.ExcludeAuthorizationHeader = true;
            request.Path = ticket.Link;

            request.Body = new ByteArrayContent(await fileContent.ReadAllAsync().ConfigureAwait(false));

            var response = await request.ExecuteRequestAsync().ConfigureAwait(false);
            CheckStatusCodeError(null, response, "Error uploading text track file.", HttpStatusCode.BadRequest);

            return ticket;
        }

        /// <inheritdoc />
        public async Task<TextTrack> UpdateTextTrackAsync(long videoId, long trackId, TextTrack track)
        {
            try
            {
                var request = GenerateUpdateTextTrackRequest(videoId, trackId, track);
                var response = await request.ExecuteRequestAsync<TextTrack>().ConfigureAwait(false);
                UpdateRateLimit(response);
                CheckStatusCodeError(response, "Error updating text track for video.", HttpStatusCode.NotFound);

                return response.StatusCode == HttpStatusCode.NotFound ? null : response.Content;
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

        /// <inheritdoc />
        public async Task DeleteTextTrackAsync(long videoId, long trackId)
        {
            try
            {
                var request = GenerateDeleteTextTrackRequest(videoId, trackId);
                var response = await request.ExecuteRequestAsync<TextTrack>().ConfigureAwait(false);
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

        /// <summary>
        /// Get upload text track ticket as an asynchronous operation.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="track">The track.</param>
        /// <returns>A Task&lt;TextTrack&gt; representing the asynchronous operation.</returns>
        /// <exception cref="VimeoDotNet.Exceptions.VimeoUploadException">Error generating upload text track ticket. - null</exception>
        private async Task<TextTrack> GetUploadTextTrackTicketAsync(long clipId, TextTrack track)
        {
            try
            {
                var request = GenerateUploadTextTrackTicketRequest(clipId, track);
                var response = await request.ExecuteRequestAsync<TextTrack>().ConfigureAwait(false);
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

        /// <summary>
        /// Generates the upload text track ticket request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="track">The track.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateUploadTextTrackTicketRequest(long clipId, TextTrack track)
        {
            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = HttpMethod.Post;
            request.Path = Endpoints.TextTracks;
            request.UrlSegments.Add("clipId", clipId.ToString());
            if (track == null)
                return request;
            var parameters = new Dictionary<string, string>
            {
                ["active"] = track.Active.ToString().ToLower(),
                ["name"] = track.Name,
                ["language"] = track.Language,
                ["type"] = track.Type.ToString().ToLowerInvariant()
            };
            request.Body = new FormUrlEncodedContent(parameters);
            return request;
        }

        /// <summary>
        /// Generates the update text track request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="trackId">The track identifier.</param>
        /// <param name="track">The track.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateUpdateTextTrackRequest(long clipId, long trackId, [NotNull] TextTrack track)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            request.Method = new HttpMethod("PATCH");
            request.Path = Endpoints.TextTrack;
            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("trackId", trackId.ToString());

            var parameters = new Dictionary<string, string>
            {
                ["active"] = track.Active.ToString().ToLower()
            };
            if (track.Name != null)
            {
                parameters["name"] = track.Name;
            }

            if (track.Language != null)
            {
                parameters["language"] = track.Language;
            }

            parameters["type"] = track.Type.ToString().ToLowerInvariant();
            request.Body = new FormUrlEncodedContent(parameters);

            return request;
        }

        /// <summary>
        /// Generates the text tracks request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="trackId">The track identifier.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateTextTracksRequest(long clipId, long? trackId = null)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            var endpoint = trackId.HasValue ? Endpoints.TextTrack : Endpoints.TextTracks;
            request.Method = HttpMethod.Get;
            request.Path = endpoint;

            request.UrlSegments.Add("clipId", clipId.ToString());
            if (trackId.HasValue)
            {
                request.UrlSegments.Add("trackId", trackId.ToString());
            }

            return request;
        }

        /// <summary>
        /// Generates the delete text track request.
        /// </summary>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="trackId">The track identifier.</param>
        /// <returns>IApiRequest.</returns>
        private IApiRequest GenerateDeleteTextTrackRequest(long clipId, long trackId)
        {
            ThrowIfUnauthorized();

            var request = _apiRequestFactory.GetApiRequest(AccessToken);
            const string endpoint = Endpoints.TextTrack;
            request.Method = HttpMethod.Delete;
            request.Path = endpoint;

            request.UrlSegments.Add("clipId", clipId.ToString());
            request.UrlSegments.Add("trackId", trackId.ToString());

            return request;
        }
    }
}