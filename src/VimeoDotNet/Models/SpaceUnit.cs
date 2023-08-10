using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VimeoDotNet.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum SpaceUnit
{
    [EnumMember(Value = "video_size")]
    VideoSize, 
    [EnumMember(Value = "video_count")]
    VideoCount 
}