using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models;

/// <summary>
/// Class Interaction.
/// </summary>
public class Interaction
{
    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    /// <value>The comment.</value>
    [PublicAPI]
    [JsonProperty(PropertyName = "comment")]
    public Comment Comment { get; set; }
}