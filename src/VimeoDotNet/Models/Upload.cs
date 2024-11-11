using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class Upload.
    /// </summary>
    public class Upload
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
        /// The approach mappings
        /// </summary>
        private static readonly IDictionary<string, string> ApproachMappings = new Dictionary<string, string>
        {
            {"post", "Post"},
            {"pull", "Pull"},
            {"tus", "Tus"}
        };

        /// <summary>
        /// Gets or sets the approach.
        /// </summary>
        /// <value>The approach.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "approach")]
        [CanBeNull]
        private string approach { get; set; }

        /// <summary>
        /// Gets or sets the approach.
        /// </summary>
        /// <value>The approach.</value>
        [PublicAPI]
        [CanBeNull]
        public string Approach
        {
            get => approach != null && ApproachMappings.ContainsKey(approach) ? ApproachMappings[approach] : approach;
            set
            {
                foreach (var approachMapping in ApproachMappings)
                {
                    if (value == approachMapping.Value)
                    {
                        approach = approachMapping.Key;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>The form.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "form")]
        [CanBeNull]
        public string Form { get; set; }

        /// <summary>
        /// Gets or sets the gcsu identifier.
        /// </summary>
        /// <value>The gcsu identifier.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "gcs_uid")]
        [CanBeNull]
        public string GCSUId { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        [CanBeNull]
        public string Link { get; set; }


        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>The redirect URL.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "redirect_url")]
        [CanBeNull]
        public string RedirectUrl { get; set; }

        ///// <summary>
        ///// Gets or sets the size.
        ///// </summary>
        ///// <value>The size.</value>
        //[PublicAPI]
        //[JsonProperty(PropertyName = "size")]
        //[CanBeNull]
        //public long? Size { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "status")]
        [CanBeNull]
        // ReSharper disable once InconsistentNaming
        private string status { get; set; }


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [PublicAPI]
        [CanBeNull]
        public string Status
        {
            get => status != null && StatusMappings.ContainsKey(status) ? StatusMappings [status] : status;
            set
            {
                foreach (var statusMapping in StatusMappings)
                {
                    if (value == statusMapping.Value)
                    {
                        status = statusMapping.Key;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the upload link.
        /// </summary>
        /// <value>The upload link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload_link")]
        [CanBeNull]
        public string UploadLink { get; set; }
    }
}
