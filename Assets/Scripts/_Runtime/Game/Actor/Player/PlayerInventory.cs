using System;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Personal.InteractiveObject;
using Personal.GameState;
using Personal.Manager;
using Personal.Item;
using Helper;

namespace Personal.Character.Player
{
	public class PlayerInventory : GameInitialize
	{
		[Serializable]
		public class Inventory
		{
			// This is the real interactable object.
			[SerializeField] InteractablePickupable pickupableObject = null;

			// This is the interactable object portrayed in the inventory ui.
			[SerializeField] InteractablePickupable pickupableObjectUI = null;

			public InteractablePickupable PickupableObject { get => pickupableObject; }
			public InteractablePickupable PickupableObjectUI { get => pickupableObjectUI; }

			public void SetInteractableObject(InteractablePickupable pickupableObject)
			{
				this.pickupableObject = pickupableObject;
			}

			public void SetInteractableObjectUI(InteractablePickupable pickupableObjectUI)
			{
				this.pickupableObjectUI = pickupableObjectUI;
			}
		}

		[SerializeField] float autoHideItemDuration = 10f;

		[SerializeField]
		[ReadOnly]
		Inventory activeObject = null;

		[SerializeField]
		[ReadOnly]
		List<Inventory> inventoryList = new();

		public Inventory ActiveObject { get => activeObject; }
		public List<Inventory> InventoryList { get => inventoryList; }

		public int CurrentActiveIndex { get; private set; } = -1;

		Vector3 initialPosition = new Vector3(0, -0.25f, 0);

		CoroutineRun comeIntoViewCR = new CoroutineRun();
		CoroutineRun autoHideItemCR = new CoroutineRun();

		/// <summary>
		/// Use/interact/place item on someone or something.
		/// </summary>
		/// <param name="isDestroy">If you are interacting with the object somewhere else, put it to false. Otherwise it return to the pool.</param>
		public void UseActiveItem(bool isDestroy = true)
		{
			if (isDestroy)
			{
				PoolManager.Instance.ReturnSpawnedObject(activeObject.PickupableObject.gameObject);
				PoolManager.Instance.ReturnSpawnedObject(activeObject.PickupableObjectUI.ParentTrans.gameObject);
			}

			// Remove the item from the inventory and the ui view.
			inventoryList.Remove(activeObject);
			activeObject = null;

			if (inventoryList.Count <= 0)
			{
				CurrentActiveIndex = -1;
				return;
			}

			// Move the index down 1.
			CurrentActiveIndex = (--CurrentActiveIndex).WithinCountLoopOver(inventoryList.Count);
		}

		/// <summary>
		/// Add item to inventory.
		/// </summary>
		/// <param name="interactablePickupable"></param>
		public void AddItem(InteractablePickupable interactablePickupable)
		{
			activeObject?.PickupableObject?.gameObject.SetActive(false);

			Inventory inventory = new Inventory();
			inventory.SetInteractableObject(interactablePickupable);
			inventoryList.Add(inventory);

			activeObject = inventory;
			CurrentActiveIndex = inventoryList.Count - 1;

			// Add item to inventory ui.
			ItemType itemType = interactablePickupable.ItemTypeSet.ItemType;
			UIManager.Instance.InventoryUI.SpawnObject(itemType, inventory);

			HoldItemInHand();
		}

		/// <summary>
		/// Switch between items with mouse wheel or left/right bumpers.
		/// </summary>
		/// <param name="isNext"></param>
		public void NextItem(bool isNext)
		{
			if (inventoryList.Count == 0) return;

			// Scroll throught the list.
			CurrentActiveIndex = isNext ? CurrentActiveIndex + 1 : CurrentActiveIndex - 1;
			CurrentActiveIndex = CurrentActiveIndex.WithinCountLoopOver(inventoryList.Count);

			if (CurrentActiveIndex > inventoryList.Count - 1) return;

			// You only want to update the active object after exiting the inventory menu.
			if (UIManager.Instance.ActiveInterfaceType == UI.UIInterfaceType.Inventory) return;

			UpdateActiveObject();
		}

		/// <summary>
		/// This will only apply to keyboard controls. Number 1~9 keys to select items from the list.
		/// </summary>
		/// <param name="index"></param>
		public void KeyboardButtonSelect(int index)
		{
			if (index < 0 || index > inventoryList.Count - 1) return;

			CurrentActiveIndex = index;
			UpdateActiveObject();
		}

		/// <summary>
		/// Update the active object. Used when needs updating the active object.
		/// </summary>
		public void UpdateActiveObject()
		{
			if (CurrentActiveIndex < 0) return;

			var newActiveObject = inventoryList[CurrentActiveIndex];
			if (activeObject != null)
			{
				// Do nothing if it's the same object.
				if (activeObject.PickupableObject.Equals(newActiveObject.PickupableObject)) return;

				// Disable the active gameobject.
				activeObject.PickupableObject?.gameObject.SetActive(false);
			}

			activeObject = newActiveObject;
			HoldItemInHand();
		}

		/// <summary>
		/// Show or hide the active object.
		/// </summary>
		/// <param name="isFlag"></param>
		public void FPS_ShowItem(bool isFlag)
		{
			if (activeObject == null || !activeObject.PickupableObject) return;

			autoHideItemCR?.StopCoroutine();
			if (isFlag)
			{
				AnimateActiveItem(Vector3.zero);
				autoHideItemCR = CoroutineHelper.WaitFor(autoHideItemDuration, () => FPS_ShowItem(false));

				return;
			}

			AnimateActiveItem(initialPosition);
			activeObject = null;
		}

		public void ResetInventoryUI()
		{
			// Remove the object from the inventory UI.
			foreach (var inventory in inventoryList)
			{
				PoolManager.Instance.ReturnSpawnedObject(inventory.PickupableObject.ParentTrans.gameObject);
				PoolManager.Instance.ReturnSpawnedObject(inventory.PickupableObjectUI.ParentTrans.gameObject);
			}
		}

		/// <summary>
		/// Put it near the player's view.
		/// </summary>
		void HoldItemInHand()
		{
			var pickupable = activeObject.PickupableObject;
			Transform activeTrans = pickupable.transform;
			Transform fpsCameraView = StageManager.Instance.CameraHandler.PlayerCameraView.FpsInventoryView;

			activeTrans.SetParent(fpsCameraView);
			activeTrans.localPosition = initialPosition;
			activeTrans.localRotation = Quaternion.Euler(pickupable.FPSRotation);
			activeTrans.localScale = pickupable.FPSScale;

			pickupable.gameObject.SetActive(true);
			FPS_ShowItem(true);
		}

		/// <summary>
		/// Animation of it moving to the hand/away from hand.
		/// </summary>
		/// <param name="toPosition"></param>
		void AnimateActiveItem(Vector3 toPosition)
		{
			Transform activeTrans = activeObject.PickupableObject.transform;

			comeIntoViewCR?.StopCoroutine();
			comeIntoViewCR = CoroutineHelper.LerpFromTo(activeTrans, activeTrans.localPosition, toPosition, 0.3f);
		}
	}
}