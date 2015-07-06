using System.Collections.Generic;
using System.Threading.Tasks;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public interface IVimeoClient
    {
        void CompleteFileUpload(IUploadRequest uploadRequest);
        Task CompleteFileUploadAsync(IUploadRequest uploadRequest);
        VerifyUploadResponse ContinueUploadFile(IUploadRequest uploadRequest);
        Task<VerifyUploadResponse> ContinueUploadFileAsync(IUploadRequest uploadRequest);
        AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl);
        Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUrl);
        User GetAccountInformation();
        Task<User> GetAccountInformationAsync();
        User GetUserInformation(long userId);
        Task<User> GetUserInformationAsync(long userId);
        Video GetVideo(long clipId, string fieldsCsv = null);
        Task<Video> GetVideoAsync(long clipId, string fieldsCsv = null);
        Paginated<Video> GetVideos(string fieldsCsv = null);
        Task<Paginated<Video>> GetVideosAsync(int? page, int? perPage, string fieldsCsv = null);
        string GetOauthUrl(string redirectUri, IEnumerable<string> scope, string state);
        UploadTicket GetUploadTicket();
        Task<UploadTicket> GetUploadTicketAsync();
        Video GetUserVideo(long userId, long clipId, string fieldsCsv = null);
        Task<Video> GetUserVideoAsync(long userId, long clipId, string fieldsCsv = null);
        Paginated<Video> GetUserVideos(long userId, string fieldsCsv = null);
        Task<Paginated<Video>> GetUserVideosAsync(long userId, string fieldsCsv = null);
        Paginated<Video> GetUserVideos(long userId, int? page, int? perPage, string fieldsCsv = null);
        Task<Paginated<Video>> GetUserVideosAsync(long userId, int? page, int? perPage, string fieldsCsv = null);
        IUploadRequest StartUploadFile(IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE);

        Task<IUploadRequest> StartUploadFileAsync(IBinaryContent fileContent,
            int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE);

        void UpdateVideoMetadata(long clipId, VideoUpdateMetadata metaData);
        Task UpdateVideoMetadataAsync(long clipId, VideoUpdateMetadata metaData);
        void DeleteVideo(long clipId);
        Task DeleteVideoAsync(long clipId);
        IUploadRequest UploadEntireFile(IBinaryContent fileContent,
            int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE);
        Task<IUploadRequest> UploadEntireFileAsync(IBinaryContent fileContent,
            int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE);
        VerifyUploadResponse VerifyUploadFile(IUploadRequest uploadRequest);
        Task<VerifyUploadResponse> VerifyUploadFileAsync(IUploadRequest uploadRequest);

        Paginated<Video> GetAlbumVideos(long albumId, string fieldsCsv = null);
        Task<Paginated<Video>> GetAlbumVideosAsync(long albumId, int? page, int? perPage, string fieldsCsv = null);
        Video GetAlbumVideo(long albumId, long clipId, string fieldsCsv = null);
        Task<Video> GetAlbumVideoAsync(long albumId, long clipId, string fieldsCsv = null);
        Paginated<Video> GetUserAlbumVideos(long userId, long albumId, int? page = null, int? perPage = null, string fieldsCsv = null);
        Task<Paginated<Video>> GetUserAlbumVideosAsync(long userId, long albumId, int? page = null, int? perPage = null, string fieldsCsv = null);
        Video GetUserAlbumVideo(long userId, long albumId, long clipId, string fieldsCsv = null);
        Task<Video> GetUserAlbumVideoAsync(long userId, long albumId, long clipId, string fieldsCsv = null);

        Paginated<Album> GetAlbums(long? userId, int? page = null, int? perPage = null, string fieldsCsv = null);
        void AddVideoToAlbum(long? userId, long albumId, long clipId);
        void RemoveVideoFromAlbum(long? userId, long albumId, long clipId);
    }
}