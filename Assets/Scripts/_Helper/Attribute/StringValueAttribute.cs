using System;
using System.Reflection;
using UnityEngine;

namespace Helper
{
	public class StringValueAttribute : PropertyAttribute
	{
		public string StringValue { get; private set; }
		public StringValueAttribute(string value) { StringValue = value; }
	}

	public static class StringValueExtensions
	{
		public static string GetStringValue(this Enum value)
		{
			// Get the type
			Type type = value.GetType();

			// Get fieldinfo for this type
			FieldInfo fieldInfo = type.GetField(value.ToString());

			// Get the stringvalue attributes
			StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
				typeof(StringValueAttribute), false) as StringValueAttribute[];

			// Return the first if there was a match.
			return attribs.Length > 0 ? attribs[0].StringValue : null;
		}
	}
}