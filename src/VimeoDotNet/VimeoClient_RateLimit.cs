using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VimeoDotNet
{
	public partial class VimeoClient
	{
	    /// <summary>
	    /// Return rate limit
	    /// </summary>
	    public long RateLimit
		{
			get
			{
				if (_headers != null)
				{
					var v = _headers.FirstOrDefault(h => h.Name.Equals("X-RateLimit-Limit"));
					return Convert.ToInt64(v != null ? v.Value.ToString() : "0");
				}
				return 0;
			}
		}

	    /// <summary>
	    /// Return remaning rate limit
	    /// </summary>
	    public long RateLimitRemaining
		{
			get
			{
				if (_headers != null)
				{
					var v = _headers.FirstOrDefault(h => h.Name.Equals("X-RateLimit-Remaining"));
					return Convert.ToInt64(v != null ? v.Value.ToString() : "0");
				}
				return 0;
			}
		}

	    /// <summary>
	    /// Return rate limit reset time
	    /// </summary>
	    public DateTime RateLimitReset
		{
			get
			{
				if (_headers != null)
				{
					var v = _headers.FirstOrDefault(h => h.Name.Equals("X-RateLimit-Reset"));
					if (v != null)
					{
						return DateTime.Parse(v.Value.ToString());
					}
				}
				return DateTime.UtcNow;
			}
		}

		private IList<Parameter> _headers = null;
		private void UpdateRateLimit(IRestResponse response)
		{
			_headers = response.Headers;
		}
	}
}