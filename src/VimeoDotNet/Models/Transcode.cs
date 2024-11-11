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
        /// <summary>
        /// The status mappings
        /// </summary>
        private static readonly IDictionary<string, string> StatusMappings = new Dictionary<string, string>
        {
            {"complete", "Complete"},
            {"error", "Error"},
            {"in_progress", "InProgress"}
        };

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// State
        /// </summary>
        /// <value>The transcode status.</value>
        [PublicAPI]
        public TranscodeStatusEnum TranscodeStatus
        {
            get => ModelHelpers.GetEnumValue<TranscodeStatusEnum>(Status, StatusMappings);
            set => Status = ModelHelpers.GetEnumString(value, StatusMappings);
        }
    }
}