using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		/// Add 'symbol' of 'length' in front of 'value'.
		/// If the 'symbol' 'length' is 3 while the 'value' length is 1, the symbol length will be reduced to 2.
		/// Ex. 'symbol' = 0, 'length' = 3, = 000 |and| 'value' = 5, length = 1, it will become 005.
		/// Ex. 'symbol' = A, 'length' = 5, = AAAAA |and| 'value' = 123, length = 3, it will become AA123.
		/// </summary>
		public static string AddSymbolInFront(char symbol, int length, string value)
		{
			string s = "";
			for (int i = 0; i < length - value.Length; i++)
			{
				s += symbol;
			}

			return s + value;
		}

		/// <summary>
		/// Let the 1st letter be upper-case. 'john doe' to John doe.
		/// 'ignoreStr' is to ignore the specified char/string. Ex: " ". 'john doe' to John Doe.
		/// </summary>
		public static string FirstLetterToUpper(string s, string ignoreStr = "")
		{
			if (String.IsNullOrEmpty(s))
				throw new ArgumentException("Is empty or null", "s");

			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);

			if (!String.IsNullOrEmpty(ignoreStr))
			{
				List<int> indexList = AllIndexesOf(s, ignoreStr);
				for (int i = 0; i < indexList.Count; i++)
				{
					int index = indexList[i] + 1;
					a[index] = char.ToUpper(a[index]);
				}
			}

			return new string(a);
		}

		/// <summary>
		/// Receives string and returns the string with its letters reversed.
		/// </summary>
		public static string ReverseString(string s)
		{
			char[] arr = s.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
		}

		/// <summary>
		/// Get all indexes of 'toFindStr' in 's'. 
		/// Ex: " " / ","
		/// </summary>
		public static List<int> AllIndexesOf(string s, string toFindStr)
		{
			if (String.IsNullOrEmpty(toFindStr))
				throw new ArgumentException("Is empty or null", "toFindStr");

			List<int> indexes = new List<int>();
			for (int index = 0; ; index += toFindStr.Length)
			{
				index = s.IndexOf(toFindStr, index);

				if (index == -1) return indexes;
				indexes.Add(index);
			}
		}

		/// <summary>
		/// Convert string to Vector3. 
		/// </summary>
		public static Vector3 StringToVector3(string sVector)
		{
			// Remove the parentheses
			if (sVector.StartsWith("(") && sVector.EndsWith(")"))
			{
				sVector = sVector.Substring(1, sVector.Length - 2);
			}
			else if (sVector.StartsWith("\"") && sVector.EndsWith("\""))
			{
				sVector = sVector.Substring(1, sVector.Length - 2);
			}

			// split the items
			string[] sArray = sVector.Split(',');

			// store as a Vector3
			Vector3 result = new Vector3(
				float.Parse(sArray[0]),
				float.Parse(sArray[1]),
				float.Parse(sArray[2]));

			return result;
		}

		/// <summary>
		/// Duplicate 's' by 'amount' with an optional 'divider' in between and 'endingWithDivider'.
		/// </summary>
		public static string GetString(string s, int amount, char divider = default, bool endWithDivider = false)
		{
			string newStr = "";
			for (int i = 0; i < amount; i++)
			{
				if (i == amount - 1 && !endWithDivider) newStr += s;
				else newStr += s + divider;
			}
			return newStr;
		}

		/// <summary>
		/// Remove white spaces.
		/// </summary>
		public static string RemoveAllWhiteSpaces(string s)
		{
			return string.Concat(s.Where(c => !char.IsWhiteSpace(c)));
		}

		/// <summary>
		/// Remove all characters including and after 'c'
		/// Ex: s = '100 <sprite=0>', c = '<', will return '100'.
		/// </summary>
		public static string RemoveAllIncludingAndAfter(string s, char c)
		{
			int index = s.IndexOf(c);
			return index >= 0 ? s.Substring(0, index) : s;
		}

		/// <summary>
		/// Convert string to any json.
		/// </summary>
		public static void ConvertStringToJson<T>(string s, ref T definition) where T : ScriptableObject, new()
		{
			if (String.IsNullOrEmpty(s))
				return;

			T temp = new T();
			JsonUtility.FromJsonOverwrite(s, temp);
			definition = temp;
		}

		public static string Repeat(string str, int times)
		{
			if (times <= 0) { return String.Empty; }

			return string.Concat(Enumerable.Repeat(str, times));
		}
	}
}