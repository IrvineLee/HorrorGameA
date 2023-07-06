using UnityEngine;
using UnityEngine.EventSystems;

using Personal.Manager;
using Personal.UI.Window;

namespace Personal.UI
{
	public class UISelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		[SerializeField] bool isInitialSelection = false;

		MenuUIBase menuUIBase = null;
		WindowUIAnimator windowUIAnimator;

		void Awake()
		{
			menuUIBase = GetComponentInParent<MenuUIBase>(true);
			windowUIAnimator = GetComponentInChildren<WindowUIAnimator>(true);
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
			windowUIAnimator?.Run(true);
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			windowUIAnimator?.Run(false);
		}
	}
}
