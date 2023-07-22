using System.Runtime.Serialization;

namespace VimeoDotNet.Enums
{
    /// <summary>
    /// A video's 360 stereo (audio) format
    /// </summary>
    public enum StereoFormatEnum
    {
        /// <summary>
        /// The stereo format is left-right
        /// </summary>
        [EnumMember(Value = "left-right")]
        LeftRight,

        /// <summary>
        /// The audio is monaural
        /// </summary>
        Mono,

        /// <summary>
        /// The stereo format is top-bottom
        /// </summary>
        [EnumMember(Value = "top-bottom")]
        TopBottom
    }
}