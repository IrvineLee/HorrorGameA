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

		/// <summary>
		/// +1 to dictionary value.
		/// </summary>
		public static void AddTo<TK>(this Dictionary<TK, int> dic, TK key, int value = 1)
		{
			if (!dic.ContainsKey(key))
			{
				dic.Add(key, value);
				return;
			}

			dic[key] += value;
		}

		/// <summary>
		/// This adds the key and value. If already existed, just update the value.
		/// </summary>
		/// <param name="dic"></param>
		/// <param name="id"></param>
		/// <param name="newValue"></param>
		public static void AddOrUpdateValue<TV>(this Dictionary<string, TV> dic, string id, TV newValue)
		{
			if (dic.TryGetValue(id, out TV value))
			{
				dic[id] = newValue;
				return;
			}

			dic.Add(id, newValue);
		}

		static TV GetValue<TDic, TK, TV>(this TDic dic, TK key, TV defaultValue) where TDic : IReadOnlyDictionary<TK, TV>
		{
			var value = dic.TryGetValue(key, out var result) ? result : defaultValue;

			//if (value == null)
			//	Debug.LogError("Key " + key + " is not found.");

			return value;
		}
	}
}
