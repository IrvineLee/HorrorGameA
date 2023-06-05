using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Personal.UI
{
	public class UISelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] bool isInitialSelection = false;

		MenuUIBase menuUIBase = null;

		void OnEnable()
		{
			menuUIBase = GetComponentInParent<MenuUIBase>();

			if (!isInitialSelection) return;

			EventSystem.current.SetSelectedGameObject(gameObject);
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			eventData.selectedObject = gameObject;
			menuUIBase.SetLastSelectedGO(gameObject);
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			eventData.selectedObject = null;
		}
	}
}
