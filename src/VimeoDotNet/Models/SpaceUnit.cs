using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VimeoDotNet.Models;

/// <summary>
/// Enum SpaceUnit
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum SpaceUnit
{
    /// <summary>
    /// The video size
    /// </summary>
    [EnumMember(Value = "video_size")]
    VideoSize,
    /// <summary>
    /// The video count
    /// </summary>
    [EnumMember(Value = "video_count")]
    VideoCount 
}