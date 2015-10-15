using System;
using System.Runtime.ExceptionServices;
using VimeoDotNet.Extensions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        #region Authorization

        public AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl)
        {
            return OAuth2Client.GetAccessTokenAsync(authorizationCode, redirectUrl).RunSynchronouslyWithCurrentCulture();
        }

        #endregion

        #region Account

        public User GetAccountInformation()
        {
            try
            {
                return GetAccountInformationAsync().RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public User UpdateAccountInformation(EditUserParameters parameters)
        {
            try
            {
                return UpdateAccountInformationAsync(parameters).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public User GetUserInformation(long userId)
        {
            try
            {
                return GetUserInformationAsync(userId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        #endregion

        #region Albums

        public Paginated<Album> GetAlbums(long userId, GetAlbumsParameters parameters = null)
        {
            try
            {
                return GetAlbumsAsync(userId, parameters).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Album> GetAlbums(GetAlbumsParameters parameters = null)
        {
            try
            {
                return GetAlbumsAsync(parameters).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Album CreateAlbum(EditAlbumParameters parameters = null)
        {
            try
            {
                return CreateAlbumAsync(parameters).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Album UpdateAlbum(long albumId, EditAlbumParameters parameters = null)
        {
            try
            {
                return UpdateAlbumAsync(albumId, parameters).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public bool DeleteAlbum(long albumId)
        {
            try
            {
                return DeleteAlbumAsync(albumId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return false;
            }
        }

        #endregion

        #region Videos

        public Paginated<Video> GetVideos()
        {
            try
            {
                return GetVideosAsync().RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Video GetVideo(long clipId)
        {
            try
            {
                return GetVideoAsync(clipId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Video> GetUserVideos(long userId, string query = null)
        {
            return GetUserVideos(userId, null, null, query);
        }

        public Paginated<Video> GetUserVideos(long userId, int? page, int? perPage, string query = null)
        {
            try
            {
                return GetUserVideosAsync(userId, page, perPage, query).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Video GetUserVideo(long userId, long clipId)
        {
            try
            {
                return GetUserVideoAsync(userId, clipId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Video> GetAlbumVideos(long albumId, int? page, int? perPage, string sort = null, string direction = null)
        {
            try
            {
                return GetAlbumVideosAsync(albumId, page, perPage, sort, direction).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Video GetAlbumVideo(long albumId, long clipId)
        {
            try
            {
                return GetAlbumVideoAsync(albumId, clipId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Video> GetUserAlbumVideos(long userId, long albumId)
        {
            try
            {
                return GetUserAlbumVideosAsync(userId, albumId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Video GetUserAlbumVideo(long userId, long albumId, long clipId)
        {
            try
            {
                return GetUserAlbumVideoAsync(userId, albumId, clipId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public void UpdateVideoMetadata(long clipId, VideoUpdateMetadata metaData)
        {
            try
            {
                UpdateVideoMetadataAsync(clipId, metaData).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        public void DeleteVideo(long clipId)
        {
            try
            {
                DeleteVideoAsync(clipId).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        #endregion

        #region Upload

        public UploadTicket GetUploadTicket()
        {
            try
            {
                return GetUploadTicketAsync().RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public IUploadRequest StartUploadFile(IBinaryContent fileContent, int chunkSize = DEFAULT_UPLOAD_CHUNK_SIZE)
        {
            try
            {
                return StartUploadFileAsync(fileContent, chunkSize).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public IUploadRequest UploadEntireFile(IBinaryContent fileContent, int chunkSize = DEFAULT_UPLOAD_CHUNK_SIZE)
        {
            try
            {
                return UploadEntireFileAsync(fileContent, chunkSize).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public VerifyUploadResponse ContinueUploadFile(IUploadRequest uploadRequest)
        {
            try
            {
                return ContinueUploadFileAsync(uploadRequest).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public VerifyUploadResponse VerifyUploadFile(IUploadRequest uploadRequest)
        {
            try
            {
                return VerifyUploadFileAsync(uploadRequest).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public void CompleteFileUpload(IUploadRequest uploadRequest)
        {
            try
            {
                CompleteFileUploadAsync(uploadRequest).RunSynchronouslyWithCurrentCulture();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        #endregion
    }
}