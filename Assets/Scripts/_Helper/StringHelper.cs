using System;
using System.Security.Cryptography;
using System.Text;
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
		public static string GenerateNewGuid()
		{
			return Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Get a unique hash.
		/// </summary>
		/// <param name="input">Typically just Application.dataPath</param>
		/// <returns></returns>
		public static string GetMd5Hash(string input)
		{
			MD5 md5 = MD5.Create();
			byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				sb.Append(data[i].ToString("x2"));
			}

			return sb.ToString();
		}
	}
}