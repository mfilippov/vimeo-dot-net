namespace VimeoDotNet.Models
{
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    /// <summary>
    /// Class VideoPageSettings.
    /// </summary>
    public class VideoPageSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether [ask ai].
        /// </summary>
        /// <value><c>true</c> if [ask ai]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "ask_ai")]
        public bool AskAi { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is categories.
        /// </summary>
        /// <value><c>true</c> if categories; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "categories")]
        public bool Categories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is collections.
        /// </summary>
        /// <value><c>true</c> if collections; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "collections")]
        public bool Collections { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is comments.
        /// </summary>
        /// <value><c>true</c> if comments; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "comments")]
        public bool Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [creative commons].
        /// </summary>
        /// <value><c>true</c> if [creative commons]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "creative_commons")]
        public bool CreativeCommons { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is credits.
        /// </summary>
        /// <value><c>true</c> if credits; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "credits")]
        public bool Credits { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [date added].
        /// </summary>
        /// <value><c>true</c> if [date added]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "date_added")]
        public bool DateAdded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is description.
        /// </summary>
        /// <value><c>true</c> if description; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "description")]
        public bool Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is download.
        /// </summary>
        /// <value><c>true</c> if download; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "download")]
        public bool Download { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is like.
        /// </summary>
        /// <value><c>true</c> if like; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "like")]
        public bool Like { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is owner.
        /// </summary>
        /// <value><c>true</c> if owner; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "owner")]
        public bool Owner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is portrait.
        /// </summary>
        /// <value><c>true</c> if portrait; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "portrait")]
        public bool Portrait { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is share.
        /// </summary>
        /// <value><c>true</c> if share; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "share")]
        public bool Share { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is tags.
        /// </summary>
        /// <value><c>true</c> if tags; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "tags")]
        public bool Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is uploader.
        /// </summary>
        /// <value><c>true</c> if uploader; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uploader")]
        public bool Uploader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoPageSettings"/> is views.
        /// </summary>
        /// <value><c>true</c> if views; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "views")]
        public bool Views { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [watch later].
        /// </summary>
        /// <value><c>true</c> if [watch later]; otherwise, <c>false</c>.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "watch_later")]
        public bool WatchLater { get; set; }
    }

}
