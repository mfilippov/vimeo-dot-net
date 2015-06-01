using System;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class VideoUpdateMetadata
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public VideoPrivacyEnum Privacy { get; set; }
        public VideoEmbedPrivacyEnum EmbedPrivacy { get; set; }
        public bool ReviewLinkEnabled { get; set; }
        public string Password { get; set; }
    }
}