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
        Video GetVideo(long clipId);
        Task<Video> GetVideoAsync(long clipId);
        Paginated<Video> GetAccountVideos();
        Task<Paginated<Video>> GetAccountVideosAsync();
        string GetOauthUrl(string redirectUri, IEnumerable<string> scope, string state);
        UploadTicket GetUploadTicket();
        Task<UploadTicket> GetUploadTicketAsync();
        Video GetUserVideo(long userId, long clipId);
        Task<Video> GetUserVideoAsync(long userId, long clipId);
        Paginated<Video> GetUserVideos(long userId);
        Task<Paginated<Video>> GetUserVideosAsync(long userId);
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
    }
}