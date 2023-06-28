using UnityEngine;

using Personal.Manager;
using Personal.UI;

namespace Personal.InputProcessing
{
	public class UIInputController : InputControllerBase
	{
		protected override void OnPostEnable()
		{
			inputReaderDefinition.OnMoveEvent += MoveInput;
			inputReaderDefinition.OnTabSwitchEvent += TabSwitch;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CloseMenu;

			inputReaderDefinition.OnMenuUIDefaultPressedEvent += DefaultOptionMenu;
		}

		void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		void InteractInput()
		{
			IsInteract = true;
		}

		void CancelInput()
		{
			IsCancel = true;
		}

		void CloseMenu()
		{
			// Dialogue will close by itself and pop from the stack.
			if (UIManager.Instance.ActiveInterfaceType == UIInterfaceType.Dialogue) return;

			if (UIManager.Instance.WindowStack.Count <= 0) return;
			UIManager.Instance.WindowStack.Peek().CloseWindow();
		}

		/// ------------------------------------------------------------------------
		/// ---------------------------- Option events -----------------------------
		/// ------------------------------------------------------------------------

		void TabSwitch(bool isNext)
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Option) return;

			UIManager.Instance.OptionUI.NextTab(isNext);
		}

		void DefaultOptionMenu()
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Option) return;

			UIManager.Instance.OptionUI.IDefaultHandler.ResetToDefault();
		}

		protected override void OnPostDisable()
		{
			base.OnPostDisable();

			inputReaderDefinition.OnMoveEvent -= MoveInput;
			inputReaderDefinition.OnTabSwitchEvent -= TabSwitch;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CloseMenu;

			inputReaderDefinition.OnMenuUIDefaultPressedEvent -= DefaultOptionMenu;
		}
	}
}