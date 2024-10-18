// ***********************************************************************
// Assembly         : 
// Author           : Cemal
// Created          : 10-18-2024
//
// Last Modified By : Cemal
// Last Modified On : 10-18-2024
// ***********************************************************************
// <copyright file="Project.cs" company="Ozet Akademi">
//     Copyright (c) Ozet Akademi. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using JetBrains.Annotations;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class Project.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the pictures.
        /// </summary>
        /// <value>The pictures.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "pictures")]
        public Pictures Pictures { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "metadata")]
        public VideoMetadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets the embed.
        /// </summary>
        /// <value>The embed.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "embed")]
        public Embed Embed { get; set; }

        /// <summary>
        /// Gets or sets the stats.
        /// </summary>
        /// <value>The stats.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "stats")]
        public Stats Stats { get; set; }

        /// <summary>
        /// Gets or sets the privacy.
        /// </summary>
        /// <value>The privacy.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "privacy")]
        public Privacy Privacy { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "permissions")]
        public Permissions Permissions { get; set; }

        /// <summary>
        /// Gets or sets the interaction.
        /// </summary>
        /// <value>The interaction.</value>
        [PublicAPI]
        [JsonProperty(PropertyName = "interaction")]
        public Interaction Interaction { get; set; }
    }
}
