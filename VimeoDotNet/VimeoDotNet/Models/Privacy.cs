using System;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Privacy
    {
        public string view { get; set; }
        public string embed { get; set; }
        public bool download { get; set; }
        public bool add { get; set; }

        public VideoPrivacyEnum ViewPrivacy
        {
            get { return ModelHelpers.GetEnumValue<VideoPrivacyEnum>(view); }
            set { view = ModelHelpers.GetEnumString(value); }
        }

        public VideoEmbedPrivacyEnum EmbedPrivacy
        {
            get { return ModelHelpers.GetEnumValue<VideoEmbedPrivacyEnum>(embed); }
            set { embed = ModelHelpers.GetEnumString(value); }
        }
    }
}
