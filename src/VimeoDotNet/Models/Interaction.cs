using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models;

public class Interaction
{
    [PublicAPI]
    [JsonProperty(PropertyName = "comment")]
    public Comment Comment { get; set; }
}