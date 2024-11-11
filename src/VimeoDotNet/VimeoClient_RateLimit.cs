using System;
using System.Globalization;
using System.Linq;
using VimeoDotNet.Net;

namespace VimeoDotNet
{
    /// <summary>
    /// Class VimeoClient.
    /// Implements the <see cref="VimeoDotNet.IVimeoClient" />
    /// </summary>
    /// <seealso cref="VimeoDotNet.IVimeoClient" />
    public partial class VimeoClient
    {
        /// <inheritdoc />
        public long RateLimit { get; private set; }

        /// <inheritdoc />
        public long RateLimitRemaining { get; private set; }

        /// <inheritdoc />
        public DateTime RateLimitReset { get; private set; }

        /// <summary>
        /// Updates the rate limit.
        /// </summary>
        /// <param name="response">The response.</param>
        private void UpdateRateLimit(IApiResponse response)
        {
            if (response.Headers == null || !response.Headers.Contains("X-RateLimit-Limit"))
            {
                RateLimit = 0;
            }
            else
            {
                RateLimit = Convert.ToInt64(response.Headers.GetValues("X-RateLimit-Limit").First());
            }

            if (response.Headers == null || !response.Headers.Contains("X-RateLimit-Remaining"))
            {
                RateLimitRemaining = 0;
            }
            else
            {
                RateLimitRemaining = Convert.ToInt64(response.Headers.GetValues("X-RateLimit-Remaining").First());
            }

            if (response.Headers == null || !response.Headers.Contains("X-RateLimit-Reset"))
            {
                RateLimitReset = DateTime.UtcNow;
            }
            else
            {
                RateLimitReset = DateTime.ParseExact(response.Headers.GetValues("X-RateLimit-Reset").First(),
                    "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }
        }
    }
}