using System;
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
			UIManager.Instance.WindowStack.Push(itemInACircle3DUI);

			PauseEventBegin(true);
			itemInACircle3DUI.Setup();
			InputManager.Instance.EnableActionMap(ActionMapType.UI);
		}

		public override void CloseWindow()
		{
			base.CloseWindow();

			PauseEventBegin(false);
			StageManager.Instance.PlayerController.Inventory.UpdateActiveObject();
			InputManager.Instance.SetToDefaultActionMap();
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