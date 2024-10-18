// ***********************************************************************
// Assembly         : 
// Author           : Cemal
// Created          : 10-18-2024
//
// Last Modified By : Cemal
// Last Modified On : 10-18-2024
// ***********************************************************************
// <copyright file="Context.cs" company="Ozet Akademi">
//     Copyright (c) Ozet Akademi. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
