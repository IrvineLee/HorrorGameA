using System.Collections.Generic;
using UnityEngine;

using static Helper.ClassHelper;

namespace Helper
{
	public static class ListExtensions
	{
		/// <summary>
		/// Stop all CoroutineRun.
		/// </summary>
		/// <param name="originalList"></param>
		public static void StopAllCoroutineRun(this List<CoroutineRun> originalList)
		{
			foreach (CoroutineRun cr in originalList)
			{
				cr.StopCoroutine();
			}
		}

		/// <summary>
		/// Returns the first value in the list if 'index' is over or below the list count.
		/// This is to avoid the 'index' going out of bounds.
		/// </summary>
		public static T GetObjectFromList<T>(this List<T> originalList, int index) where T : Object
		{
			if (index >= 0 && index < originalList.Count) return originalList[index];
			else if (originalList.Count > 0) return originalList[0];

			return default;
		}

		/// <summary>
		/// Get a random enum from 'enumList' without the value of 'without'.
		/// </summary>
		public static T GetRandomEnumWithout<T>(this List<T> enumList, T without) where T : System.Enum
		{
			T enumValue = (T)(object)(0);
			do
			{
				int randNum = Random.Range(0, enumList.Count);
				enumValue = enumList[randNum];

			} while (EqualityComparer<T>.Default.Equals(enumValue, without));

			return enumValue;
		}


		/// <summary>
		/// Convert the list to a list that combines all the duplicate type into count.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static List<TCount<T>> ConvertToDuplicateCountList<T>(this List<T> list)
		{
			List<T> tempList = new();
			List<TCount<T>> countList = new();

			foreach (var t in list)
			{
				// Do not add duplicates to countList.
				if (tempList.Contains(t)) continue;
				tempList.Add(t);

				int count = list.GetDuplicateCount(t);
				countList.Add(new TCount<T>(t, count));
			}
			return countList;
		}

		/// <summary>
		/// Get the duplicate type count.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static int GetDuplicateCount<T>(this List<T> list, T t)
		{
			int count = 0;
			foreach (var currentT in list)
			{
				if (currentT.GetType() == t.GetType()) count++;
			}
			return count;
		}

		/// <summary>
		/// Convert the list to a dictionary that combines all the duplicate types to value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static Dictionary<T, int> ConvertToDuplicateCountDictionary<T>(this List<T> list)
		{
			Dictionary<T, int> dictionary = new();

			foreach (var t in list)
			{
				// Add to dictionary.
				if (dictionary.ContainsKey(t))
				{
					dictionary.AddTo(t);
					continue;
				}

				dictionary.Add(t, 1);
			}
			return dictionary;
		}
	}
}
