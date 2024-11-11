using System;
using System.Collections.Generic;
using System.Linq;

namespace VimeoDotNet.Helpers
{
    /// <summary>
    /// Class ModelHelpers.
    /// </summary>
    internal static class ModelHelpers
    {
        /// <summary>
        /// Parses the model URI identifier.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        public static long? ParseModelUriId(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            var pieces = uri.Split(new[] {'/', '?'}, StringSplitOptions.RemoveEmptyEntries);

            for (int pieceIndex = pieces.Length - 1; pieceIndex >= 0; pieceIndex--)
            {
                var idString = pieces[pieceIndex];
                if (idString.Contains(":"))
                {
                    idString = idString.Split(':')[0];
                }

                if (long.TryParse(idString, out var id))
                {
                    return id;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="mappings">The mappings.</param>
        /// <returns>T.</returns>
        public static T GetEnumValue<T>(string value, IDictionary<string, string> mappings = null) where T : struct
        {
            if (Enum.TryParse(FindKeyMapping(value, mappings), true, out T enumVal))
                return enumVal;
            return !Enum.TryParse("Unknown", true, out enumVal) ? default(T) : enumVal;
        }

        /// <summary>
        /// Gets the enum string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mappings">The mappings.</param>
        /// <returns>System.String.</returns>
        public static string GetEnumString(Enum value, IDictionary<string, string> mappings = null)
        {
            var sVal = FindValueMapping(value.ToString(), mappings);
            return string.Compare(sVal, "Unknown", StringComparison.OrdinalIgnoreCase) == 0 ? null : sVal;
        }

        /// <summary>
        /// Finds the key mapping.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="mappings">The mappings.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Finds the value mapping.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mappings">The mappings.</param>
        /// <returns>System.String.</returns>
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