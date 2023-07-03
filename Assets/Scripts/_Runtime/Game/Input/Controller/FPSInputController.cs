using UnityEngine;

using Personal.Manager;
using Personal.UI.Option;
using Personal.UI;

namespace Personal.InputProcessing
{
	public class FPSInputController : InputControllerBase
	{
		public bool IsJump { get; private set; }
		public bool IsSprint { get; private set; }

		void OnEnable()
		{
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
		}

		void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		void LookInput(Vector2 newLookDirection)
		{
			Look = newLookDirection;
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
			if (!UIManager.Instance.IsWindowStackEmpty) return;
			UIManager.Instance.PauseUI.OpenWindow();
		}

		void OpenInventory()
		{
			UIManager.Instance.InventoryUI.OpenWindow();
		}

		void InventoryNextPrevious(int value)
		{
			if (value == 0) return;
			StageManager.Instance.PlayerController.Inventory.NextItem(value > 0);
		}

		void InventoryIndexSelect(int number)
		{
			StageManager.Instance.PlayerController.Inventory.KeyboardButtonSelect(number - 1);
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