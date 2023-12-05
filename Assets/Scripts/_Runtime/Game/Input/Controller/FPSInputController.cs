using UnityEngine;

using Personal.Manager;

namespace Personal.InputProcessing
{
	public class FPSInputController : InputControllerBase
	{
		InputMovement_FPSController inputMovement;

		protected override void OnEnable()
		{
			base.OnEnable();

			inputReaderDefinition.OnMoveEvent += MoveInput;
			inputReaderDefinition.OnLookEvent += LookInput;

			inputReaderDefinition.OnSprintEvent += SprintInput;
			inputReaderDefinition.OnJumpEvent += JumpInput;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CancelInput;

			inputReaderDefinition.OnMenuUIPressedEvent += OpenPauseMenu;
			inputReaderDefinition.OnInventoryUIPressedEvent += OpenInventory;

			inputReaderDefinition.OnInventoryNextPreviousEvent += InventoryNextPrevious;
			inputReaderDefinition.OnInventoryIndexSelectEvent += InventoryIndexSelect;

			// This is the default input movement when in the main scene.
			inputMovement = StageManager.Instance?.PlayerController?.InputMovement_FPSController;
		}

		void MoveInput(Vector2 direction)
		{
			Move = direction;
		}

		void LookInput(Vector2 direction)
		{
			Look = direction;
		}

		void InteractInput()
		{
			IsInteract = true;
		}

		void CancelInput()
		{
			IsCancel = true;
		}

		void JumpInput(bool isFlag)
		{
			(inputMovement as IFPSControlInput)?.Jump(isFlag);
		}

		void SprintInput(bool isFlag)
		{
			(inputMovement as IFPSControlInput)?.Sprint(isFlag);
		}

		void OpenPauseMenu()
		{
			(inputMovement as IFPSControlInput)?.OpenPauseMenu();
		}

		void OpenInventory()
		{
			(inputMovement as IFPSControlInput)?.OpenInventory();
		}

		void InventoryNextPrevious(int value)
		{
			if (value == 0) return;
			(inputMovement as IFPSControlInput)?.Next(value > 0);
		}

		void InventoryIndexSelect(int number)
		{
			(inputMovement as IFPSControlInput)?.InventoryIndexSelect(number - 1);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			inputReaderDefinition.OnMoveEvent -= MoveInput;
			inputReaderDefinition.OnLookEvent -= LookInput;

			inputReaderDefinition.OnSprintEvent -= SprintInput;
			inputReaderDefinition.OnJumpEvent -= JumpInput;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CancelInput;

			inputReaderDefinition.OnMenuUIPressedEvent -= OpenPauseMenu;
			inputReaderDefinition.OnInventoryUIPressedEvent -= OpenInventory;

			inputReaderDefinition.OnInventoryNextPreviousEvent -= InventoryNextPrevious;
			inputReaderDefinition.OnInventoryIndexSelectEvent -= InventoryIndexSelect;
		}
	}
}