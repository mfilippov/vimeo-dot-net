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

            var pieces = uri.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var idString = pieces[pieces.Length - 1];
            if (idString.Contains(":"))
            {
                idString = idString.Split(':')[0];
            }

            if (long.TryParse(idString, out var userId))
            {
                return userId;
            }

            return null;
        }

        public static T GetEnumValue<T>(string value, IDictionary<string, string> mappings = null) where T : struct
        {
            if (Enum.TryParse(FindKeyMapping(value, mappings), true, out T enumVal))
                return enumVal;
            return !Enum.TryParse("Unknown", true, out enumVal) ? default(T) : enumVal;
        }

        public static string GetEnumString(Enum value, IDictionary<string, string> mappings = null)
        {
            var sVal = FindValueMapping(value.ToString(), mappings);
            return string.Compare(sVal, "Unknown", StringComparison.OrdinalIgnoreCase) == 0 ? null : sVal;
        }

        private static string FindKeyMapping(string key, IDictionary<string, string> mappings)
        {
            if (key == null || mappings == null)
            {
                return key;
            }

            var found = mappings.Keys.FirstOrDefault(k =>
                string.Compare(k, key, StringComparison.OrdinalIgnoreCase) == 0);
            if (found != null)
                return mappings[found];
            return !mappings.ContainsKey(key) ? key : mappings[key];
        }

        private static string FindValueMapping(string value, IDictionary<string, string> mappings)
        {
            if (value == null || mappings == null)
            {
                return value;
            }

            var found = mappings.Keys
                .FirstOrDefault(k => string.Compare(mappings[k], value, StringComparison.OrdinalIgnoreCase) == 0);
            return found ?? value;
        }
    }
}