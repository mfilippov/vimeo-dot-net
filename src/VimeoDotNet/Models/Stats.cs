using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models;

/// <summary>
/// Class Stats.
/// </summary>
public class Stats
{
    /// <summary>
    /// Gets or sets the plays.
    /// </summary>
    /// <value>The plays.</value>
    [PublicAPI]
    [JsonProperty(PropertyName = "plays")]
    public int Plays { get; set; }

    /// <summary>
    /// Gets or sets the likes.
    /// </summary>
    /// <value>The likes.</value>
    [PublicAPI]
    [JsonProperty(PropertyName = "likes")]
    public int Likes { get; set; }

    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    /// <value>The comments.</value>
    [PublicAPI]
    [JsonProperty(PropertyName = "comments")]
    public int Comments { get; set; }
}