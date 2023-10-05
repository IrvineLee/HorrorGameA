using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helper
{
	public static class StringExtensions
	{
		/// <summary>
		/// Let the 1st letter be upper-case. 'john doe' to John doe.
		/// 'ignoreStr' is to ignore the specified char/string. Ex: " ". 'john doe' to John Doe.
		/// </summary>
		public static string FirstLetterToUpper(this string source, string ignoreStr = "")
		{
			if (String.IsNullOrEmpty(source))
				throw new ArgumentException("Is empty or null", "s");

			char[] a = source.ToCharArray();
			a[0] = char.ToUpper(a[0]);

			if (!String.IsNullOrEmpty(ignoreStr))
			{
				List<int> indexList = source.AllIndexesOf(ignoreStr);
				for (int i = 0; i < indexList.Count; i++)
				{
					int index = indexList[i] + 1;
					a[index] = char.ToUpper(a[index]);
				}
			}

			return new string(a);
		}

		/// <summary>
		/// Add 'symbol' of 'length' in front of 'source'.
		/// If the 'symbol' 'length' is 3 while the 'source' length is 1, the symbol length will be reduced to 2.
		/// Ex. 'symbol' = 0, 'length' = 3, = 000 |and| 'source' = 5, it will become 005.
		/// Ex. 'symbol' = A, 'length' = 5, = AAAAA |and| 'source' = 123, it will become AA123.
		/// </summary>
		public static string AddSymbolInFront(this string source, char symbol, int length)
		{
			string s = "";
			for (int i = 0; i < length - source.Length; i++)
			{
				s += symbol;
			}

			return s + source;
		}

		/// <summary>
		/// Receives string and returns the string with its letters reversed.
		/// </summary>
		public static string ReverseString(this string source)
		{
			char[] arr = source.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
		}


		/// <summary>
		/// Get all indexes of 'toFindStr' in 's'. 
		/// Ex: " " / ","
		/// </summary>
		public static List<int> AllIndexesOf(this string source, string toFindStr)
		{
			if (String.IsNullOrEmpty(toFindStr))
				throw new ArgumentException("Is empty or null", "toFindStr");

			List<int> indexes = new List<int>();
			for (int index = 0; ; index += toFindStr.Length)
			{
				index = source.IndexOf(toFindStr, index);

				if (index == -1) return indexes;
				indexes.Add(index);
			}
		}

		/// <summary>
		/// Convert string to Vector3. 
		/// </summary>
		public static Vector3 StringToVector3(this string source)
		{
			// Remove the parentheses
			if (source.StartsWith("(") && source.EndsWith(")"))
			{
				source = source.Substring(1, source.Length - 2);
			}
			else if (source.StartsWith("\"") && source.EndsWith("\""))
			{
				source = source.Substring(1, source.Length - 2);
			}

			// split the items
			string[] sArray = source.Split(',');

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
		public static string GetString(this string source, int amount, char divider = default, bool endWithDivider = false)
		{
			string newStr = "";
			for (int i = 0; i < amount; i++)
			{
				if (i == amount - 1 && !endWithDivider) newStr += source;
				else newStr += source + divider;
			}
			return newStr;
		}

		/// <summary>
		/// Remove white spaces.
		/// </summary>
		public static string RemoveAllWhiteSpaces(this string source)
		{
			return string.Concat(source.Where(c => !char.IsWhiteSpace(c)));
		}

		/// <summary>
		/// Remove all characters including and after 'c'
		/// Ex: s = '100 <sprite=0>', c = '<', will return '100'.
		/// </summary>
		public static string SearchFrontRemoveEnd(this string source, char c, bool isRemoveChar = false)
		{
			int index = isRemoveChar ? source.IndexOf(c) : source.IndexOf(c) + 1;
			return index >= 0 ? source.Substring(0, index) : source;
		}

		/// <summary>
		/// Kill all characters after encountering char 'c'. Check behind to front.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="isRemoveC">Whether to remove character 'c'</param>
		/// <param name="isRemoveFromBack">Whether to remove all characters from the back or front</param>
		/// <returns></returns>
		public static string SearchBehindRemoveFrontOrEnd(this string source, char c, bool isRemoveC = false, bool isRemoveFromBack = true)
		{
			for (int i = source.Length - 1; i >= 0; i--)
			{
				if (source[i] != c) continue;

				int index = i;

				if (isRemoveFromBack)
				{
					if (!isRemoveC) index--;
					source = source.Substring(0, index);
				}
				else
				{
					if (isRemoveC) index++;
					source = source.Substring(index);
				}
				break;
			}
			return source;
		}

		/// <summary>
		/// Convert string to any json.
		/// </summary>
		public static void ConvertStringToJson<T>(this string source, ref T definition) where T : ScriptableObject, new()
		{
			if (String.IsNullOrEmpty(source))
				return;

			T temp = new T();
			JsonUtility.FromJsonOverwrite(source, temp);
			definition = temp;
		}

		/// <summary>
		/// Repeat the string 'times' times.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="times"></param>
		/// <returns></returns>
		public static string Repeat(this string source, int times)
		{
			if (times <= 0) { return String.Empty; }

			return string.Concat(Enumerable.Repeat(source, times));
		}

		/// <summary>
		/// See whether it contains the 'toCheck' string.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="toCheck"></param>
		/// <param name="comp"></param>
		/// <returns></returns>
		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return source?.IndexOf(toCheck, comp) >= 0;
		}
	}
}
