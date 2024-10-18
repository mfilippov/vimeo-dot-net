using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models;

public class Permissions
{
    [PublicAPI]
    [JsonProperty(PropertyName = "comment")]
    public string Comment { get; set; }

    [PublicAPI]
    [JsonProperty(PropertyName = "add")]
    public bool Add { get; set; }
}