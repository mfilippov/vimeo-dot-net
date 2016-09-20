using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Enums;
using VimeoDotNet.Exceptions;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
	public partial class VimeoClient : IVimeoClient
	{
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