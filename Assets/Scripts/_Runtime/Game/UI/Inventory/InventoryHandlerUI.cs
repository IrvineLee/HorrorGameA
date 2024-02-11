using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using Personal.Character.Player;

namespace Personal.UI
{
	public class InventoryHandlerUI : UIHandlerBase
	{
		[SerializeField] ItemInACircle3DUI itemInACircle3DUI = null;

		PlayerController pc;

		public override void InitialSetup()
		{
			base.InitialSetup();

			pc = StageManager.Instance.PlayerController;
			itemInACircle3DUI.InitialSetup();
		}

		public override void OpenWindow()
		{
			base.OpenWindow();

			itemInACircle3DUI.PutObjectsIntoACircle();
		}

		public override async UniTask CloseWindow(bool isInstant = false)
		{
			await base.CloseWindow(isInstant);

			pc.Inventory.UpdateActiveObject();
		}

		/// <summary>
		/// Add item to canvas camera for ui selection.
		/// </summary>
		/// <param name="itemTypeStr"></param>
		public void Init(SelfRotate inventoryUI)
		{
			itemInACircle3DUI.Init(inventoryUI);
		}
	}
}