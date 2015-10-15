using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Parameters;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
	public enum EditAlbumPrivacyOption
	{
		Anybody,
		Password
	}

	public enum EditAlbumSortOption
	{
		Arranged,
		Newest,
		Oldest,
		Plays,
		Comments,
		Likes,
		[ParameterValue("added_first")]
		AddedFirst,
		[ParameterValue("added_last")]
		AddedLast,
		Alphbetical
	}

	public class EditAlbumParameters : IParameterProvider
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public EditAlbumPrivacyOption? Privacy { get; set; }

		public string Password { get; set; }

		public EditAlbumSortOption? Sort { get; set; }

		public string ValidationError()
		{
			if (Privacy.HasValue && Privacy.Value == EditAlbumPrivacyOption.Password && Password == null)
			{
				return "Password is required if Privacy value is set to Password.";
			}

			return null;
		}

		public IDictionary<string, string> GetParameterValues()
		{
			Dictionary<string, string> parameterValues = new Dictionary<string, string>();

			if (Privacy.HasValue) { parameterValues.Add("privacy", Privacy.Value.GetParameterValue()); }
			if (Sort.HasValue) { parameterValues.Add("sort", Sort.Value.GetParameterValue()); }

			if (Name != null) { parameterValues.Add("name", Name); }
			if (Description != null) { parameterValues.Add("description", Description); }
			if (Password != null) { parameterValues.Add("password", Password); }

			if (parameterValues.Keys.Count > 0) { return parameterValues; }

			return null;

		}
	}
}
