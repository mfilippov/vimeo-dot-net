using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models;

/// <summary>
/// The total combined file size for all versions of the video, and the file size type.
/// </summary>
// https://developer.vimeo.com/api/reference/response/video-versions-size
public class VideoVersionsSize
{
    /// <summary>
    /// The status of the video and its corresponding file size type.
    /// </summary>
    [PublicAPI]
    [JsonProperty(PropertyName = "file_size_type")]
    public string FileSizeType { get; set; }

    /// <summary>
    /// The number of versions for the video.
    /// </summary>
    [PublicAPI]
    [JsonProperty(PropertyName = "num_versions")]
    public int NumVersions { get; set; }

    /// <summary>
    /// The total combined file size for all versions of the video.
    /// </summary>
    [PublicAPI]
    [JsonProperty(PropertyName = "total_size")]
    public long TotalSize { get; set; }
}
