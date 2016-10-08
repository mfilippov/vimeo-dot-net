using System;
using System.Collections.Generic;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// User
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? id
        {
            get { return ModelHelpers.ParseModelUriId(uri); }
        }

        /// <summary>
        /// URI
        /// </summary>
        public string uri { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// Location
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// Bio
        /// </summary>
        public string bio { get; set; }
        /// <summary>
        /// Created time
        /// </summary>
        public DateTime created_time { get; set; }
        /// <summary>
        /// Account
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// Content filter
        /// </summary>
        public string content_filter { get; set; }
        /// <summary>
        /// Pictures
        /// </summary>
        public List<Picture> pictures { get; set; }
        /// <summary>
        /// Web sites
        /// </summary>
        public List<Website> websites { get; set; }
        /// <summary>
        /// Stats
        /// </summary>
        public UserStats stats { get; set; }
        /// <summary>
        /// Metadata
        /// </summary>
        public UserMetadata metadata { get; set; }
        /// <summary>
        /// Upload quota
        /// </summary>
        public UserUploadQuota upload_quota { get; set; }

        /// <summary>
        /// Account type
        /// </summary>
        public AccountTypeEnum AccountType
        {
            get { return ModelHelpers.GetEnumValue<AccountTypeEnum>(account); }
            set { account = ModelHelpers.GetEnumString(value); }
        }
    }
}