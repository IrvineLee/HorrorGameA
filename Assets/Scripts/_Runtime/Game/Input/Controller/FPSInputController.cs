using UnityEngine;

using Personal.Manager;

namespace Personal.InputProcessing
{
	public class FPSInputController : InputControllerBase
	{
		public bool IsJump { get; private set; }
		public bool IsSprint { get; private set; }

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
			controlInput = StageManager.Instance?.PlayerController?.InputMovement_FPSController;
		}

		void MoveInput(Vector2 direction)
		{
			Move = direction;
		}

		void LookInput(Vector2 direction)
		{
			Look = direction;
		}

		void JumpInput(bool isFlag)
		{
			IsJump = isFlag;
		}

		void SprintInput(bool isFlag)
		{
			IsSprint = isFlag;
		}

		void InteractInput()
		{
			IsInteract = true;
		}

		void CancelInput()
		{
			IsCancel = true;
		}

		void OpenPauseMenu()
		{
			(controlInput as IFPSControlInput)?.OpenPauseMenu();
		}

		void OpenInventory()
		{
			(controlInput as IFPSControlInput)?.OpenInventory();
		}

		void InventoryNextPrevious(int value)
		{
			if (value == 0) return;
			(controlInput as IFPSControlInput)?.Next(value > 0);
		}

		void InventoryIndexSelect(int number)
		{
			(controlInput as IFPSControlInput)?.InventoryIndexSelect(number - 1);
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