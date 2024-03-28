using System;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Helper;
using Personal.InteractiveObject;
using Personal.GameState;
using Personal.Manager;
using Personal.Item;
using Personal.Save;
using Cysharp.Threading.Tasks;

namespace Personal.Character.Player
{
	public class PlayerInventory : GameInitialize
	{
		[Serializable]
		public class Item
		{
			[SerializeField] Transform pickupableObjectFPS = null;
			[SerializeField] SelfRotate pickupableObjectRotateUI = null;

			public ItemType ItemType { get; private set; }
			public Transform PickupableObjectFPS { get => pickupableObjectFPS; }
			public SelfRotate PickupableObjectRotateUI { get => pickupableObjectRotateUI; }

			Quaternion defaultRotationUI;

			public Item(ItemType itemType, Transform pickupableObjectFPS, SelfRotate pickupableObjectRotateUI)
			{
				ItemType = itemType;
				this.pickupableObjectFPS = pickupableObjectFPS;
				this.pickupableObjectRotateUI = pickupableObjectRotateUI;

				defaultRotationUI = pickupableObjectRotateUI.transform.localRotation;
			}

			public void ResetPickupableObjectUI()
			{
				pickupableObjectRotateUI.transform.localRotation = defaultRotationUI;
				pickupableObjectRotateUI.enabled = false;
			}
		}

		[SerializeField] float autoHideItemDuration = 10f;

		[SerializeField]
		[ReadOnly]
		Item activeObject = null;

		[SerializeField]
		[ReadOnly]
		List<Item> inventoryList = new();

		public Item ActiveObject { get => activeObject; }
		public List<Item> InventoryList { get => inventoryList; }

		public int CurrentActiveIndex { get; private set; } = -1;

		public event Action<Item> OnPickupItemEvent;
		public event Action<Item> OnUseActiveItemEvent;

		Vector3 initialPosition = new Vector3(0, -0.25f, 0);

		CoroutineRun comeIntoViewCR = new CoroutineRun();
		CoroutineRun autoHideItemCR = new CoroutineRun();

		InventoryData inventoryData;

		protected override void Initialize()
		{
			inventoryData = GameStateBehaviour.Instance.SaveObject.PlayerSavedData.InventoryData;
			InitInventoryData().Forget();
		}

		/// <summary>
		/// Use/interact/place item on someone or something.
		/// </summary>
		public void UseActiveItem()
		{
			OnUseActiveItemEvent?.Invoke(activeObject);
			inventoryData.ItemList.Remove(activeObject.ItemType);

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
			// Disable pickupable.
			interactablePickupable.gameObject.SetActive(false);
			if (activeObject != null && activeObject.PickupableObjectFPS) activeObject.PickupableObjectFPS.gameObject.SetActive(false);

			var inventory = AddItemToInventory(interactablePickupable);
			inventoryData.ItemList.Add(interactablePickupable.ItemType);

			activeObject = inventory;
			CurrentActiveIndex = inventoryList.Count - 1;

			// Setup the item transform and show it to the player.
			FPS_SetupItem();
			FPS_ShowItem(true);

			UIManager.Instance.InventoryUI.Init(activeObject.PickupableObjectRotateUI);
			OnPickupItemEvent?.Invoke(activeObject);
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
		/// Show the CurrentActiveIndex object. Used when needs updating the active object.
		/// </summary>
		public void UpdateActiveObject()
		{
			if (CurrentActiveIndex < 0) return;

			var newActiveObject = inventoryList[CurrentActiveIndex];
			if (activeObject != null && activeObject.PickupableObjectFPS != null)
			{
				// Do nothing if it's the same object.
				if (activeObject.PickupableObjectFPS.Equals(newActiveObject.PickupableObjectFPS)) return;

				// Disable the active gameobject.
				activeObject.PickupableObjectFPS?.gameObject.SetActive(false);
			}

			activeObject = newActiveObject;

			FPS_SetupItem();
			FPS_ShowItem(true);
		}

		/// <summary>
		/// Hide the active object. As of now, there are no reasons to activate an object outside of this script.
		/// </summary>
		public void FPS_HideItem() { FPS_ShowItem(false); }

		/// <summary>
		/// Remove all objects from the FPS and UI view.
		/// </summary>
		public void ResetInventory()
		{
			foreach (var inventory in inventoryList)
			{
				PoolManager.Instance.ReturnSpawnedObject(inventory.PickupableObjectFPS.gameObject);
				PoolManager.Instance.ReturnSpawnedObject(inventory.PickupableObjectRotateUI.gameObject);
			}
			inventoryList.Clear();
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
				if (inventory.ItemType != itemType) continue;
				count++;
			}
			return count;
		}

		/// <summary>
		/// Load from inventory data.
		/// </summary>
		/// <returns></returns>
		async UniTask InitInventoryData()
		{
			// Get the item prefab.
			var unitaskList = new List<UniTask<GameObject>>();
			foreach (var itemType in inventoryData.ItemList)
			{
				unitaskList.Add(AddressableHelper.GetResult(itemType.GetStringValue()));
			}

			List<GameObject> goList = new List<GameObject>(await UniTask.WhenAll(unitaskList));

			// Put the spawned objects into inventory.
			foreach (GameObject go in goList)
			{
				var pickupable = go.GetComponentInChildren<InteractablePickupable>();
				activeObject = AddItemToInventory(pickupable);

				FPS_SetupItem();
				UIManager.Instance.InventoryUI.Init(activeObject.PickupableObjectRotateUI);
			}

			if (goList.Count <= 0) return;
			CurrentActiveIndex = inventoryList.Count - 1;
		}

		/// <summary>
		/// Get from pool or instantiate item into inventory.
		/// </summary>
		/// <param name="interactablePickupable"></param>
		/// <returns></returns>
		Item AddItemToInventory(InteractablePickupable interactablePickupable)
		{
			// Try to get from pool.
			GameObject fpsPrefab = PoolManager.Instance.GetSpawnedObject(interactablePickupable.FPSPrefab.name);
			GameObject uiPrefab = PoolManager.Instance.GetSpawnedObject(interactablePickupable.UIPrefab.name);

			// Spawn the fps and ui version and add it to inventory.
			var pickupableFPS = fpsPrefab ? fpsPrefab.transform : Instantiate(interactablePickupable.FPSPrefab);
			var instanceUI = uiPrefab ? uiPrefab.GetComponentInChildren<SelfRotate>() : Instantiate(interactablePickupable.UIPrefab);

			// Update the name without the "clone" attached to them.
			pickupableFPS.name = interactablePickupable.FPSPrefab.name;
			instanceUI.name = interactablePickupable.UIPrefab.name;

			Item inventory = new Item(interactablePickupable.ItemType, pickupableFPS, instanceUI);
			inventoryList.Add(inventory);

			return inventory;
		}

		/// <summary>
		/// Put it near the player's view.
		/// </summary>
		void FPS_SetupItem()
		{
			Transform activeTrans = activeObject.PickupableObjectFPS.transform;
			Transform fpsCameraView = StageManager.Instance.CameraHandler.PlayerCameraView.FpsInventoryView;

			activeTrans.SetParent(fpsCameraView, false);
			activeTrans.localPosition = initialPosition;

			activeTrans.gameObject.SetActive(true);
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
	}
}