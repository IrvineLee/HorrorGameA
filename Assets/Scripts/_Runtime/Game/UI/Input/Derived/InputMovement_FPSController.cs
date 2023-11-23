using UnityEngine;

using Personal.Manager;

namespace Personal.InputProcessing
{
	public class InputMovement_FPSController : ControlInputBase, IFPSControlInput
	{
		void IFPSControlInput.OpenPauseMenu()
		{
			if (!UIManager.IsWindowStackEmpty) return;
			UIManager.Instance.PauseUI.OpenWindow();
		}

		void IFPSControlInput.OpenInventory()
		{
			if (!UIManager.IsWindowStackEmpty) return;
			UIManager.Instance.InventoryUI.OpenWindow();
		}

		void IFPSControlInput.Next(bool isFlag)
		{
			StageManager.Instance.PlayerController.Inventory.NextItem(isFlag);
		}

		void IFPSControlInput.InventoryIndexSelect(int number)
		{
			StageManager.Instance.PlayerController.Inventory.KeyboardButtonSelect(number);
		}
	}
}