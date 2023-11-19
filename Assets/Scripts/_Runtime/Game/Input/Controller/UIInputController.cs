using UnityEngine;

using Personal.Manager;
using Personal.UI;
using Helper;

namespace Personal.InputProcessing
{
	public class UIInputController : InputControllerBase
	{
		bool isPressedCancel;

		void OnEnable()
		{
			inputReaderDefinition.OnMoveEvent += MoveInput;
			inputReaderDefinition.OnLookEvent += LookInput;

			inputReaderDefinition.OnTabSwitchEvent += TabSwitch;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CloseMenu;

			inputReaderDefinition.OnMenuUIDefaultPressedEvent += DefaultOptionMenu;

			inputReaderDefinition.OnMenuUICancelledEvent += ClosePauseMenu;

			inputReaderDefinition.OnDialogueUIFastForwardPressed_StartEvent += FastForwardStart;
			inputReaderDefinition.OnDialogueUIFastForwardPressed_EndEvent += FastForwardEnd;
		}

		void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		void LookInput(Vector2 newLookDirection)
		{
			Look = newLookDirection;
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
		/// --------------------------- Dialogue events ----------------------------
		/// ------------------------------------------------------------------------

		void FastForwardStart()
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Dialogue) return;

			StageManager.Instance.DialogueController.DialogueSkip.Begin(true);
		}

		void FastForwardEnd()
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Dialogue) return;

			StageManager.Instance.DialogueController.DialogueSkip.Begin(false);
		}

		/// ------------------------------------------------------------------------
		/// ---------------------------- Option events -----------------------------
		/// ------------------------------------------------------------------------

		protected override void CloseMenu()
		{
			// Since gamepad uses the start button to open/close the initial pause menu, do not allow the cancel button to close it.
			if (UIManager.Instance.ActiveInterfaceType == UIInterfaceType.Pause && !InputManager.Instance.IsCurrentDeviceMouse) return;

			base.CloseMenu();

			// Since the escape button closses both the option and pause menu, prevent it from closing both at the same time.
			isPressedCancel = true;
			CoroutineHelper.WaitNextFrame(() => isPressedCancel = false);
		}

		void ClosePauseMenu()
		{
			if (isPressedCancel) return;
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Pause) return;

			base.CloseMenu();
		}

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

		protected override void OnDisable()
		{
			base.OnDisable();

			inputReaderDefinition.OnMoveEvent -= MoveInput;
			inputReaderDefinition.OnLookEvent -= LookInput;

			inputReaderDefinition.OnTabSwitchEvent -= TabSwitch;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CloseMenu;

			inputReaderDefinition.OnMenuUIDefaultPressedEvent -= DefaultOptionMenu;

			inputReaderDefinition.OnMenuUICancelledEvent -= ClosePauseMenu;

			inputReaderDefinition.OnDialogueUIFastForwardPressed_StartEvent -= FastForwardStart;
			inputReaderDefinition.OnDialogueUIFastForwardPressed_EndEvent -= FastForwardEnd;
		}
	}
}