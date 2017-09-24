using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// Exchange the code for an access token
        /// </summary>
        /// <param name="authorizationCode">A string token you must exchange for your access token</param>
        /// <param name="redirectUrl">This field is required, and must match one of your application’s
        /// redirect URI’s</param>
        /// <returns>AccessTokenResponse</returns>
        AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl);

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
        /// Get user information
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>User information object</returns>
        User GetUserInformation(long userId);

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
        /// Get video by ClipId
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields"></param>
        /// <returns>Video</returns>
        Video GetVideo(long clipId, string[] fields = null);

        /// <summary>
        /// Get video by ClipId asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields"></param>
        /// <returns>Video</returns>
        Task<Video> GetVideoAsync(long clipId, string[] fields = null);


        // ...for current account

        /// <summary>
        /// Get paginated video for current account
        /// </summary>
        /// <returns>Paginated videos</returns>
        Paginated<Video> GetVideos(string[] fields = null);

        /// <summary>
        /// Get paginated video for current account asynchronously
        /// </summary>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Video>> GetVideosAsync(int? page, int? perPage, string[] fields = null);


        // ...for another account

        /// <summary>
        /// Get video by ClipId for UserId
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields"></param>
        /// <returns>Video</returns>
        Video GetUserVideo(long userId, long clipId, string[] fields = null);

        /// <summary>
        /// Get video by ClipId for UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields"></param>
        /// <returns>Video</returns>
        Task<Video> GetUserVideoAsync(long userId, long clipId, string[] fields = null);

        /// <summary>
        /// Get videos  by UserId and query
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="query">Search query</param>
        /// <param name="fields"></param>
        /// <returns>Paginated videos</returns>
        Paginated<Video> GetUserVideos(long userId, string query = null, string[] fields = null);

        /// <summary>
        /// Get videos by UserId and query asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="query">Search query</param>
        /// <param name="fields"></param>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Video>> GetUserVideosAsync(long userId, string query = null, string[] fields = null);

        /// <summary>
        /// Get videos by UserId and query and page parameters
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="perPage">Number of items to show on each page. Max 50</param>
        /// <param name="query">Search query</param>
        /// <param name="fields"></param>
        /// <param name="page">The page number to show</param>
        /// <returns>Paginated videos</returns>
        Paginated<Video> GetUserVideos(long userId, int? page, int? perPage, string query = null, string[] fields = null);

        /// <summary>
        /// Get videos by UserId and query and page parameters asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="perPage">Number of items to show on each page. Max 50</param>
        /// <param name="query">Search query</param>
        /// <param name="fields"></param>
        /// <param name="page">The page number to show</param>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Video>> GetUserVideosAsync(long userId, int? page, int? perPage, string query = null, string[] fields = null);


        // ...for an album

        /// <summary>
        /// Get videos by AlbumId
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="page">The page number to show</param>
        /// <param name="perPage">Number of items to show on each page. Max 50</param>
        /// <param name="sort">The default sort order of an Album's videos</param>
        /// <param name="direction">The direction that the results are sorted</param>
        /// <param name="fields"></param>
        /// <returns>Paginated videos</returns>
        Paginated<Video> GetAlbumVideos(long albumId, int? page, int? perPage,
            string sort = null, string direction = null, string[] fields = null);

        /// <summary>
        /// Get videos by AlbumId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="page">The page number to show.</param>
        /// <param name="perPage">Number of items to show on each page. Max 50.</param>
        /// <param name="sort">The default sort order of an Album's videos</param>
        /// <param name="direction">The direction that the results are sorted.</param>
        /// <param name="fields">JSON filter, as per https://developer.vimeo.com/api/common-formats#json-filter </param>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Video>> GetAlbumVideosAsync(long albumId, int? page, int? perPage,
            string sort = null, string direction = null, string[] fields = null);

        /// <summary>
        /// Get video from album by AlbumId and ClipId
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields"></param>
        /// <returns>Video</returns>
        Video GetAlbumVideo(long albumId, long clipId, string[] fields = null);

        /// <summary>
        /// Get video from album by AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields"></param>
        /// <returns>Video</returns>
        Task<Video> GetAlbumVideoAsync(long albumId, long clipId, string[] fields = null);

        /// <summary>
        /// Get videos from album by AlbumId and UserId
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="fields"></param>
        /// <returns>Paginated videos</returns>
        Paginated<Video> GetUserAlbumVideos(long userId, long albumId, string[] fields = null);

        /// <summary>
        /// Get videos from album by AlbumId and UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="fields"></param>
        /// <returns>Paginated videos</returns>
        Task<Paginated<Video>> GetUserAlbumVideosAsync(long userId, long albumId, string[] fields = null);

        /// <summary>
        /// Get video from album by AlbumId and UserId and ClipId
        /// </summary>
        /// <param name="userId">AlbumId</param>
        /// <param name="albumId">UserId</param>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields"></param>
        /// <returns>Video</returns>
        Video GetUserAlbumVideo(long userId, long albumId, long clipId, string[] fields = null);

        /// <summary>
        /// Get video from album by AlbumId and UserId and ClipId asynchronously
        /// </summary>
        /// <param name="userId">AlbumId</param>
        /// <param name="albumId">UserId</param>
        /// <param name="clipId">ClipId</param>
        /// <param name="fields"></param>
        /// <returns>Video</returns>
        Task<Video> GetUserAlbumVideoAsync(long userId, long albumId, long clipId, string[] fields = null);

        /// <summary>
        /// Update allowed domain for clip
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="domain">Domain</param>
        void UpdateVideoAllowedDomain(long clipId, string domain);

        /// <summary>
        /// Update allowed domain for clip asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="domain">Domain</param>
        Task UpdateVideoAllowedDomainAsync(long clipId, string domain);
        #endregion

        #region Update video metadata
        /// <summary>
        /// Update video metadata by ClipId
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="metaData">New video metadata</param>
        void UpdateVideoMetadata(long clipId, VideoUpdateMetadata metaData);

        /// <summary>
        /// Update video metadata by ClipId asynchronously
        /// </summary>
        /// <param name="clipId">ClipId</param>
        /// <param name="metaData">New video metadata</param>
        Task UpdateVideoMetadataAsync(long clipId, VideoUpdateMetadata metaData);
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
        /// Create new upload ticket
        /// </summary>
        /// <returns>Upload ticket</returns>
        UploadTicket GetUploadTicket();

        /// <summary>
        /// Create new upload ticket asynchronously
        /// </summary>
        /// <returns>Upload ticket</returns>
        Task<UploadTicket> GetUploadTicketAsync();

        /// <summary>
        /// Create new upload ticket for replace video
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <returns>Upload ticket</returns>
        UploadTicket GetReplaceVideoUploadTicket(long videoId);

        /// <summary>
        /// Create new upload ticket for replace video asynchronously
        /// </summary>
        /// <param name="videoId">VideoId</param>
        /// <returns>Upload ticket</returns>
        Task<UploadTicket> GetReplaceVideoUploadTicketAsync(long videoId);

        /// <summary>
        /// Upload file part
        /// </summary>
        /// <param name="fileContent">FileContent</param>
        /// <param name="chunkSize">ChunkSize</param>
        /// <param name="replaceVideoId">ReplaceVideoId</param>
        /// <returns>Upload request</returns>
        IUploadRequest UploadEntireFile(IBinaryContent fileContent,
            int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE, long? replaceVideoId = null);

        /// <summary>
        /// Upload file part asynchronously
        /// </summary>
        /// <param name="fileContent">FileContent</param>
        /// <param name="chunkSize">ChunkSize</param>
        /// <param name="replaceVideoId">ReplaceVideoId</param>
        /// <returns>Upload request</returns>
        Task<IUploadRequest> UploadEntireFileAsync(IBinaryContent fileContent,
            int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE, long? replaceVideoId = null);

        /// <summary>
        /// Verify upload file part
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns>Verification reponse</returns>
        VerifyUploadResponse VerifyUploadFile(IUploadRequest uploadRequest);

        /// <summary>
        /// Verify upload file part asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns>Verification reponse</returns>
        Task<VerifyUploadResponse> VerifyUploadFileAsync(IUploadRequest uploadRequest);

        /// <summary>
        /// Start upload file
        /// </summary>
        /// <param name="fileContent">FileContent</param>
        /// <param name="chunkSize">ChunkSize</param>
        /// <param name="replaceVideoId">ReplaceVideoId</param>
        /// <returns>Upload request</returns>
        IUploadRequest StartUploadFile(IBinaryContent fileContent,
            int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE, long? replaceVideoId = null);

        /// <summary>
        /// Start upload file asynchronously
        /// </summary>
        /// <param name="fileContent">FileContent</param>
        /// <param name="chunkSize">ChunkSize</param>
        /// <param name="replaceVideoId">ReplaceVideoId</param>
        /// <returns></returns>
        Task<IUploadRequest> StartUploadFileAsync(IBinaryContent fileContent,
            int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE, long? replaceVideoId = null);

        /// <summary>
        /// Continue upload file
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns>Verification upload response</returns>
        VerifyUploadResponse ContinueUploadFile(IUploadRequest uploadRequest);

        /// <summary>
        /// Continue upload file asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns>Verification upload response</returns>
        Task<VerifyUploadResponse> ContinueUploadFileAsync(IUploadRequest uploadRequest);

        /// <summary>
        /// Complete upload file
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns></returns>
        void CompleteFileUpload(IUploadRequest uploadRequest);

        /// <summary>
        /// Complete upload file asynchronously
        /// </summary>
        /// <param name="uploadRequest">UploadRequest</param>
        /// <returns></returns>
        Task CompleteFileUploadAsync(IUploadRequest uploadRequest);
        #endregion

        #region Account information
        /// <summary>
        /// Get user information
        /// </summary>
        /// <returns>User information</returns>
        User GetAccountInformation();

        /// <summary>
        /// Get user information asynchronously
        /// </summary>
        /// <returns>User information</returns>
        Task<User> GetAccountInformationAsync();

        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="parameters">User parameters</param>
        /// <returns>User information</returns>
        User UpdateAccountInformation(EditUserParameters parameters);

        /// <summary>
        /// Update user information asynchronously
        /// </summary>
        /// <param name="parameters">User parameters</param>
        /// <returns>User information</returns>
        Task<User> UpdateAccountInformationAsync(EditUserParameters parameters);
        #endregion

        #region Albums
        /// <summary>
        /// Get album by parameters
        /// </summary>
        /// <param name="parameters">GetAlbumsParameters</param>
        /// <returns>Paginated albums</returns>
        Paginated<Album> GetAlbums(GetAlbumsParameters parameters = null);

        /// <summary>
        /// Get album by parameters asynchronously
        /// </summary>
        /// <param name="parameters">GetAlbumsParameters</param>
        /// <param name="fields"></param>
        /// <returns>Paginated albums</returns>
        Task<Paginated<Album>> GetAlbumsAsync(GetAlbumsParameters parameters = null, string[] fields = null);

        /// <summary>
        /// Get album by UserId and parameters
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="parameters">GetAlbumsParameters</param>
        /// <returns>Paginated albums</returns>
        Paginated<Album> GetAlbums(long userId, GetAlbumsParameters parameters = null);

        /// <summary>
        /// Get album by UserId and parameters asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="parameters">GetAlbumsParameters</param>
        /// <returns>Paginated albums</returns>
        Task<Paginated<Album>> GetAlbumsAsync(long userId, GetAlbumsParameters parameters = null);

        /// <summary>
        /// Get album by AlbumId
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Album</returns>
        Album GetAlbum(long albumId);

        /// <summary>
        /// Get album by AlbumId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Album</returns>
        Task<Album> GetAlbumAsync(long albumId);

        /// <summary>
        ///Get album by AlbumId and UserId
        /// </summary>
        /// <param name="userId">AlbumId</param>
        /// <param name="albumId">UserId</param>
        /// <returns>Album</returns>
        Album GetAlbum(long userId, long albumId);

        /// <summary>
        /// Get album by AlbumId and UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Album</returns>
        Task<Album> GetAlbumAsync(long userId, long albumId);

        /// <summary>
        /// Create new album
        /// </summary>
        /// <param name="parameters">Creation parameters</param>
        /// <returns>Album</returns>
        Album CreateAlbum(EditAlbumParameters parameters = null);

        /// <summary>
        /// Create new album asynchronously
        /// </summary>
        /// <param name="parameters">Creation parameters</param>
        /// <returns>Album</returns>
        Task<Album> CreateAlbumAsync(EditAlbumParameters parameters = null);

        /// <summary>
        /// Update album
        /// </summary>
        /// <param name="albumId">Albumid</param>
        /// <param name="parameters">Album parameters</param>
        /// <returns>Album</returns>
        Album UpdateAlbum(long albumId, EditAlbumParameters parameters = null);

        /// <summary>
        /// Update album asynchronously
        /// </summary>
        /// <param name="albumId">Albumid</param>
        /// <param name="parameters">Album parameters</param>
        /// <returns>Album</returns>
        Task<Album> UpdateAlbumAsync(long albumId, EditAlbumParameters parameters = null);

        /// <summary>
        /// Delete album
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Deletion result</returns>
        bool DeleteAlbum(long albumId);

        /// <summary>
        /// Delete album asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <returns>Deletion result</returns>
        Task<bool> DeleteAlbumAsync(long albumId);

        /// <summary>
        /// Add video to album by AlbumId and ClipId
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Adding result</returns>
        bool AddToAlbum(long albumId, long clipId);

        /// <summary>
        /// Add video to album by AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Adding result</returns>
        Task<bool> AddToAlbumAsync(long albumId, long clipId);

        /// <summary>
        /// Add video to album by UserId and AlbumId and ClipId
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Adding result</returns>
        bool AddToAlbum(long userId, long albumId, long clipId);

        /// <summary>
        /// Add video to album by UserId and AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Adding result</returns>
        Task<bool> AddToAlbumAsync(long userId, long albumId, long clipId);

        /// <summary>
        /// Remove video from album by AlbumId and ClipId
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Removing result</returns>
        bool RemoveFromAlbum(long albumId, long clipId);

        /// <summary>
        /// Remove video from album by AlbumId and ClipId asynchronously
        /// </summary>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Removing result</returns>
        Task<bool> RemoveFromAlbumAsync(long albumId, long clipId);

        /// <summary>
        /// Remove video from album by AlbumId and ClipId and UserId
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Removing result</returns>
        bool RemoveFromAlbum(long userId, long albumId, long clipId);

        /// <summary>
        /// Remove video from album by AlbumId and ClipId and UserId asynchronously
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="albumId">AlbumId</param>
        /// <param name="clipId">ClipId</param>
        /// <returns>Removing result</returns>
        Task<bool> RemoveFromAlbumAsync(long userId, long albumId, long clipId);
        #endregion

        #region Deleting videos
        /// <summary>
        /// Delete video
        /// </summary>
        /// <param name="clipId">CliepId</param>
        void DeleteVideo(long clipId);

        /// <summary>
        /// Delete video asynchronously
        /// </summary>
        /// <param name="clipId">CliepId</param>
        Task DeleteVideoAsync(long clipId);
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
    }
}