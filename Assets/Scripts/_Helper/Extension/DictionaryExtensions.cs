using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Get the value. If doesn't exist, return default.
		/// </summary>
		public static TV GetOrDefault<TK, TV>(this Dictionary<TK, TV> dic, TK key, TV defaultValue = default(TV))
		{
			return GetValue(dic, key, defaultValue);

		}

		/// <summary>
		/// Get the value. If doesn't exist, return default.
		/// </summary>
		public static TV GetOrDefault<TK, TV>(this IReadOnlyDictionary<TK, TV> dic, TK key, TV defaultValue = default(TV))
		{
			return GetValue(dic, key, defaultValue);
		}

		static TV GetValue<TDic, TK, TV>(this TDic dic, TK key, TV defaultValue) where TDic : IReadOnlyDictionary<TK, TV>
		{
			var value = dic.TryGetValue(key, out var result) ? result : defaultValue;

			if (value == null)
				Debug.LogError("Key " + key + " is not found.");

			return value;
		}
	}
}
