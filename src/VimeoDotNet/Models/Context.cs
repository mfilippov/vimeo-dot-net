using Newtonsoft.Json;
using JetBrains.Annotations;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class Context.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>The resource.</value>
        [JsonProperty(PropertyName = "resource")]
        [CanBeNull] 
        public object Resource { get; set; }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>The type of the resource.</value>
        [JsonProperty(PropertyName = "resource_type")]
        public string ResourceType { get; set; }
    }
}
