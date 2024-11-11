using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models;

/// <summary>
/// Class Permissions.
/// </summary>
public class Permissions
{
    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    /// <value>The comment.</value>
    [PublicAPI]
    [JsonProperty(PropertyName = "comment")]
    public string Comment { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Permissions" /> is add.
    /// </summary>
    /// <value><c>true</c> if add; otherwise, <c>false</c>.</value>
    [PublicAPI]
    [JsonProperty(PropertyName = "add")]
    public bool Add { get; set; }
}