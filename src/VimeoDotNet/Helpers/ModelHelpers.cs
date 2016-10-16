using System;
using System.Collections.Generic;
using System.Linq;

namespace VimeoDotNet.Helpers
{
    internal static class ModelHelpers
    {
        public static long? ParseModelUriId(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }
            string[] pieces = uri.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            long userId = 0;
            if (long.TryParse(pieces[pieces.Length - 1], out userId))
            {
                return userId;
            }
            return null;
        }

        public static T GetEnumValue<T>(string value, IDictionary<string, string> mappings = null) where T : struct
        {
            T enumVal;
            if (!Enum.TryParse(FindKeyMapping(value, mappings), true, out enumVal))
            {
                if (!Enum.TryParse("Unknown", true, out enumVal))
                {
                    return default(T);
                }
            }
            return enumVal;
        }

        public static string GetEnumString(Enum value, IDictionary<string, string> mappings = null)
        {
            string sVal = FindValueMapping(value.ToString(), mappings);
            if (string.Compare(sVal, "Unknown", true) == 0)
            {
                return null;
            }
            return sVal;
        }

        private static string FindKeyMapping(string key, IDictionary<string, string> mappings)
        {
            if (key == null || mappings == null)
            {
                return key;
            }
            string found = mappings.Keys.FirstOrDefault(k => string.Compare(k, key, true) == 0);
            if (found == null)
            {
                if (!mappings.ContainsKey(key))
                {
                    return key;
                }
                return mappings[key];
            }
            return mappings[found];
        }

        private static string FindValueMapping(string value, IDictionary<string, string> mappings)
        {
            if (value == null || mappings == null)
            {
                return value;
            }
            string found = mappings.Keys.FirstOrDefault(k => string.Compare(mappings[k], value, true) == 0);
            if (found == null)
            {
                return value;
            }
            return found;
        }
    }
}