using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	public static class ComponentExtensions
	{
		public static List<T> GetComponentsRecursive<T>(this GameObject gameObject) where T : Component
		{
			List<T> componentList = new List<T>();
			GetComponentDeepSearch(gameObject.transform, componentList);

			return componentList;
		}

		static void GetComponentDeepSearch<T>(Transform parent, List<T> componentList)
		{
			foreach (Transform child in parent)
			{
				T currentComponent = child.GetComponent<T>();

				if (currentComponent != null) componentList.Add(currentComponent);
				if (child.childCount > 0) GetComponentDeepSearch(child, componentList);
			}
		}
	}
}
