namespace VimeoDotNet.Tests.Settings
{
    public class VimeoApiTestSettings
    {
        public VimeoApiTestSettings()
        {
            UserId = 2433258;
            PublicUserId = 115220313;
            PublicVideoId = 417178750;
            AccessToken = "5oGVeY4GQKr4l4T/wYS64Q==";
        }

        /// <summary>
        /// OAuth client Id
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// OAuth client secret
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// OAuth access token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Test Content Settings for Me
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Embed preset id (available only on paid account)
        /// </summary>
        public long EmbedPresetId { get; set; }
        
        /// <summary>
        /// User id for testing purpose 
        /// </summary>
        public long PublicUserId { get; }
        
        /// <summary>
        /// Public video id
        /// </summary>
        public long PublicVideoId { get; }
    }
}