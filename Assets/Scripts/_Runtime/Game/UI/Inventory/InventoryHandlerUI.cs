using System;
using UnityEngine;

using Personal.InputProcessing;
using Personal.Manager;
using Personal.Item;
using Personal.Object;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.UI
{
	public class InventoryHandlerUI : MenuUIBase, IWindowHandler
	{
		[SerializeField] ItemInACircle3DUI itemInACircle3DUI = null;

		public override void InitialSetup()
		{
			IWindowHandler = this;
			itemInACircle3DUI.InitialSetup();
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

		void IWindowHandler.OpenWindow()
		{
			UIManager.Instance.WindowStack.Push(itemInACircle3DUI);

			SetupMenu(true);
			itemInACircle3DUI.Setup();
			InputManager.Instance.EnableActionMap(ActionMapType.UI);
		}

		void IWindowHandler.CloseWindow()
		{
			UIManager.Instance.WindowStack.Pop();

			SetupMenu(false);
			StageManager.Instance.PlayerController.Inventory.UpdateActiveObject();
			InputManager.Instance.SetToDefaultActionMap();
		}

		protected override void SetupMenu(bool isFlag)
		{
			base.SetupMenu(isFlag);
			gameObject.SetActive(isFlag);
		}
	}
}