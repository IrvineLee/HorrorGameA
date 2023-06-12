using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	public class ButtonPressedDisableColor : GameInitialize, IPointerClickHandler
	{
		Button button;

		protected override void Initialize()
		{
			button = GetComponentInChildren<Button>();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			UIManager.Instance.OptionUI.EnableAllTabButtons();
			button.interactable = false;
		}
	}
}
