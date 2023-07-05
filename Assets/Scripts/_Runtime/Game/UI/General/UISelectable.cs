using UnityEngine;
using UnityEngine.EventSystems;

using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	public class UISelectable : GameInitialize, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] bool isInitialSelection = false;
		[SerializeField] bool isPointerExitable = false;

		MenuUIBase menuUIBase = null;

		protected override void Initialize()
		{
			menuUIBase = GetComponentInParent<MenuUIBase>();
			OnEnable();
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
			if (UIManager.Instance.ActiveInterfaceType != menuUIBase.UiInterfaceType) return;
			if (!isPointerExitable) return;

			eventData.selectedObject = null;
		}
	}
}
