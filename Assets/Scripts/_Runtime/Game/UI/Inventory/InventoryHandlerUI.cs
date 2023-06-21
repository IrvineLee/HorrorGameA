using UnityEngine;

using Personal.InputProcessing;
using Personal.Manager;
using Personal.Item;
using Personal.Object;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.UI
{
	public class InventoryHandlerUI : MenuUIBase
	{
		[SerializeField] ItemInACircle3DUI itemInACircle3DUI = null;

		public override void InitialSetup()
		{
			itemInACircle3DUI.InitialSetup();
		}

		public override void OpenWindow()
		{
			base.OpenWindow();

			InputManager.Instance.EnableActionMap(ActionMapType.UI);
			itemInACircle3DUI.PutObjectsIntoACircle();
		}

		public override void CloseWindow()
		{
			base.CloseWindow();

			StageManager.Instance.PlayerController.Inventory.UpdateActiveObject();
		}

		/// <summary>
		/// Add item to canvas camera for ui selection.
		/// </summary>
		/// <param name="itemTypeStr"></param>
		public void SpawnObject(ItemType itemType, Inventory inventory)
		{
			_ = itemInACircle3DUI.SpawnObject(itemType, inventory);
		}

		/// <summary>
		/// Remove object from ui selection.
		/// </summary>
		/// <param name="interactableObject"></param>
		public void RemoveObject(InteractableObject interactableObject)
		{
			PoolManager.Instance.ReturnSpawnedObject(interactableObject.ParentTrans.gameObject);
		}
	}
}