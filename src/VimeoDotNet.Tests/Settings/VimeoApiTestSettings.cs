namespace VimeoDotNet.Tests.Settings
{
    public class VimeoApiTestSettings
    {
        // API Client Settings
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }

        // Test Content Settings for Me
        public long UserId { get; set; }
        public long AlbumId { get; set; }
        public long VideoId { get; set; }
        public long TextTrackId { get; set; }

        public long PublicUserId { get; set; }
    }
}