using System;

namespace VimeoDotNet
{
    public interface IVimeoClient
    {
        void CompleteFileUpload(VimeoDotNet.Net.IUploadRequest uploadRequest);
        System.Threading.Tasks.Task CompleteFileUploadAsync(VimeoDotNet.Net.IUploadRequest uploadRequest);
        VimeoDotNet.Models.VerifyUploadResponse ContinueUploadFile(VimeoDotNet.Net.IUploadRequest uploadRequest);
        System.Threading.Tasks.Task<VimeoDotNet.Models.VerifyUploadResponse> ContinueUploadFileAsync(VimeoDotNet.Net.IUploadRequest uploadRequest);
        VimeoDotNet.Models.AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl);
        System.Threading.Tasks.Task<VimeoDotNet.Models.AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUrl);
        VimeoDotNet.Models.User GetAccountInformation();
        System.Threading.Tasks.Task<VimeoDotNet.Models.User> GetAccountInformationAsync();
        VimeoDotNet.Models.Video GetAccountVideo(long clipId);
        System.Threading.Tasks.Task<VimeoDotNet.Models.Video> GetAccountVideoAsync(long clipId);
        VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video> GetAccountVideos();
        System.Threading.Tasks.Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video>> GetAccountVideosAsync();
        string GetOauthUrl(string redirectUri, System.Collections.Generic.IEnumerable<string> scope, string state);
        VimeoDotNet.Models.UploadTicket GetUploadTicket();
        System.Threading.Tasks.Task<VimeoDotNet.Models.UploadTicket> GetUploadTicketAsync();
        VimeoDotNet.Models.Video GetUserVideo(long userId, long clipId);
        System.Threading.Tasks.Task<VimeoDotNet.Models.Video> GetUserVideoAsync(long userId, long clipId);
        VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video> GetUserVideos(long userId);
        System.Threading.Tasks.Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video>> GetUserVideosAsync(long userId);
        VimeoDotNet.Net.IUploadRequest StartUploadFile(VimeoDotNet.Net.IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE);
        System.Threading.Tasks.Task<VimeoDotNet.Net.IUploadRequest> StartUploadFileAsync(VimeoDotNet.Net.IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE);
        VimeoDotNet.Net.IUploadRequest UploadEntireFile(VimeoDotNet.Net.IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE);
        System.Threading.Tasks.Task<VimeoDotNet.Net.IUploadRequest> UploadEntireFileAsync(VimeoDotNet.Net.IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE);
        VimeoDotNet.Models.VerifyUploadResponse VerifyUploadFile(VimeoDotNet.Net.IUploadRequest uploadRequest);
        System.Threading.Tasks.Task<VimeoDotNet.Models.VerifyUploadResponse> VerifyUploadFileAsync(VimeoDotNet.Net.IUploadRequest uploadRequest);
    }
}
