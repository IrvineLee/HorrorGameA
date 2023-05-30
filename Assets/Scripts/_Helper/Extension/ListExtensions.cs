using System.Collections.Generic;
using UnityEngine;

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
	}
}
