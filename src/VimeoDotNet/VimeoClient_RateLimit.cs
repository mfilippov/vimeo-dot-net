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
		public long RateLimit { get; private set; } = 0;
		public long RateLimitRemaining { get; private set; } = 0;
		public DateTime RateLimitReset { get; private set; }

		public bool RateLimitUpdatingOn { get; set; } = false;

		private void UpdateRateLimit(IRestResponse response)
		{
			if (!RateLimitUpdatingOn) { return; }
			try
			{

			foreach (var header in response.Headers)
			{
				switch (header.Name)
				{
					case "X-RateLimit-Limit":
						RateLimit = Convert.ToInt64(header.Value.ToString());
						break;
					case "X-RateLimit-Remaining":
						RateLimitRemaining = Convert.ToInt64(header.Value.ToString());
						break;
					case "X-RateLimit-Reset":
						RateLimitReset = DateTime.Parse(header.Value.ToString());
						break;
					default:
						break;
				}
			}
			} catch (Exception)
			{
				// Do not let a failure here break the call
				RateLimit = 0;
				RateLimitRemaining = 0;
				RateLimitReset = DateTime.UtcNow;
			}
		}
	}
}