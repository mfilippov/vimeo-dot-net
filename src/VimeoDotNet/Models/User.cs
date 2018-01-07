using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id => ModelHelpers.ParseModelUriId(Uri);

        /// <summary>
        /// URI
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        /// <summary>
        /// Bio
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        /// <summary>
        /// Content filter
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "content_filter")]
        public string[] ContentFilter { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Web sites
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "websites")]
        public List<Website> Websites { get; set; }

        /// <summary>
        /// Stats
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "stats")]
        public UserStats Stats { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public UserMetadata Metadata { get; set; }

        /// <summary>
        /// Upload quota
        /// </summary>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload_quota")]
        public UserUploadQuota UploadQuota { get; set; }

        /// <summary>
        /// Account type
        /// </summary>
        [PublicAPI]
        public AccountTypeEnum AccountType
        {
            get { return ModelHelpers.GetEnumValue<AccountTypeEnum>(Account); }
            set { Account = ModelHelpers.GetEnumString(value); }
        }
    }
}