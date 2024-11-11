using System.Collections.Generic;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
    /// <summary>
    /// Edit album privacy option
    /// </summary>
    public enum ChannelPrivacyOption
    {
        /// <summary>
        /// Anyone can access the channel
        /// </summary>
        Anybody,

        /// <summary>
        /// Only moderators can access the channel
        /// </summary>
        Moderators,

        /// <summary>
        /// Only moderators and designated users can access the channel
        /// </summary>
        User
    }

    /// <summary>
    /// Class EditChannelParameters.
    /// Implements the <see cref="VimeoDotNet.Parameters.IParameterProvider" />
    /// </summary>
    /// <seealso cref="VimeoDotNet.Parameters.IParameterProvider" />
    public class EditChannelParameters : IParameterProvider
    {
        /// <summary>
        /// The name of the channel
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// The description of the channel
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// The link to access the channel. You can use a custom name in the URL in place of a numeric channel ID,
        /// as in /channels/{url_custom}
        /// </summary>
        /// <value>The link.</value>
        public string Link { get; set; }

        /// <summary>
        /// The privacy level of the channel
        /// </summary>
        /// <value>The privacy.</value>
        public ChannelPrivacyOption? Privacy { get; set; }

        /// <summary>
        /// Performs validation and returns a description of the first error encountered.
        /// </summary>
        /// <returns>Description of first error, or null if none found.</returns>
        public string ValidationError()
        {
            return string.IsNullOrEmpty(Name) || Privacy == null? "Name and Privacy is required for channel" : null;
        }

        /// <summary>
        /// Provides universal interface to retrieve parameter values.
        /// </summary>
        /// <returns>Returns all parameters as name/value pairs.</returns>
        public IDictionary<string, string> GetParameterValues()
        {
            var parameterValues = new Dictionary<string, string>();

            if (Privacy.HasValue)
            {
                parameterValues.Add("privacy", Privacy.Value.GetParameterValue());
            }

            if (Name != null)
            {
                parameterValues.Add("name", Name);
            }

            if (Description != null)
            {
                parameterValues.Add("description", Description);
            }
            
            if (Link != null)
            {
                parameterValues.Add("link", Link);
            }

            return parameterValues.Keys.Count > 0 ? parameterValues : null;
        }
    }
}
