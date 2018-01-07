using System.Collections.Generic;
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

    /// <summary>
    /// Edit album parameters
    /// </summary>
    public class EditAlbumParameters : IParameterProvider
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Privacy
        /// </summary>
        public EditAlbumPrivacyOption? Privacy { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public EditAlbumSortOption? Sort { get; set; }

        /// <summary>
        /// Performs validation and returns a description of the first error encountered.
        /// </summary>
        /// <returns>Description of first error, or null if none found.</returns>
        public string ValidationError()
        {
            if (Privacy.HasValue && Privacy.Value == EditAlbumPrivacyOption.Password && Password == null)
            {
                return "Password is required if Privacy value is set to Password.";
            }

            return null;
        }

        /// <summary>
        /// Provides universal interface to retrieve parameter values.
        /// </summary>
        /// <returns>Returns all parameters as name/value pairs.</returns>
        public IDictionary<string, string> GetParameterValues()
        {
            Dictionary<string, string> parameterValues = new Dictionary<string, string>();

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

            if (parameterValues.Keys.Count > 0)
            {
                return parameterValues;
            }

            return null;
        }
    }
}