namespace VimeoDotNet.Tests.Settings
{
    public class VimeoApiTestSettings
    {
        public VimeoApiTestSettings()
        {
            UserId = 2433258;
            PublicUserId = 115220313;
            PublicVideoId = 417178750;
            EmbedPresetId = 1;
            ClientId = "b9ba3be18a6747e30b60a5108be3c567ee362535";
            ClientSecret = "yN9Os06F8I410SZnYmkKymy3kIoaxLX3QzJZ91ZHFdr9o9on7fviI/ZCxiWeT47piuf5A+1oMn2ks9JmAuhnR5ilvqVZPhJ3qH6nIEBwjlaHXa5qEygrPPSuDya8L+QW";
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