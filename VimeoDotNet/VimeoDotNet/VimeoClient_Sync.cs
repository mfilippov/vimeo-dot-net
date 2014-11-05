using System;
using System.Runtime.ExceptionServices;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        #region Authorization

        public AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl)
        {
            return OAuth2Client.GetAccessTokenAsync(authorizationCode, redirectUrl).Result;
        }

        #endregion

        #region Account

        public User GetAccountInformation()
        {
            try
            {
                return GetAccountInformationAsync().Result;
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
                return GetUserInformationAsync(userId).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        #endregion

        #region Videos

        public Paginated<Video> GetVideos()
        {
            try
            {
                return GetVideosAsync().Result;
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
                return GetVideoAsync(clipId).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Video> GetUserVideos(long userId)
        {
            try
            {
                return GetUserVideosAsync(userId).Result;
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
                return GetUserVideoAsync(userId, clipId).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Video> GetAlbumVideos(long albumId)
        {
            try
            {
                return GetAlbumVideosAsync(albumId).Result;
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
                return GetAlbumVideoAsync(albumId, clipId).Result;
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
                return GetUserAlbumVideosAsync(userId, albumId).Result;
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
                return GetUserAlbumVideoAsync(userId, albumId, clipId).Result;
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
                UpdateVideoMetadataAsync(clipId, metaData).Wait();
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
                DeleteVideoAsync(clipId).Wait();
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
                return GetUploadTicketAsync().Result;
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
                return StartUploadFileAsync(fileContent, chunkSize).Result;
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
                return UploadEntireFileAsync(fileContent, chunkSize).Result;
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
                return ContinueUploadFileAsync(uploadRequest).Result;
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
                return VerifyUploadFileAsync(uploadRequest).Result;
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
                CompleteFileUploadAsync(uploadRequest).Wait();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        #endregion
    }
}