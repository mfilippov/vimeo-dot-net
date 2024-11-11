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
        /// The account type mappings
        /// </summary>
        private static readonly IDictionary<string, string> AccountTypeMappings = new Dictionary<string, string>
        {
            {"pro_unlimited", "ProUnlimited"}
        };

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The identifier.</value>
        public long? Id => ModelHelpers.ParseModelUriId(Uri);

        /// <summary>
        /// URI
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        /// <value>The location.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        /// <summary>
        /// Bio
        /// </summary>
        /// <value>The bio.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        /// <value>The created time.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        /// <value>The account.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        /// <summary>
        /// Content filter
        /// </summary>
        /// <value>The content filter.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "content_filter")]
        public string[] ContentFilter { get; set; }

        /// <summary>
        /// Pictures
        /// </summary>
        /// <value>The pictures.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Web sites
        /// </summary>
        /// <value>The websites.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "websites")]
        public List<Website> Websites { get; set; }

        /// <summary>
        /// Stats
        /// </summary>
        /// <value>The stats.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "stats")]
        public UserStats Stats { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        /// <value>The metadata.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public UserMetadata Metadata { get; set; }

        /// <summary>
        /// Upload quota
        /// </summary>
        /// <value>The upload quota.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "upload_quota")]
        public UserUploadQuota UploadQuota { get; set; }

        /// <summary>
        /// Account type
        /// </summary>
        /// <value>The type of the account.</value>
        [PublicAPI]
        public AccountTypeEnum AccountType
        {
            get => ModelHelpers.GetEnumValue<AccountTypeEnum>(Account, AccountTypeMappings);
            set => Account = ModelHelpers.GetEnumString(value);
        }
    }
}