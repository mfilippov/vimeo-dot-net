using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimeoDotNet.Helpers
{
    public static class ModelHelpers
    {
        public static long? ParseModelUriId(string uri)
        {
            if (string.IsNullOrEmpty(uri)) { return null; }
            string[] pieces = uri.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            long userId = 0;
            if (long.TryParse(pieces[pieces.Length - 1], out userId))
            {
                return userId;
            }
            return null;
        }
    }
}
