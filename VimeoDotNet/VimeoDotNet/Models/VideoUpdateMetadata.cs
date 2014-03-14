using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class VideoUpdateMetadata
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public VideoPrivacyEnum Privacy { get; set; }
        public bool ReviewLinkEnabled { get; set; }
    }
}
