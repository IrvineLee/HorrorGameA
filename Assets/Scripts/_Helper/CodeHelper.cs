using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	public class CodeHelper : MonoBehaviour
	{
		#region List

		/// <summary>
		/// Generate a list with a unique random numbers between 'startNumber' and 'endNumber'(exclude).
		/// </summary>
		public static List<int> GenerateRandomList(int startNumber, int endNumber, int totalUniqueNumber = -1)
		{
			List<int> uniqueNumbers = new List<int>();
			List<int> finishedList = new List<int>();

			for (int i = startNumber; i < endNumber; i++)
			{
				uniqueNumbers.Add(i);
			}

			if (totalUniqueNumber == -1) totalUniqueNumber = endNumber;
			for (int i = 0; i < totalUniqueNumber; i++)
			{
				int ranNum = uniqueNumbers[Random.Range(0, uniqueNumbers.Count)];
				finishedList.Add(ranNum);
				uniqueNumbers.Remove(ranNum);
			}

			return finishedList;
		}

		/// <summary>
		/// Generate a new list of size 'total' that takes random values(int) from 'numberList'.
		/// </summary>
		public static List<int> GenerateRandomList(List<int> numberList, int total)
		{
			List<int> finishedList = new List<int>();

			for (int i = 0; i < total; i++)
			{
				int ranNum = numberList[Random.Range(0, numberList.Count)];
				finishedList.Add(ranNum);
				numberList.Remove(ranNum);
			}

			return finishedList;
		}

		#endregion

		#region Dictionary

		/// <summary>
		/// Add 'value' to dicionary if key exist. Else add new key and value into dictionary.
		/// </summary>
		public static void AddToDictionary<TKey>(Dictionary<TKey, int> dictionary, TKey addT, int value)
		{
			// Check to see whether the key exist. 
			if (dictionary.TryGetValue(addT, out int num))
			{
				dictionary[addT] = num + value;
			}
			else
			{
				dictionary.Add(addT, value);
			}
		}

		/// <summary>
		/// Replace the 'value' in the dictionary if have the key and 'condition' is true. Else add new key and value into dictionary.
		/// </summary>
		public static void ReplaceInDictionary<TKey>(Dictionary<TKey, int> dictionary, TKey addT, int value, System.Func<int, bool> condition)
		{
			// Check to see whether the key exist. 
			if (dictionary.TryGetValue(addT, out int num))
			{
				if (condition(num)) dictionary[addT] = value;
			}
			else
			{
				dictionary.Add(addT, value);
			}
		}

		/// <summary>
		/// Are both dictionaries the same?
		/// </summary>
		public static bool CompareDictionaries<TKey, TValue>(Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2)
		{
			if (dict1 == dict2) return true;
			if ((dict1 == null) || (dict2 == null)) return false;
			if (dict1.Count != dict2.Count) return false;

			var valueComparer = EqualityComparer<TValue>.Default;

			foreach (var kvp in dict1)
			{
				TValue value2;
				if (!dict2.TryGetValue(kvp.Key, out value2)) return false;
				if (!valueComparer.Equals(kvp.Value, value2)) return false;
			}
			return true;
		}

		#endregion

		#region Rigidbody
		public static void RotateRigidBodyAroundPointBy(Rigidbody rb, Vector3 origin, Vector3 axis, float angle)
		{
			RotateRigidbodyAround(rb, origin, axis, angle);
		}

		public static void RotateRigidBodyAroundPointBy(Rigidbody2D rb, Vector3 origin, Vector3 axis, float angle)
		{
			RotateRigidbodyAround(rb, origin, axis, angle);
		}

		static void RotateRigidbodyAround<T>(T rb, Vector3 origin, Vector3 axis, float angle) where T : Component
		{
			Quaternion q = Quaternion.AngleAxis(angle, axis);

			if (typeof(T) == typeof(Rigidbody))
			{
				(rb as Rigidbody).MovePosition(q * (rb.transform.position - origin) + origin);
				(rb as Rigidbody).MoveRotation(rb.transform.rotation * q);
			}
			else if (typeof(T) == typeof(Rigidbody2D))
			{
				(rb as Rigidbody2D).MovePosition(q * (rb.transform.position - origin) + origin);
				(rb as Rigidbody2D).MoveRotation(rb.transform.rotation * q);
			}
		}
		#endregion

		#region Debug

		/// <summary>
		/// Debug.Log all values in list.
		/// </summary>
		public static void DebugInList<T>(List<T> list)
		{
			foreach (T child in list)
			{
				Debug.Log(child);
			}
		}

		/// <summary>
		/// Debug.Log all keys and values for dictionary.
		/// </summary>
		public static void DebugInDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, string color = "")
		{
			foreach (var dic in dictionary)
			{
				Debug.Log("<color=" + color + ">Key = " + dic.Key + ", Value = " + dic.Value + "</color>");
			}
		}

		#endregion
	}
}