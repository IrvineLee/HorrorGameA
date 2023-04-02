using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Helper
{
	public class UIHelper : MonoBehaviour
	{
		static List<RaycastResult> raycastResults = new List<RaycastResult>();

		// TODO : This can be further improved by visiting
		// https://forum.unity.com/threads/eventsystem-raycastall-alternative-workaround.372234/
		// to avoid the garbage collection from the raycast.
		public static bool IsHittingUILayer(int layerIndex)
		{
			if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0))
			{
				PointerEventData pointer = new PointerEventData(EventSystem.current);
				pointer.position = Input.mousePosition;

				raycastResults.Clear();
				EventSystem.current.RaycastAll(pointer, raycastResults);

				foreach (var go in raycastResults)
				{
					if (go.gameObject.layer == layerIndex) return true;
				}
			}
			return false;
		}
	}
}