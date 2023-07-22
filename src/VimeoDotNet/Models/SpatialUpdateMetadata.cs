using System.Collections.Generic;
using JetBrains.Annotations;
using VimeoDotNet.Enums;
using VimeoDotNet.Helpers;

namespace VimeoDotNet.Models
{
    public class SpatialUpdateMetadata
    {
        /// <summary>
        /// The video's 360 field of view value
        /// </summary>
        [PublicAPI]
        public int? FieldOfView { get; set; }

        /// <summary>
        /// The video's 360 spatial projection
        /// </summary>
        [PublicAPI]
        public SpatialProjectionEnum? Projection { get; set; }

        /// <summary>
        /// The video's 360 stereo format
        /// </summary>
        [PublicAPI]
        public StereoFormatEnum? StereoFormat { get; set; }

        public void SetParameterValues(Dictionary<string, string> parameters, string prefix)
        {
            if (FieldOfView.HasValue)
            {
                parameters[$"{prefix}.field_of_view"] = FieldOfView.Value.ToString();
            }

            if (Projection.HasValue)
            {
                parameters[$"{prefix}.projection"] = Projection.Value.MemberValue().ToLowerInvariant();
            }

            if (StereoFormat.HasValue)
            {
                parameters[$"{prefix}.stereo_format"] = StereoFormat.Value.MemberValue().ToLowerInvariant();
            }
        }
    }
}