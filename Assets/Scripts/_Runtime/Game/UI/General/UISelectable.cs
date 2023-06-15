using UnityEngine;
using UnityEngine.EventSystems;

using Helper;
using Personal.GameState;

namespace Personal.UI
{
	public class UISelectable : GameInitialize, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] bool isInitialSelection = false;
		[SerializeField] bool isPointerExitable = false;

		MenuUIBase menuUIBase = null;

		protected override void OnPostEnable()
		{
			menuUIBase = GetComponentInParent<MenuUIBase>();

			if (!isInitialSelection) return;

			CoroutineHelper.WaitFor(0.5f, () => EventSystem.current.SetSelectedGameObject(gameObject));
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			eventData.selectedObject = gameObject;
			menuUIBase.SetLastSelectedGO(gameObject);
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (!isPointerExitable) return;
			eventData.selectedObject = null;
		}
	}
}
