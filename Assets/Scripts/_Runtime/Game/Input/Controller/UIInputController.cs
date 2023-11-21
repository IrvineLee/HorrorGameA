using UnityEngine;

using Personal.Manager;
using Personal.UI;

namespace Personal.InputProcessing
{
	public class UIInputController : InputControllerBase
	{
		protected override void OnEnable()
		{
			base.OnEnable();

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

		void MoveInput(Vector2 direction)
		{
			Move = direction;
			IControlInput?.Move(direction);
		}

		void LookInput(Vector2 direction)
		{
			Look = direction;
			IControlInput?.RightAnalog_Look(direction);
		}

		void InteractInput()
		{
			IsInteract = true;
			IControlInput?.ButtonSouth_Submit();
		}

		void CancelInput()
		{
			IsCancel = true;
			IControlInput?.ButtonEast_Cancel();
		}

		/// ------------------------------------------------------------------------
		/// --------------------------- Dialogue events ----------------------------
		/// ------------------------------------------------------------------------

		void FastForwardStart()
		{
			ButtonNorth(UIInterfaceType.Dialogue);
		}

		void FastForwardEnd()
		{
			ButtonNorth_Released(UIInterfaceType.Dialogue);
		}

		/// ------------------------------------------------------------------------
		/// --------------------------- Generic events -----------------------------
		/// ------------------------------------------------------------------------

		/// <summary>
		/// This handles the close button.
		/// </summary>
		protected override void CloseMenu()
		{
			if (UIManager.Instance.ActiveInterfaceType == UIInterfaceType.Pause) return;
			base.CloseMenu();
		}

		/// <summary>
		/// This handles the pause menu button. (PS Option button)
		/// </summary>
		void ClosePauseMenu()
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Pause) return;
			base.CloseMenu();
		}

		/// ------------------------------------------------------------------------
		/// ---------------------------- Option events -----------------------------
		/// ------------------------------------------------------------------------

		void TabSwitch(bool isNext)
		{
			Next(isNext, UIInterfaceType.Option);
		}

		void DefaultOptionMenu()
		{
			ButtonNorth(UIInterfaceType.Option);
		}

		/// ------------------------------------------------------------------------
		/// ------------------------------- Generic --------------------------------
		/// ------------------------------------------------------------------------

		void Next(bool isNext, UIInterfaceType uiInterfaceType)
		{
			if (UIManager.Instance.ActiveInterfaceType != uiInterfaceType) return;
			IControlInput?.Next(isNext);
		}

		void ButtonNorth(UIInterfaceType uiInterfaceType)
		{
			if (UIManager.Instance.ActiveInterfaceType != uiInterfaceType) return;
			IControlInput?.ButtonNorth();
		}

		void ButtonNorth_Released(UIInterfaceType uiInterfaceType)
		{
			if (UIManager.Instance.ActiveInterfaceType != uiInterfaceType) return;
			IControlInput?.ButtonNorth_Released();
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