using System;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class Picture
    {
        public string type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string link { get; set; }

        public PictureTypeEnum PictureType
        {
            get { return ModelHelpers.GetEnumValue<PictureTypeEnum>(type); }
            set { type = ModelHelpers.GetEnumString(value); }
        }
    }
}
