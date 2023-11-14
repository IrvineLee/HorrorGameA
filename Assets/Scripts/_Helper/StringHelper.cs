using System;
using UnityEngine;

namespace Helper
{
	public class StringHelper : MonoBehaviour
	{
		/// <summary>
		/// Format value to string and add in 'insertBlank' for every 'spaces'.
		/// Ex. To change 100000 to 100,000 or 100 000.
		/// </summary>
		public static string ToStringInsert<T>(T value, string insertBlank = " ", int spaces = 3) where T : IConvertible
		{
			string s = value.ToString();
			string newStr = "";

			int remaining = s.Length % spaces;
			if (remaining != 0)
			{
				newStr = s.Substring(0, remaining) + insertBlank;
				s = s.Remove(0, remaining);
			}

			for (int i = 0; i < s.Length; i += spaces)
			{
				newStr += s.Substring(i, spaces) + insertBlank;
			}

			return newStr.Remove(newStr.Length - 1);
		}

		/// <summary>
		/// Generate a new string guid.
		/// </summary>
		/// <param name="source"></param>
		public static void GenerateNewGuid(ref string source)
		{
			source = Guid.NewGuid().ToString();
		}
	}
}