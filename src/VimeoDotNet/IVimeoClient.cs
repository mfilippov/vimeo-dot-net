using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    /// <summary>
    /// Interface of Vimeo API
    /// </summary>
    public interface IVimeoClient
    {
        #region User authentication

        /// <summary>
        /// Exchange the code for an access token asynchronously
        /// </summary>
        /// <param name="authorizationCode">A string token you must exchange for your access token</param>
        /// <param name="redirectUrl">This field is required, and must match one of your application’s
        /// redirect URI’s</param>
        /// <returns></returns>
        Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUrl);

        /// <summary>
        /// Return authorztion URL
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <param name="scope">Defaults to "public" and "private"; this is a space-separated list of <a href="#supported-scopes">scopes</a> you want to access</param>
        /// <param name="state">A unique value which the client will return alongside access tokens</param>
        /// <returns>Authorization URL</returns>
        string GetOauthUrl(string redirectUri, IEnumerable<string> scope, string state);

        // User Information
        /// <summary>
        /// Get user information async
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>User information object</returns>
        Task<User> GetUserInformationAsync(long userId);

        #endregion

        #region Videos

        // ...by id

        /// <summary>
        /// Get video by ClipId asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields">JSON filter, as per https://developer.vimeo.com/api/common-formats#json-filter</param>
        /// <returns>Video</returns>
        Task<Video> GetVideoAsync(long clipId, string[] fields = null);

        /// <summary>
        /// Get videos by UserId and query and page parameters asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="perPage">Number of items to show on each page. Max 50</param>
        /// <param name="query">Search query</param>
        /// <param name="fields">JSON filter, as per https://developer.vimeo.com/api/common-formats#json-filter</param>
        /// <param name="page">The page number to show</param>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Video>> GetVideosAsync(UserId userId, int? page = null, int? perPage = null,
            string query = null, string[] fields = null);

        // ... for an album

        /// <summary>
        /// Get videos by AlbumId asynchronously
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="page">The page number to show.</param>
        /// <param name="perPage">Number of items to show on each page. Max 50.</param>
        /// <param name="sort">The default sort order of an Album's videos</param>
        /// <param name="direction">The direction that the results are sorted.</param>
        /// <param name="fields">JSON filter, as per https://developer.vimeo.com/api/common-formats#json-filter </param>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Video>> GetAlbumVideosAsync(UserId userId, long albumId, int? page = null, int? perPage = null,
            string sort = null, string direction = null, string[] fields = null);

        /// <summary>
        /// Allows a video to be embedded on specified domain asynchronously.
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="domain">Domain</param>
        /// <remarks>
        /// The call is valid only when video embed privacy is set to
        /// <see cref="VideoEmbedPrivacyEnum.Whitelist"/>.
        /// Use <see cref="UpdateVideoMetadataAsync(long, VideoUpdateMetadata)"/> and
        /// <see cref="VideoUpdateMetadata.EmbedPrivacy"/> property to change this setting.
        /// </remarks>
        /// <seealso cref="DisallowEmbedVideoOnDomainAsync(long, string)"/>
        /// <seealso cref="GetAllowedDomainsForEmbeddingVideoAsync(long)"/>
        Task AllowEmbedVideoOnDomainAsync(long clipId, string domain);

        /// <summary>
        /// Disallows a video to be embedded on specified domain asynchronously.
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="domain">Domain</param>
        /// <remarks>
        /// The call is valid only when video embed privacy is set to
        /// <see cref="VideoEmbedPrivacyEnum.Whitelist"/>.
        /// Use <see cref="UpdateVideoMetadataAsync(long, VideoUpdateMetadata)"/> and
        /// <see cref="VideoUpdateMetadata.EmbedPrivacy"/> property to change this setting.
        /// </remarks>
        /// <seealso cref="AllowEmbedVideoOnDomainAsync(long, string)"/>
        /// <seealso cref="GetAllowedDomainsForEmbeddingVideoAsync(long)"/>
        Task DisallowEmbedVideoOnDomainAsync(long clipId, string domain);

        /// <summary>
        /// Get all domains on which a video can be embedded.
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <returns></returns>
        /// <remarks>
        /// The call is valid only when video embed privacy is set to
        /// <see cref="VideoEmbedPrivacyEnum.Whitelist"/>.
        /// Use <see cref="UpdateVideoMetadataAsync(long, VideoUpdateMetadata)"/> and
        /// <see cref="VideoUpdateMetadata.EmbedPrivacy"/> property to change this setting.
        /// </remarks>
        /// <seealso cref="AllowEmbedVideoOnDomainAsync(long, string)"/>
        /// <seealso cref="DisallowEmbedVideoOnDomainAsync(long, string)"/>
        Task<Paginated<DomainForEmbedding>> GetAllowedDomainsForEmbeddingVideoAsync(long clipId);

        /// <summary>
        /// Update allowed domain for clip asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="domain">Domain</param>
        [Obsolete("Use AllowEmbedVideoOnDomainAsync instead.")]
        Task UpdateVideoAllowedDomainAsync(long clipId, string domain);

        /// <summary>
        /// Get all thumbnails on a video
        /// </summary>
        /// <param name="clipId"></param>
        /// <returns></returns>
        Task<Paginated<Picture>> GetPicturesAsync(long clipId);

        /// <summary>
        /// Get a video thumbnail
        /// </summary>
        /// <param name="clipId">clipdId</param>
        /// <param name="pictureId">pictureId</param>
        /// <returns></returns>
        Task<Picture> GetPictureAsync(long clipId, long pictureId);

        /// <summary>
        /// Update video metadata by ClipId asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="metaData">New video metadata</param>
        Task UpdateVideoMetadataAsync(long clipId, VideoUpdateMetadata metaData);

        /// <summary>
        /// Assign an embed preset to a video asynchronously
        /// </summary>
        /// <param name="clipId">Clip ID</param>
        /// <param name="presetId">Preset ID</param>
        Task AssignEmbedPresetToVideoAsync(long clipId, long presetId);

        /// <summary>
        /// Unassign an embed preset from a video asynchronously
        /// </summary>
        /// <param name="clipId">Clip ID</param>
        /// <param name="presetId">Preset ID</param>
        Task UnassignEmbedPresetFromVideoAsync(long clipId, long presetId);

        /// <summary>
        /// Delete video asynchronously
        /// </summary>
        /// <param name="clipId">CliepId</param>
        Task DeleteVideoAsync(long clipId);

        /// <summary>
        /// Set a Video Thumbnail by a time code
        /// </summary>
        /// <param name="timeOffset">Time offset for the thumbnail in seconds</param>
        /// <param name="clipId">Clip Id</param>
        Task SetThumbnailAsync(long timeOffset, long clipId);

        /// <summary>
        /// Moves a video to a folder
        /// </summary>
        /// <param name="projectId">Folder Id (called project in Vimeo)</param>
        /// <param name="clipId">Clip Id</param>
        Task MoveVideoToFolder(long projectId, long clipId);

        #endregion

        #region Text tracks

        /// <summary>
        /// Get text tracks asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <returns>Return text tracks</returns>
        ///
        Task<TextTracks> GetTextTracksAsync(long videoId);

        /// <summary>
        /// Get text track asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <param name="trackId">TrackId</param>
        /// <returns>Return text track</returns>
        Task<TextTrack> GetTextTrackAsync(long videoId, long trackId);

        /// <summary>
        /// Update text track asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <param name="trackId">TrackId</param>
        /// <param name="track">TextTrack</param>
        /// <returns>Updated text track</returns>
        Task<TextTrack> UpdateTextTrackAsync(long videoId, long trackId, TextTrack track);

        /// <summary>
        /// Upload new text track file asynchronously
        /// </summary>
        /// <param name="fileContent">File content</param>
        /// <param name="videoId">VideoId</param>
        /// <param name="track">Track</param>
        /// <returns>New text track</returns>
        Task<TextTrack> UploadTextTrackFileAsync(IBinaryContent fileContent, long videoId, TextTrack track);

        /// <summary>
        /// Delete text track asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <param name="trackId">TrackId</param>
        /// <returns></returns>
        Task DeleteTextTrackAsync(long videoId, long trackId);

        #endregion

        #region Uploading files

        /// <summary>
        /// Create new upload ticket asynchronously
        /// </summary>
        /// <returns>Upload ticket</returns>
        Task<UploadTicket> GetUploadTicketAsync();

        /// <summary>
        /// Create new resumable upload ticket asynchronously
        /// </summary>
        /// <returns>Upload ticket</returns>
        Task<TusResumableUploadTicket> GetTusResumableUploadTicketAsync(long size, string name = null);

        /// <summary>
        /// Create new resumable replace upload ticket asynchronously
        /// </summary>
        /// <returns>Upload ticket</returns>
        Task<TusResumableUploadTicket> GetTusReplaceResumableUploadTicketAsync(long size, long clipId, string name = null);


        /// <summary>
        /// Create new upload ticket for replace video asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <returns>Upload ticket</returns>
        Task<UploadTicket> GetReplaceVideoUploadTicketAsync(long videoId);

        /// <summary>
        /// Upload file part asynchronously
        /// </summary>
        /// <param name="fileContent">FileContent</param>
        /// <param name="chunkSize">ChunkSize</param>
        /// <param name="replaceVideoId">ReplaceVideoId</param>
        /// <returns>Upload request</returns>
        Task<IUploadRequest> UploadEntireFileAsync(IBinaryContent fileContent,
            int chunkSize = VimeoClient.DefaultUploadChunkSize,
            long? replaceVideoId = null,
            Action<double> statusCallback = null);

        /// <summary>
        /// Create new upload ticket asynchronously
        /// </summary>
        /// <returns>Upload ticket</returns>
        Task<Video> UploadPullLinkAsync(string link);

        /// <summary>
        /// Upload and set thumbnail active
        /// </summary>
        /// <param name="clipId"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        /// <exception cref="VimeoUploadException"></exception>
        Task<Picture> UploadThumbnailAsync(long clipId, IBinaryContent fileContent);

        #endregion

        #region Account information

        /// <summary>
        /// Get user information asynchronously
        /// </summary>
        /// <returns>User information</returns>
        Task<User> GetAccountInformationAsync();

        /// <summary>
        /// Update user information asynchronously
        /// </summary>
        /// <param name="parameters">User parameters</param>
        /// <returns>User information</returns>
        Task<User> UpdateAccountInformationAsync(EditUserParameters parameters);

        #endregion

        #region Albums

        /// <summary>
        /// Get album by AlbumId and UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Album</returns>
        Task<Album> GetAlbumAsync(UserId userId, long albumId);

        /// <summary>
        /// Get album by UserId and parameters asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="parameters">GetAlbumsParameters</param>
        /// <returns>Paginated albums</returns>
        Task<Paginated<Album>> GetAlbumsAsync(UserId userId, GetAlbumsParameters parameters = null);

        /// <summary>
        /// Create new album asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="parameters">Creation parameters</param>
        /// <returns>Album</returns>
        Task<Album> CreateAlbumAsync(UserId userId, EditAlbumParameters parameters = null);

        /// <summary>
        /// Update album asynchronously
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="albumId">Album Id</param>
        /// <param name="parameters">Album parameters</param>
        /// <returns>Album</returns>
        Task<Album> UpdateAlbumAsync(UserId userId, long albumId, EditAlbumParameters parameters = null);

        /// <summary>
        /// Delete album asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Deletion result</returns>
        Task<bool> DeleteAlbumAsync(UserId userId, long albumId);

        /// <summary>
        /// Add video to album by UserId and AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Adding result</returns>
        Task<bool> AddToAlbumAsync(UserId userId, long albumId, long clipId);

        /// <summary>
        /// Remove video from album by AlbumId and ClipId and UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Removing result</returns>
        Task<bool> RemoveFromAlbumAsync(UserId userId, long albumId, long clipId);

        #endregion

        #region Rate Limit

        /// <summary>
        /// Return rate limit
        /// </summary>
        long RateLimit { get; }

        /// <summary>
        /// Return remaning rate limit
        /// </summary>
        long RateLimitRemaining { get; }

        /// <summary>
        /// Return rate limit reset time
        /// </summary>
        DateTime RateLimitReset { get; }

        #endregion

        #region Tags

        /// <summary>
        /// Add a tag to a video
        /// </summary>
        /// <param name="clipId">Clip Id</param>
        /// <param name="tag">Tag</param>
        /// <returns>Tag</returns>
        Task<Tag> AddVideoTagAsync(long clipId, string tag);

        /// <summary>
        /// Delete a tag from a video
        /// </summary>
        Task DeleteVideoTagAsync(long clipId, string tag);

        /// <summary>
        /// List a videos' tags
        /// </summary>
        /// <param name="clipId">Clip Id</param>
        /// <param name="page">Page number</param>
        /// <param name="perPage">Tags per page</param>
        /// <returns></returns>
        /// <returns>Paginated tags</returns>
        Task<Paginated<Tag>> GetVideoTags(long clipId, int? page = null, int? perPage = null);

        /// <summary>
        /// Get a tag
        /// </summary>
        /// <param name="tag">Tag word</param>
        /// <returns>Tag</returns>
        Task<Tag> GetVideoTagAsync(string tag);

        /// <summary>
        /// Get all videos tagged with a specific word
        /// </summary>
        /// <param name="tag">Tag id</param>
        /// <param name="page">Page number</param>
        /// <param name="perPage">Video per page</param>
        /// <param name="sort">Technique used to sort the results.</param>
        /// <param name="direction">The direction that the results are sorted.</param>
        /// <param name="fields">JSON filter, as per https://developer.vimeo.com/api/common-formats#json-filter</param>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Video>> GetVideoByTag(string tag, int? page = null,
            int? perPage = null, GetVideoByTagSort? sort = null, GetVideoByTagDirection? direction = null, string[] fields = null);

        #endregion

        #region EmbedPresets

        /// <summary>
        /// Get embed preset by user ID and preset ID asynchronously
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="presetId">Preset ID</param>
        /// <param name="fields">JSON filter, as per https://developer.vimeo.com/api/common-formats#json-filter</param>
        /// <returns>Embed preset</returns>
        Task<EmbedPresets> GetEmbedPresetAsync(UserId userId, long presetId, string[] fields = null);

        /// <summary>
        /// Get embed presets by user ID and page parameters asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="page">The page number to show</param>
        /// <param name="perPage">Number of items to show on each page. Max 50</param>
        /// <param name="fields">JSON filter, as per https://developer.vimeo.com/api/common-formats#json-filter</param>
        /// <returns>Paginated embed presets</returns>
        Task<Paginated<EmbedPresets>> GetEmbedPresetsAsync(UserId userId, int? page = null, int? perPage = null, string[] fields = null);

        #endregion

        #region Channels

        /// <summary>
        /// This method returns a single channel.
        /// </summary>
        /// <param name="channelId">The ID of the channel</param>
        /// <returns></returns>
        Task<Channel> GetChannelAsync(long channelId);

        /// <summary>
        /// This method returns all available channels.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Paginated<Channel>> GetChannelsAsync(GetChannelsParameters parameters = null);

        /// <summary>
        /// This method creates a new channel
        /// </summary>
        /// <param name="parameters">Channel creation parameters</param>
        /// <returns>Channel</returns>
        Task<Channel> CreateChannelAsync(EditChannelParameters parameters = null);

        /// <summary>
        /// This method deletes the specified channel
        /// </summary>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns>Deletion result</returns>
        Task<bool> DeleteChannelAsync(long channelId);

        /// <summary>
        /// Add video to channel.
        /// </summary>
        /// <param name="channelId">Channel Id</param>
        /// <param name="clipId">Clip Id</param>
        /// <returns></returns>
        Task<bool> AddToChannelAsync(long channelId, long clipId);

        /// <summary>
        /// This method returns all the channels to which the user subscribes
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Paginated<Channel>> GetUserChannelsAsync(GetChannelsParameters parameters = null);

        #endregion

        #region Folders

        /// <summary>
        /// Create a folder
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="name">Name</param>
        /// <returns>Tag</returns>
        Task<Folder> CreateVideoFolder(UserId userId, string name);

        /// <summary>
        /// Get all folders by UserId and query and page parameters asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="perPage">Number of items to show on each page. Max 50</param>
        /// <param name="query">Search query</param>
        /// <param name="fields">JSON filter, as per https://developer.vimeo.com/api/common-formats#json-filter</param>
        /// <param name="page">The page number to show</param>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Folder>> GetUserFolders(UserId userId, int? page = null, int? perPage = null, string query = null, string[] fields = null);



        #endregion
    }
}
