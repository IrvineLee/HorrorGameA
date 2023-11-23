using UnityEngine;

using Personal.Manager;
using Personal.Character.Player;

namespace Personal.InputProcessing
{
	public class InputMovement_FPSController : ControlInputBase, IFPSControlInput
	{
		PlayerController pc;

		protected override void Initialize()
		{
			pc = StageManager.Instance.PlayerController;
		}

		void IFPSControlInput.Sprint(bool isFlag)
		{
			pc.FPSController.Sprint(isFlag);
		}

		void IFPSControlInput.Jump(bool isFlag)
		{
			pc.FPSController.Jump(isFlag);
		}

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
			pc.Inventory.NextItem(isFlag);
		}

		void IFPSControlInput.InventoryIndexSelect(int number)
		{
			pc.Inventory.KeyboardButtonSelect(number);
		}
	}
}