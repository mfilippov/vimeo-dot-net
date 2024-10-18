using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Transcode
    /// </summary>
    public class Transcode
    {
        private static readonly IDictionary<string, string> StatusMappings = new Dictionary<string, string>
        {
            {"complete", "Complete"},
            {"error", "Error"},
            {"in_progress", "InProgress"}
        };

        [PublicAPI]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// State
        /// </summary>
        [PublicAPI]
        public TranscodeStatusEnum TranscodeStatus
        {
            get => ModelHelpers.GetEnumValue<TranscodeStatusEnum>(Status, StatusMappings);
            set => Status = ModelHelpers.GetEnumString(value, StatusMappings);
        }
    }
}