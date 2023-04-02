using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Helper
{
	public class ObjectHelper : MonoBehaviour
	{
		static List<GameObject> childrenTagList = new List<GameObject>();

		/// <summary>
		/// Perform a deep Copy of the object.
		/// </summary>
		/// <typeparam name="T">The type of object being copied.</typeparam>
		/// <param name="source">The object instance to copy.</param>
		/// <returns>The copied object.</returns>
		/// http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx (Binary Serialization)
		public static T Clone<T>(T source)
		{
			if (!typeof(T).IsSerializable)
			{
				throw new ArgumentException("The type must be serializable.", nameof(source));
			}

			// Don't serialize a null object, simply return the default for that object
			if (ReferenceEquals(source, null))
			{
				return default(T);
			}

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			using (stream)
			{
				formatter.Serialize(stream, source);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}

		/// <summary>
		/// Set layer recurcively.
		/// </summary>
		public static void SetLayerRecursively(GameObject obj, int newLayer)
		{
			if (null == obj)
				return;

			obj.layer = newLayer;

			foreach (Transform child in obj.transform)
			{
				if (null == child)
					continue;

				SetLayerRecursively(child.gameObject, newLayer);
			}
		}

		/// <summary>
		/// Traverse the hierarchy for every children.
		/// </summary>
		public static void TraverseHierarchy(Transform root, Action<Transform> action)
		{
			foreach (Transform child in root)
			{
				action(child);
				TraverseHierarchy(child, action);
			}
		}

		/// <summary>
		/// Find the 1st instance of child(does not go to grandchildren or lower) with the tag 'tag' in 'parent'.
		/// </summary>
		public static GameObject FindChildWithTag(Transform parent, string tag)
		{
			foreach (Transform child in parent)
			{
				if (child.CompareTag(tag)) return child.gameObject;
			}
			return null;
		}

		/// <summary>
		/// Find the 1st instance of child(however deep) with the tag 'tag' in 'parent'.
		/// </summary>
		public static GameObject FindDeepChildWithTag(Transform parent, string tag)
		{
			foreach (Transform child in parent)
			{
				if (child.CompareTag(tag)) return child.gameObject;
				if (child.childCount > 0)
				{
					GameObject go = FindDeepChildWithTag(child, tag);
					if (go) return go;
				}
			}
			return null;
		}

		/// <summary>
		/// Find all instances of children(does not go to grandchildren or lower) with the tag 'tag' in 'parent'.
		/// </summary>
		public static List<GameObject> FindChildrenWithTag(Transform parent, string tag)
		{
			childrenTagList.Clear();
			foreach (Transform child in parent)
			{
				if (child.CompareTag(tag)) childrenTagList.Add(child.gameObject);
			}

			return childrenTagList;
		}

		/// <summary>
		/// Find all instances of children(however deep) with the tag 'tag' in 'parent'.
		/// </summary>
		public static List<GameObject> FindDeepChildrenWithTag(Transform parent, string tag)
		{
			childrenTagList.Clear();
			FindDeepChildrenWithTagRecursive(parent, tag);

			return childrenTagList;
		}

		//public static GameObject GetFirstDeepestChild(Transform parent)
		//{
		//	Transform deepestChild = parent;

		//}

		static List<GameObject> FindDeepChildrenWithTagRecursive(Transform parent, string tag)
		{
			foreach (Transform child in parent)
			{
				if (child.CompareTag(tag)) childrenTagList.Add(child.gameObject);
				if (child.childCount > 0) FindDeepChildrenWithTag(child, tag);
			}

			return childrenTagList;
		}

		//static Transform GetFirstDeepestChildRecurcive(Transform parent, ref int currentLevelDeep)
		//{
		//	if (parent.childCount <= 0) return parent;

		//	Transform deepestChild = parent.GetChild(0);
		//	int childCount = deepestChild.childCount;

		//	currentLevelDeep++;

		//	foreach (Transform child in parent)
		//	{
		//		if (child.childCount >= childCount)
		//		{
		//			childCount = child.childCount;

		//			Transform someChild = GetFirstDeepestChildRecurcive(child, ref currentLevelDeep);
		//			if (currentLevelDeep)
		//			deepestChild = GetFirstDeepestChildRecurcive(child, ref currentLevelDeep);
		//		}
		//	}

		//	return deepestChild;
		//}

		/// <summary>
		/// Enable and disable gameobjects.
		/// </summary>
		public static void EnableDisableGO(GameObject go1, GameObject go2, bool isEnable1 = true, bool isEnable2 = false)
		{
			go1.SetActive(isEnable1);
			go2.SetActive(isEnable2);
		}
	}
}