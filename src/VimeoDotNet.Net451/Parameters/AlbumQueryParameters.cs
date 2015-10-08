using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
	public enum AlbumQuerySortOption
	{
		Date,
		Alphabetical,
		Videos,
		Duration
	}

	public enum AlbumQuerySortDirectionOption
	{
		Asc,
		Desc
	}

	public class AlbumQueryParameters : IParameterProvider
	{
		public int? Page { get; set; }

		public int? PerPage { get; set; }

		public string Query { get; set; }

		public AlbumQuerySortOption? Sort { get; set; }

		public AlbumQuerySortDirectionOption? Direction { get; set; }

		public string ValidationError()
		{
			if (PerPage > 50)
				return "Maximum number of items allowed per page is 50.";

			return null;
		}

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
