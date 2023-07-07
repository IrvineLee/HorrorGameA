using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Personal.Manager;
using Personal.UI.Window;

namespace Personal.UI
{
	public class UISelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		[SerializeField] bool isInitialSelection = false;

		MenuUIBase menuUIBase = null;
		WindowSelectionUIAnimator windowSelectionUIAnimator;

		List<Selectable> selectableList = new();

		void Awake()
		{
			menuUIBase = GetComponentInParent<MenuUIBase>(true);
			windowSelectionUIAnimator = GetComponentInChildren<WindowSelectionUIAnimator>(true);

			selectableList = GetComponentsInChildren<Selectable>(true).ToList();
		}

		void OnEnable()
		{
			if (!isInitialSelection) return;

			EventSystem.current.SetSelectedGameObject(gameObject);
			menuUIBase.SetLastSelectedGO(gameObject);
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (UIManager.Instance.ActiveInterfaceType != menuUIBase.UiInterfaceType) return;

			eventData.selectedObject = gameObject;
			menuUIBase.SetLastSelectedGO(gameObject);
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (!UIManager.Instance) return;
			if (UIManager.Instance.ActiveInterfaceType != menuUIBase.UiInterfaceType) return;

			eventData.selectedObject = null;
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			windowSelectionUIAnimator?.Run(true);

			foreach (var selectable in selectableList)
			{
				selectable.targetGraphic.color = selectable.colors.selectedColor;
			}
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			windowSelectionUIAnimator?.Run(false);

			foreach (var selectable in selectableList)
			{
				selectable.targetGraphic.color = selectable.colors.normalColor;
			}
		}
	}
}
