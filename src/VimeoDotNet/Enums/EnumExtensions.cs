using System;
using System.Reflection;

namespace VimeoDotNet.Enums
{
    /// <summary>
    /// Class ParameterValueAttribute.
    /// Implements the <see cref="Attribute" />
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Field)]
    internal class ParameterValueAttribute : Attribute
    {
        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>The text value.</value>
        public string TextValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterValueAttribute" /> class.
        /// </summary>
        /// <param name="textValue">The text value.</param>
        public ParameterValueAttribute(string textValue)
        {
            TextValue = textValue;
        }
    }

    /// <summary>
    /// Class EnumExtensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetParameterValue(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
                return value.ToString().ToLower();
            var field = type.GetRuntimeField(name);
            if (field == null)
                return value.ToString().ToLower();
            var attr = field.FieldType.GetTypeInfo().GetCustomAttribute<ParameterValueAttribute>();
            return attr != null ? attr.TextValue : value.ToString().ToLower();
        }
    }
}