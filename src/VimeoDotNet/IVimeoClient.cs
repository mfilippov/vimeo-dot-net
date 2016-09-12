using System.Collections.Generic;
using System.Threading.Tasks;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    public interface IVimeoClient
    {
		// User Authentication
        AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl);
        Task<AccessTokenResponse> GetAccessTokenAsync(string authorizationCode, string redirectUrl);
		string GetOauthUrl(string redirectUri, IEnumerable<string> scope, string state);

		// User Information
        User GetUserInformation(long userId);
        Task<User> GetUserInformationAsync(long userId);

		// Retrieve Videos
		// ...by id
        Video GetVideo(long clipId);
        Task<Video> GetVideoAsync(long clipId);
		// ...for current account
        Paginated<Video> GetVideos();
        Task<Paginated<Video>> GetVideosAsync(int? page, int? perPage);
		// ...for another acount
        Video GetUserVideo(long userId, long clipId);
        Task<Video> GetUserVideoAsync(long userId, long clipId);
        Paginated<Video> GetUserVideos(long userId, string query = null);
        Task<Paginated<Video>> GetUserVideosAsync(long userId, string query = null);
		// ...for an album
        Paginated<Video> GetAlbumVideos(long albumId, int? page, int? perPage, string sort = null, string direction = null);
        Task<Paginated<Video>> GetAlbumVideosAsync(long albumId, int? page, int? perPage, string sort = null, string direction = null);
        Video GetAlbumVideo(long albumId, long clipId);
        Task<Video> GetAlbumVideoAsync(long albumId, long clipId);
        Paginated<Video> GetUserAlbumVideos(long userId, long albumId);
        Task<Paginated<Video>> GetUserAlbumVideosAsync(long userId, long albumId);
        Video GetUserAlbumVideo(long userId, long albumId, long clipId);
        Task<Video> GetUserAlbumVideoAsync(long userId, long albumId, long clipId);

		// Update Video Metadata
		void UpdateVideoMetadata(long clipId, VideoUpdateMetadata metaData);
		Task UpdateVideoMetadataAsync(long clipId, VideoUpdateMetadata metaData);

		// Text Tracks
		Task<TextTracks> GetTextTracksAsync(long clipId);
		Task<TextTrack> GetTextTrackAsync(long clipId, long trackId);
		Task<TextTrack> UpdateTextTrackAsync(long clipId, long trackId, TextTrack track);
		Task<TextTrack> UploadTextTrackFileAsync(IBinaryContent fileContent, long videoId, TextTrack track);
		Task DeleteTextTrackAsync(long clipId, long trackId);

		// Uploading Files
		UploadTicket GetUploadTicket();
		Task<UploadTicket> GetUploadTicketAsync();
        UploadTicket GetReplaceVideoUploadTicket(long videoId);
        Task<UploadTicket> GetReplaceVideoUploadTicketAsync(long videoId);
		IUploadRequest UploadEntireFile(IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE, long? replaceVideoId = null);
		Task<IUploadRequest> UploadEntireFileAsync(IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE, long? replaceVideoId = null);
		VerifyUploadResponse VerifyUploadFile(IUploadRequest uploadRequest);
		Task<VerifyUploadResponse> VerifyUploadFileAsync(IUploadRequest uploadRequest);
		IUploadRequest StartUploadFile(IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE, long? replaceVideoId = null);
		Task<IUploadRequest> StartUploadFileAsync(IBinaryContent fileContent, int chunkSize = VimeoClient.DEFAULT_UPLOAD_CHUNK_SIZE, long? replaceVideoId = null);
		VerifyUploadResponse ContinueUploadFile(IUploadRequest uploadRequest);
		Task<VerifyUploadResponse> ContinueUploadFileAsync(IUploadRequest uploadRequest);
		void CompleteFileUpload(IUploadRequest uploadRequest);
		Task CompleteFileUploadAsync(IUploadRequest uploadRequest);

		// Account Information
		User GetAccountInformation();
		Task<User> GetAccountInformationAsync();
		Task<User> UpdateAccountInformationAsync(EditUserParameters parameters);
		User UpdateAccountInformation(EditUserParameters parameters);

		// Albums
		Task<Paginated<Album>> GetAlbumsAsync(GetAlbumsParameters parameters = null);					
		Task<Paginated<Album>> GetAlbumsAsync(long userId, GetAlbumsParameters parameters = null);
		Task<Album> GetAlbumAsync(long albumId);
		Task<Album> GetAlbumAsync(long userId, long albumId);
		Task<Album> CreateAlbumAsync(EditAlbumParameters parameters = null);
		Task<Album> UpdateAlbumAsync(long albumId, EditAlbumParameters parameters = null);
		Task<bool> DeleteAlbumAsync(long albumId);
		Task<bool> AddToAlbumAsync(long albumId, long clipId);
		Task<bool> AddToAlbumAsync(long userId, long albumId, long clipId);
		Task<bool> RemoveFromAlbumAsync(long albumId, long clipId);
		Task<bool> RemoveFromAlbumAsync(long userId, long albumId, long clipId);

		Paginated<Album> GetAlbums(GetAlbumsParameters parameters = null);
		Paginated<Album> GetAlbums(long userId, GetAlbumsParameters parameters = null);
		Album GetAlbum(long albumId);
		Album GetAlbum(long userId, long albumId);
		Album CreateAlbum(EditAlbumParameters parameters = null);
		Album UpdateAlbum(long albumId, EditAlbumParameters parameters = null);
		bool DeleteAlbum(long albumId);
		bool AddToAlbum(long albumId, long clipId);
		bool AddToAlbum(long userId, long albumId, long clipId);
		bool RemoveFromAlbum(long albumId, long clipId);
		bool RemoveFromAlbum(long userId, long albumId, long clipId);

		// Deleting Videos
		void DeleteVideo(long clipId);
		Task DeleteVideoAsync(long clipId);
    }
}