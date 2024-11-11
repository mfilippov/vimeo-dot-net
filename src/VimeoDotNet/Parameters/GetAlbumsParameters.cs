using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
    /// <summary>
    /// GetAlbumsSortOption
    /// </summary>
    [PublicAPI]
    public enum GetAlbumsSortOption
    {
        /// <summary>
        /// Date
        /// </summary>
        Date,

        /// <summary>
        /// Alphabetical
        /// </summary>
        Alphabetical,

        /// <summary>
        /// Videos
        /// </summary>
        Videos,

        /// <summary>
        /// Duration
        /// </summary>
        Duration
    }

    /// <summary>
    /// Get albums sort direction option
    /// </summary>
    [PublicAPI]
    public enum GetAlbumsSortDirectionOption
    {
        /// <summary>
        /// Ascending
        /// </summary>
        Asc,

        /// <summary>
        /// Descending
        /// </summary>
        Desc
    }

    /// <inheritdoc />
    public class GetAlbumsParameters : IParameterProvider
    {
        /// <summary>
        /// Page
        /// </summary>
        /// <value>The page.</value>
        [PublicAPI]
        public int? Page { get; set; }

        /// <summary>
        /// Per page
        /// </summary>
        /// <value>The per page.</value>
        [PublicAPI]
        public int? PerPage { get; set; }

        /// <summary>
        /// Query
        /// </summary>
        /// <value>The query.</value>
        [PublicAPI]
        public string Query { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        /// <value>The sort.</value>
        [PublicAPI]
        [JsonConverter(typeof(StringEnumConverter))]
        public GetAlbumsSortOption? Sort { get; set; }

        /// <summary>
        /// Direction
        /// </summary>
        /// <value>The direction.</value>
        [PublicAPI]
        [JsonConverter(typeof(StringEnumConverter))]
        public GetAlbumsSortDirectionOption? Direction { get; set; }

        /// <inheritdoc />
        public string ValidationError()
        {
            return PerPage > 50 ? "Maximum number of items allowed per page is 50." : null;
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetParameterValues()
        {
            var parameterValues = new Dictionary<string, string>();

            if (Page.HasValue)
            {
                parameterValues.Add("page", Page.Value.ToString());
            }

            if (PerPage.HasValue)
            {
                parameterValues.Add("per_page", PerPage.Value.ToString());
            }

            if (!string.IsNullOrEmpty(Query))
            {
                parameterValues.Add("query", Query);
            }

            if (Sort.HasValue)
            {
                parameterValues.Add("sort", Sort.Value.GetParameterValue());
            }

            if (Direction.HasValue)
            {
                parameterValues.Add("direction", Direction.Value.GetParameterValue());
            }

            return parameterValues.Keys.Count > 0 ? parameterValues : null;
        }
    }
}