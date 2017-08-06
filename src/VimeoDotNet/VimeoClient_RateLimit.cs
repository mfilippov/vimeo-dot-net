using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using VimeoDotNet.Net;

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
				if (_headers == null || !_headers.Contains("X-RateLimit-Limit"))
					return 0;
				return Convert.ToInt64(_headers.GetValues("X-RateLimit-Limit").First());
			}
		}

	    /// <summary>
	    /// Return remaning rate limit
	    /// </summary>
	    public long RateLimitRemaining
		{
			get
			{
				if (_headers == null || !_headers.Contains("X-RateLimit-Remaining"))
					return 0;
				return Convert.ToInt64(_headers.GetValues("X-RateLimit-Remaining").First());
			}
		}

	    /// <summary>
	    /// Return rate limit reset time
	    /// </summary>
	    public DateTime RateLimitReset
		{
			get
			{
				if (_headers == null || !_headers.Contains("X-RateLimit-Reset"))
					return DateTime.UtcNow;
				return DateTime.Parse(_headers.GetValues("X-RateLimit-Reset").First());
			}
		}

		private HttpResponseHeaders _headers = null;
		private void UpdateRateLimit(IApiResponse response)
		{
			_headers = response.Headers;
		}
	}
}