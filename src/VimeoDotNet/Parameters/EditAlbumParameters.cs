using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
    /// <summary>
    /// Edit album privacy option
    /// </summary>
    public enum EditAlbumPrivacyOption
    {
        /// <summary>
        /// Anybody
        /// </summary>
        Anybody,

        /// <summary>
        /// Password
        /// </summary>
        Password
    }

    /// <summary>
    /// Edit album sort option
    /// </summary>
    [PublicAPI]
    public enum EditAlbumSortOption
    {
        /// <summary>
        /// Arranged
        /// </summary>
        Arranged,

        /// <summary>
        /// Newest
        /// </summary>
        Newest,

        /// <summary>
        /// Oldest
        /// </summary>
        Oldest,

        /// <summary>
        /// Plays
        /// </summary>
        Plays,

        /// <summary>
        /// Comments
        /// </summary>
        Comments,

        /// <summary>
        /// Likes
        /// </summary>
        Likes,

        /// <summary>
        /// Added first
        /// </summary>
        [ParameterValue("added_first")] AddedFirst,

        /// <summary>
        /// Added last
        /// </summary>
        [ParameterValue("added_last")] AddedLast,

        /// <summary>
        /// Alphbetical
        /// </summary>
        Alphbetical
    }

    /// <inheritdoc />
    public class EditAlbumParameters : IParameterProvider
    {
        /// <summary>
        /// Name
        /// </summary>
        [PublicAPI]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [PublicAPI]
        public string Description { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        [PublicAPI]
        [JsonConverter(typeof(StringEnumConverter))]
        public EditAlbumPrivacyOption? Privacy { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [PublicAPI]
        public string Password { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        [PublicAPI]
        public EditAlbumSortOption? Sort { get; set; }

        /// <inheritdoc />
        public string ValidationError()
        {
            if (Privacy.HasValue && Privacy.Value == EditAlbumPrivacyOption.Password && Password == null)
            {
                return "Password is required if Privacy value is set to Password.";
            }

            return null;
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetParameterValues()
        {
            var parameterValues = new Dictionary<string, string>();

            if (Privacy.HasValue)
            {
                parameterValues.Add("privacy", Privacy.Value.GetParameterValue());
            }

            if (Sort.HasValue)
            {
                parameterValues.Add("sort", Sort.Value.GetParameterValue());
            }

            if (Name != null)
            {
                parameterValues.Add("name", Name);
            }

            if (Description != null)
            {
                parameterValues.Add("description", Description);
            }

            if (Password != null)
            {
                parameterValues.Add("password", Password);
            }

            return parameterValues.Keys.Count > 0 ? parameterValues : null;
        }
    }
}