using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        #region Authorization

        public AccessTokenResponse GetAccessToken(string authorizationCode, string redirectUrl)
        {
            return Task.Run(async () => await OAuth2Client.GetAccessTokenAsync(authorizationCode, redirectUrl)).Result;
        }

        #endregion

        #region Account

        public User GetAccountInformation()
        {
            try
            {
                return Task.Run(async () => await GetAccountInformationAsync()).Result;
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
                return Task.Run(async () => await GetUserInformationAsync(userId)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        #endregion

        #region Videos

        public Paginated<Video> GetVideos(string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetVideosAsync(fieldsCsv:fieldsCsv)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Video GetVideo(long clipId, string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetVideoAsync(clipId, fieldsCsv)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Video> GetUserVideos(long userId, string fieldsCsv = null)
        {
            return GetUserVideos(userId, null, null, fieldsCsv);
        }

        public Paginated<Video> GetUserVideos(long userId, int? page, int? perPage, string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetUserVideosAsync(userId, page, perPage, fieldsCsv)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Video GetUserVideo(long userId, long clipId, string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetUserVideoAsync(userId, clipId)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Video> GetAlbumVideos(long albumId, string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetAlbumVideosAsync(albumId)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Video GetAlbumVideo(long albumId, long clipId, string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetAlbumVideoAsync(albumId, clipId)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public Paginated<Video> GetUserAlbumVideos(long userId, long albumId, int? page = null, int? perPage = null, string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetUserAlbumVideosAsync(userId, albumId, page, perPage, fieldsCsv)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }




        public Video GetUserAlbumVideo(long userId, long albumId, long clipId, string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetUserAlbumVideoAsync(userId, albumId, clipId)).Result;
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

        #region Album
        public Paginated<Album> GetAlbums(long? userId, int? page = null, int? perPage = null, string fieldsCsv = null)
        {
            try
            {
                return Task.Run(async () => await GetAlbumsAsync(userId, page, perPage, fieldsCsv)).Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                return null;
            }
        }

        public void AddVideoToAlbum(long? userId, long albumId, long clipId)
        {
            try
            {
                AddVideoToAlbumAsync(userId, albumId, clipId).Wait();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }
        public void RemoveVideoFromAlbum(long? userId, long albumId, long clipId)
        {
            try
            {
                RemoveVideoFromAlbumAsynch(userId, albumId, clipId).Wait();
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
                return Task.Run(async () => await GetUploadTicketAsync()).Result;
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
                return Task.Run(async () => await StartUploadFileAsync(fileContent, chunkSize)).Result;
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
                return Task.Run(async () => await UploadEntireFileAsync(fileContent, chunkSize)).Result;
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
                return Task.Run(async () => await ContinueUploadFileAsync(uploadRequest)).Result;
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
                return Task.Run(async () => await VerifyUploadFileAsync(uploadRequest)).Result;
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