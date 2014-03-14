using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VimeoDotNet.Authorization;
using VimeoDotNet.Constants;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
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
            return GetAccountInformationAsync().Result;
        }

        #endregion

        #region Videos

        public Paginated<Video> GetAccountVideos()
        {
            return GetAccountVideosAsync().Result;
        }

        public Paginated<Video> GetUserVideos(string userId)
        {
            return GetUserVideosAsync(userId).Result;
        }

        #endregion

        #region Upload

        public UploadTicket GetUploadTicket()
        {
            return GetUploadTicketAsync().Result;
        }

        public IUploadRequest StartUploadFile(IBinaryContent fileContent, int chunkSize = DEFAULT_UPLOAD_CHUNK_SIZE)
        {
            return StartUploadFileAsync(fileContent, chunkSize).Result;
        }

        public IUploadRequest UploadEntireFile(IBinaryContent fileContent, int chunkSize = DEFAULT_UPLOAD_CHUNK_SIZE)
        {
            return UploadEntireFileAsync(fileContent, chunkSize).Result;
        }

        public VerifyUploadResponse ContinueUploadFile(IUploadRequest uploadRequest)
        {
            return ContinueUploadFileAsync(uploadRequest).Result;
        }
        
        public VerifyUploadResponse VerifyUploadFile(IUploadRequest uploadRequest) {
            return VerifyUploadFileAsync(uploadRequest).Result;
        }

        public void CompleteFileUpload(IUploadRequest uploadRequest) {
            CompleteFileUploadAsync(uploadRequest).RunSynchronously();
        }

        #endregion
    }
}
