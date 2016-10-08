using System;
using System.Collections.Generic;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
	/// <summary>
	/// GetAlbumsSortOption
	/// </summary>
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

	/// <summary>
	/// Get albums parameters
	/// </summary>
	public class GetAlbumsParameters : IParameterProvider
	{
		/// <summary>
		/// Page
		/// </summary>
		public int? Page { get; set; }

		/// <summary>
		/// Per page
		/// </summary>
		public int? PerPage { get; set; }

		/// <summary>
		/// Query
		/// </summary>
		public string Query { get; set; }

		/// <summary>
		/// Sort
		/// </summary>
		public GetAlbumsSortOption? Sort { get; set; }

		/// <summary>
		/// Direction
		/// </summary>
		public GetAlbumsSortDirectionOption? Direction { get; set; }

	    /// <summary>
	    /// Performs validation and returns a description of the first error encountered.
	    /// </summary>
	    /// <returns>Description of first error, or null if none found.</returns>
	    public string ValidationError()
		{
			if (PerPage > 50)
				return "Maximum number of items allowed per page is 50.";

			return null;
		}

	    /// <summary>
	    /// Provides universal interface to retrieve parameter values.
	    /// </summary>
	    /// <returns>Returns all parameters as name/value pairs.</returns>
	    public IDictionary<string, string> GetParameterValues()
		{
			Dictionary<string, string> parameterValues = new Dictionary<string, string>();

			if (Page.HasValue) { parameterValues.Add("page", Page.Value.ToString()); }
			if (PerPage.HasValue) { parameterValues.Add("per_page", PerPage.Value.ToString()); }
			if (!String.IsNullOrEmpty(Query)) { parameterValues.Add("query", Query); }
			if (Sort.HasValue) { parameterValues.Add("sort", Sort.Value.GetParameterValue()); }
			if (Direction.HasValue) { parameterValues.Add("direction", Direction.Value.GetParameterValue()); }

			if (parameterValues.Keys.Count > 0) { return parameterValues; }

			return null;
		}
	}
}
