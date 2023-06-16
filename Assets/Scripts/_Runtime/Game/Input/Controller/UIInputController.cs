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

			inputReaderDefinition.OnCancelEvent += CloseOptionMenu;
			inputReaderDefinition.OnCancelInventoryEvent += CloseInventoryMenu;

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

		/// ------------------------------------------------------------------------
		/// ---------------------------- Option events -----------------------------
		/// ------------------------------------------------------------------------

		void TabSwitch(bool isNext)
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Option) return;

			UIManager.Instance.OptionUI.NextTab(isNext);
		}

		void CloseOptionMenu()
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Option) return;

			UIManager.Instance.OptionUI.HandleCloseOptionMenu();
		}

		void DefaultOptionMenu()
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Option) return;

			UIManager.Instance.OptionUI.IDefaultHandler.ResetToDefault();
		}

		/// ------------------------------------------------------------------------
		/// -------------------------- Inventory events ----------------------------
		/// ------------------------------------------------------------------------
		void CloseInventoryMenu()
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Inventory) return;

			UIManager.Instance.InventoryUI.IWindowHandler.CloseWindow();
		}

		protected override void OnPostDisable()
		{
			base.OnPostDisable();

			inputReaderDefinition.OnMoveEvent -= MoveInput;
			inputReaderDefinition.OnTabSwitchEvent -= TabSwitch;

			inputReaderDefinition.OnInteractEvent -= InteractInput;

			inputReaderDefinition.OnCancelEvent -= CloseOptionMenu;
			inputReaderDefinition.OnCancelInventoryEvent -= CloseInventoryMenu;

			inputReaderDefinition.OnMenuUIDefaultPressedEvent -= DefaultOptionMenu;
		}
	}
}