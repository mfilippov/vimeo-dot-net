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

    public class EditChannelParameters : IParameterProvider
    {
        /// <summary>
        /// The name of the channel
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the channel
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The link to access the channel. You can use a custom name in the URL in place of a numeric channel ID,
        /// as in /channels/{url_custom}
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// The privacy level of the channel
        /// </summary>
        public ChannelPrivacyOption? Privacy { get; set; }

        public string ValidationError()
        {
            return string.IsNullOrEmpty(Name) || Privacy == null? "Name and Privacy is required for channel" : null;
        }

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
