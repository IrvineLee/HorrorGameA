using System;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Helper;
using Personal.InteractiveObject;
using Personal.GameState;
using Personal.Manager;
using Personal.Item;
using Personal.Achievement;

namespace Personal.Character.Player
{
	public class PlayerInventory : GameInitialize
	{
		[Serializable]
		public class Inventory
		{
			// This is the real interactable object.
			[SerializeField] InteractablePickupable pickupableObject = null;

			[SerializeField] Transform pickupableObjectFPS = null;
			[SerializeField] Transform pickupableObjectUI = null;

			public InteractablePickupable PickupableObject { get => pickupableObject; }
			public Transform PickupableObjectFPS { get => pickupableObjectFPS; }
			public Transform PickupableObjectUI { get => pickupableObjectUI; }
			public SelfRotate PO_UI_SelfRotate { get; private set; }

			public Inventory(InteractablePickupable pickupableObject, Transform pickupableObjectFPS, Transform pickupableObjectUI)
			{
				this.pickupableObject = pickupableObject;
				this.pickupableObjectFPS = pickupableObjectFPS;
				this.pickupableObjectUI = pickupableObjectUI;

				PO_UI_SelfRotate = pickupableObjectUI.GetComponentInChildren<SelfRotate>();
				pickupableObject.gameObject.SetActive(false);
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
		/// <param name="isReturnToPool">If you are interacting with the object somewhere else, put it to false. Otherwise it return to the pool.</param>
		public void UseActiveItem(bool isReturnToPool = true)
		{
			if (isReturnToPool)
			{
				PoolManager.Instance.ReturnSpawnedObject(activeObject.PickupableObject.gameObject);
			}

			PoolManager.Instance.ReturnSpawnedObject(activeObject.PickupableObjectFPS.gameObject);
			PoolManager.Instance.ReturnSpawnedObject(activeObject.PickupableObjectUI.gameObject);

			UpdateGlossaryAndAchievement(activeObject.PickupableObject);

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
			// Disable active pickupable.
			if (activeObject.PickupableObject)
			{
				activeObject.PickupableObject.gameObject.SetActive(false);
				activeObject.PickupableObjectFPS.gameObject.SetActive(false);
			}

			var instanceFPS = Instantiate(interactablePickupable.FPSPrefab);
			var instanceUI = Instantiate(interactablePickupable.UIPrefab);

			Inventory inventory = new Inventory(interactablePickupable, instanceFPS, instanceUI);
			inventoryList.Add(inventory);

			activeObject = inventory;
			CurrentActiveIndex = inventoryList.Count - 1;

			// Add item to inventory ui.
			UIManager.Instance.InventoryUI.Init(instanceUI);

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
				activeObject.PickupableObjectFPS?.gameObject.SetActive(false);
			}

			activeObject = newActiveObject;
			HoldItemInHand();
		}

		/// <summary>
		/// Hide the active object. As of now, there are no reasons to activate an object outside of this script.
		/// </summary>
		public void FPS_HideItem() { FPS_ShowItem(false); }

		/// <summary>
		/// Remove the object from the inventory.
		/// </summary>
		public void ResetInventoryUI()
		{
			foreach (var inventory in inventoryList)
			{
				PoolManager.Instance.ReturnSpawnedObject(inventory.PickupableObjectFPS.gameObject);
				PoolManager.Instance.ReturnSpawnedObject(inventory.PickupableObjectUI.gameObject);
			}
		}

		/// <summary>
		/// Get the item count.
		/// </summary>
		/// <param name="itemTypeSet"></param>
		/// <returns></returns>
		public int GetItemCount(ItemType itemType)
		{
			int count = 0;
			foreach (var inventory in inventoryList)
			{
				if (inventory.PickupableObject.ItemType != itemType) continue;
				count++;
			}
			return count;
		}

		/// <summary>
		/// Put it near the player's view.
		/// </summary>
		void HoldItemInHand()
		{
			Transform activeTrans = activeObject.PickupableObjectFPS.transform;
			Transform fpsCameraView = StageManager.Instance.CameraHandler.PlayerCameraView.FpsInventoryView;

			Quaternion rotation = activeTrans.localRotation;
			Vector3 scale = activeTrans.localScale;

			activeTrans.SetParent(fpsCameraView);
			activeTrans.localPosition = initialPosition;
			activeTrans.localRotation = rotation;
			activeTrans.localScale = scale;

			activeTrans.gameObject.SetActive(true);
			FPS_ShowItem(true);
		}

		/// <summary>
		/// Show or hide the active object.
		/// </summary>
		/// <param name="isFlag"></param>
		void FPS_ShowItem(bool isFlag)
		{
			if (activeObject == null || !activeObject.PickupableObjectFPS) return;

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

		/// <summary>
		/// Animation of it moving to the hand/away from hand.
		/// </summary>
		/// <param name="toPosition"></param>
		void AnimateActiveItem(Vector3 toPosition)
		{
			Transform activeTrans = activeObject.PickupableObjectFPS.transform;

			comeIntoViewCR?.StopCoroutine();
			comeIntoViewCR = CoroutineHelper.LerpFromTo(activeTrans, activeTrans.localPosition, toPosition, 0.3f);
		}


		/// <summary>
		/// Update both the glossary and achievement.
		/// </summary>
		/// <param name="pickupable"></param>
		void UpdateGlossaryAndAchievement(InteractablePickupable pickupable)
		{
			GlossaryManager.Instance.AddUsedType(pickupable.ItemType);

			var achievementTypeSet = activeObject.PickupableObject.GetComponentInChildren<AchievementTypeSet>();
			if (achievementTypeSet) AchievementManager.Instance.UpdateData(achievementTypeSet.AchievementType);
		}
	}
}