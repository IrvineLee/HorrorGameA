using System;
using UnityEngine;

using Personal.InputProcessing;
using Personal.Manager;
using Personal.Item;
using Personal.Object;
using Personal.GameState;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.UI
{
	public class InventoryHandlerUI : GameInitialize, IWindowHandler
	{
		[SerializeField] ItemInACircle3DUI itemInACircle3DUI = null;

		public IWindowHandler IWindowHandler { get => this; }

		public event Action<bool> OnMenuOpened;

		public void InitialSetup()
		{
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
			SetWindowEnable(true);
			itemInACircle3DUI.Setup();
			InputManager.Instance.EnableActionMap(ActionMapType.UI);
		}

		void IWindowHandler.CloseWindow()
		{
			SetWindowEnable(false);
			StageManager.Instance.PlayerController.Inventory.UpdateActiveObject();
			InputManager.Instance.SetToDefaultActionMap();
		}

		void SetWindowEnable(bool isFlag)
		{
			OnMenuOpened?.Invoke(isFlag);
			gameObject.SetActive(isFlag);
		}
	}
}