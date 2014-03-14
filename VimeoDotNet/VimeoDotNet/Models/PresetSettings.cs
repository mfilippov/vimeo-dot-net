using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VimeoDotNet.Models
{
    [Serializable]
    public class PresetSettings
    {
        public PresetButtons buttons { get; set; }
        public PresetLogos logos { get; set; }
        public string outro { get; set; }
        public string portrait { get; set; }
        public string title { get; set; }
        public string byline { get; set; }
        public bool badge { get; set; }
        public bool byline_badge { get; set; }
        public bool playbar { get; set; }
        public bool volume { get; set; }
        public bool fullscreen_button { get; set; }
        public bool scaling_button { get; set; }
        public bool autoplay { get; set; }
        public bool autopause { get; set; }
        public bool loop { get; set; }
        public string color { get; set; }
        public bool link { get; set; }
    }
}
