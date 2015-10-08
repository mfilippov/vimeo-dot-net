using System;
using System.Reflection;

namespace VimeoDotNet.Enums
{
	[AttributeUsage(AttributeTargets.Enum, AllowMultiple=false, Inherited=false)]
	public class ParameterValueAttribute : Attribute
	{
		public string TextValue { get; set; }

		public ParameterValueAttribute(string textValue)
		{
			this.TextValue = textValue;
		}
	}

	public static class EnumExtensions
	{
		public static string GetParameterValue(this Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (name != null)
			{
				FieldInfo field = type.GetField(name);
				if (field != null)
				{
					ParameterValueAttribute attr = Attribute.GetCustomAttribute(field, typeof(ParameterValueAttribute)) as ParameterValueAttribute;
					if (attr != null)
					{
						return attr.TextValue;
					}
				}
			}
			return value.ToString().ToLower();
		}
	}
}