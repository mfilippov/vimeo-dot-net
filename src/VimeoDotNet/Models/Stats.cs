using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models;

public class Stats
{
    [PublicAPI]
    [JsonProperty(PropertyName = "plays")]
    public int Plays { get; set; }

    [PublicAPI]
    [JsonProperty(PropertyName = "likes")]
    public int Likes { get; set; }

    [PublicAPI]
    [JsonProperty(PropertyName = "comments")]
    public int Comments { get; set; }
}