using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VimeoDotNet.Net;
using System.IO;
using System.Reflection;
using VimeoDotNet.Models;

namespace VimeoDotNet.Tests
{
    [TestClass]
    [Ignore] // Comment this line to run integration tests.
    public class VimeoClient_IntegrationTests
    {
        private const string CLIENTID = "<YOUR CLIENT ID HERE>";
        private const string CLIENTSECRET = "<YOUR CLIENT SECRET HERE>";
        private const string ACCESSTOKEN = "<YOUR ACCOUNT ACCESS TOKEN HERE>";

        private const string TESTFILEPATH = @"Resources\test.mp4"; // http://download.wavetlan.com/SVV/Media/HTTP/http-mp4.htm

        [TestMethod]
        public void Integration_VimeoClient_GetUploadTicket_CanGenerateStreamingTicket()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var ticket = client.GetUploadTicket();

            // assert
            Assert.IsNotNull(ticket);
        }

        [TestMethod]
        public void Integration_VimeoClient_UploadEntireFile_UploadsFile()
        {
            // arrange
            long length;
            IUploadRequest completedRequest;
            Video video;
            using (var file = new BinaryContent(GetFullPath(TESTFILEPATH)))
            {
                length = file.Data.Length;
                var client = CreateAuthenticatedClient();

                // act
                completedRequest = client.UploadEntireFile(file);
            }

            // assert
            Assert.IsNotNull(completedRequest);
            Assert.IsTrue(completedRequest.AllBytesWritten);
            Assert.IsTrue(completedRequest.IsVerifiedComplete);
            Assert.AreEqual(length, completedRequest.BytesWritten);
            Assert.IsNotNull(completedRequest.ClipUri);
            Assert.IsTrue(completedRequest.ClipId > 0);

        }

        [TestMethod]
        public void Integration_VimeoClient_GetAccountInformation_RetrievesCurrentAccountInfo()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var account = client.GetAccountInformation();

            // assert
            Assert.IsNotNull(account);
        }

        [TestMethod]
        public void Integration_VimeoClient_GetAccountVideos_RetrievesCurrentAccountVideos()
        {
            // arrange
            var client = CreateAuthenticatedClient();

            // act
            var videos = client.GetAccountVideos();

            // assert
            Assert.IsNotNull(videos);
        }

        [TestMethod]
        public void Integration_VimeoClient_GetAccountVideo_RetrievesVideo()
        {
            // arrange
            long clipId = 89133196; // Your video ID here
            var client = CreateAuthenticatedClient();

            // act
            var video = client.GetAccountVideo(clipId);

            // assert
            Assert.IsNotNull(video);
        }

        private VimeoClient CreateUnauthenticatedClient()
        {
            return new VimeoClient(CLIENTID, CLIENTSECRET);
        }

        private VimeoClient CreateAuthenticatedClient()
        {
            return new VimeoClient(ACCESSTOKEN);
        }

        private string GetFullPath(string relativePath)
        {
            var dir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)); // /bin/debug
            return Path.Combine(dir.Parent.Parent.FullName, relativePath);
        }
    }
}
