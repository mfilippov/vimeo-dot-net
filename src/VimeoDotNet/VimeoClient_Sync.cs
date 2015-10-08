using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
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
            return OAuth2Client.GetAccessTokenAsync(authorizationCode, redirectUrl).Result;
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

		#region Albums

		public Paginated<Album> GetUserAlbums(long userId, GetAlbumsParameters parameters = null)
		{
			try
			{
				return Task.Run(async () => await GetUserAlbumsAsync(userId, parameters)).Result;
			}
			catch (AggregateException ex)
			{
				ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
				return null;
			}
		}

		public Paginated<Album> GetAccountAlbums(GetAlbumsParameters parameters = null)
		{
			try
			{
				return Task.Run(async () => await GetAccountAlbumsAsync(parameters)).Result;
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
                return Task.Run(async () => await GetVideosAsync()).Result;
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
                return Task.Run(async () => await GetVideoAsync(clipId)).Result;
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
                return Task.Run(async () => await GetUserVideosAsync(userId, page, perPage, query)).Result;
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
                return Task.Run(async () => await GetUserVideoAsync(userId, clipId)).Result;
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
                return Task.Run(async () => await GetAlbumVideosAsync(albumId, page, perPage, sort, direction)).Result;
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
                return Task.Run(async () => await GetAlbumVideoAsync(albumId, clipId)).Result;
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
                return Task.Run(async () => await GetUserAlbumVideosAsync(userId, albumId)).Result;
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
                Task.Run(async () => await UpdateVideoMetadataAsync(clipId, metaData)).Wait();
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
                Task.Run(async () => await DeleteVideoAsync(clipId)).Wait();
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
                Task.Run(async () => await CompleteFileUploadAsync(uploadRequest)).Wait();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        #endregion
    }
}