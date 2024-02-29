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

			inputReaderDefinition.OnTopTabSwitchEvent += TopTabSwitch;
			inputReaderDefinition.OnBottomTabSwitchEvent += BottomTabSwitch;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CancelInput;
			inputReaderDefinition.OnCancelEvent += CloseMenu;

			inputReaderDefinition.OnMenuUIDefaultPressedEvent += DefaultOptionMenu;

			inputReaderDefinition.OnMenuUICancelledEvent += ClosePauseMenu;

			inputReaderDefinition.OnDialogueUIFastForwardPressed_StartEvent += FastForwardStart;
			inputReaderDefinition.OnDialogueUIFastForwardPressed_EndEvent += FastForwardEnd;
		}

		void MoveInput(Vector2 direction)
		{
			Move = direction;
			(ControlInputBase.ActiveControlInput as IUIControlInput)?.Move(direction);
		}

		void LookInput(Vector2 direction)
		{
			Look = direction;
			(ControlInputBase.ActiveControlInput as IUIControlInput)?.Look(direction);
		}

		void InteractInput()
		{
			IsInteract = true;
			(ControlInputBase.ActiveControlInput as IUIControlInput)?.Submit();
		}

		void CancelInput()
		{
			IsCancel = true;
			(ControlInputBase.ActiveControlInput as IUIControlInput)?.Cancel();
		}

		/// ------------------------------------------------------------------------
		/// --------------------------- Dialogue events ----------------------------
		/// ------------------------------------------------------------------------

		void FastForwardStart()
		{
			CheckInterfaceTypeAndAct(UIInterfaceType.Dialogue, () => ((IUIControlInput)ControlInputBase.ActiveControlInput)?.FastForward());
		}

		void FastForwardEnd()
		{
			CheckInterfaceTypeAndAct(UIInterfaceType.Dialogue, () => ((IUIControlInput)ControlInputBase.ActiveControlInput)?.FastForwardReleased());
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
			if (UIManager.Instance.ActiveInterfaceType == UIInterfaceType.ReadFile) return;

			base.CloseMenu();
		}

		/// <summary>
		/// This handles the pause menu button. (PS Option button)
		/// </summary>
		void ClosePauseMenu()
		{
			CheckInterfaceTypeAndAct(UIInterfaceType.Pause, base.CloseMenu);
		}

		/// ------------------------------------------------------------------------
		/// ---------------------------- Option events -----------------------------
		/// ------------------------------------------------------------------------

		/// <summary>
		/// This handles the top tabs in the option menu.
		/// </summary>
		void TopTabSwitch(bool isNext)
		{
			CheckInterfaceTypeAndAct(UIInterfaceType.Option, () => ((IUIControlInput)ControlInputBase.ActiveControlInput)?.NextShoulder(isNext));
		}

		/// <summary>
		/// This handles the bottom tabs in the option menu, if applicable.
		/// </summary>
		void BottomTabSwitch(bool isNext)
		{
			CheckInterfaceTypeAndAct(UIInterfaceType.Option, () => ((IUIControlInput)ControlInputBase.ActiveControlInput)?.NextTrigger(isNext));
		}

		void DefaultOptionMenu()
		{
			CheckInterfaceTypeAndAct(UIInterfaceType.Option, () => ((IUIControlInput)ControlInputBase.ActiveControlInput)?.Default());
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			inputReaderDefinition.OnMoveEvent -= MoveInput;
			inputReaderDefinition.OnLookEvent -= LookInput;

			inputReaderDefinition.OnTopTabSwitchEvent -= TopTabSwitch;
			inputReaderDefinition.OnBottomTabSwitchEvent -= BottomTabSwitch;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CloseMenu;

			inputReaderDefinition.OnMenuUIDefaultPressedEvent -= DefaultOptionMenu;

			inputReaderDefinition.OnMenuUICancelledEvent -= ClosePauseMenu;

			inputReaderDefinition.OnDialogueUIFastForwardPressed_StartEvent -= FastForwardStart;
			inputReaderDefinition.OnDialogueUIFastForwardPressed_EndEvent -= FastForwardEnd;
		}
	}
}