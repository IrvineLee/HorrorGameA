using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.Object;
using Personal.GameState;
using Personal.Manager;
using Personal.InputProcessing;
using Personal.Item;
using Helper;
using static Personal.Definition.InputReaderDefinition;

namespace Personal.Character.Player
{
	public class PlayerInventory : GameInitialize
	{
		[Serializable]
		public class Inventory
		{
			// This is the real interactable object.
			[SerializeField] InteractableObject interactableObject = null;

			// This is the interactable object portrayed in the inventory ui.
			[SerializeField] InteractableObject interactableObjectUI = null;

			public InteractableObject InteractableObject { get => interactableObject; }
			public InteractableObject InteractableObjectUI { get => interactableObjectUI; }

			public Inventory(InteractableObject interactableObject)
			{
				this.interactableObject = interactableObject;
			}

			public void SetInteractableObjectUI(InteractableObject interactableObjectUI)
			{
				this.interactableObjectUI = interactableObjectUI;
			}
		}

		[SerializeField] Transform FPSHoldItemInHandView = null;
		[SerializeField] float autoHideItemDuration = 10f;

		[SerializeField]
		[ReadOnly]
		InteractableObject activeObject = null;

		[SerializeField]
		[ReadOnly]
		List<Inventory> inventoryList = new();

		public InteractableObject ActiveObject { get => activeObject; }
		public List<Inventory> InventoryList { get => inventoryList; }

		public int CurrentActiveIndex { get; private set; } = -1;

		Vector3 initialPosition = new Vector3(0, -0.25f, 0);
		Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f);

		CoroutineRun comeIntoViewCR = new CoroutineRun();
		CoroutineRun autoHideItemCR = new CoroutineRun();

		InputControllerInfo inputControllerInfo;

		protected async override UniTask Awake()
		{
			await base.Awake();

			inputControllerInfo = InputManager.Instance.GetInputControllerInfo(ActionMapType.Player);
			inputControllerInfo.OnEnableEvent += FPS_ShowItem;
		}

		/// <summary>
		/// Use/interact/place item on someone or something.
		/// </summary>
		public void UseActiveItem()
		{
			activeObject = null;

			// Remove the item from the inventory and the ui view.
			Inventory inventory = inventoryList[CurrentActiveIndex];
			UIManager.Instance.InventoryUI.RemoveObject(inventory.InteractableObjectUI);
			inventoryList.Remove(inventory);

			if (inventoryList.Count <= 0)
			{
				CurrentActiveIndex = -1;
				return;
			}

			// Move the index down 1.
			CurrentActiveIndex = (--CurrentActiveIndex).WithinCount(inventoryList.Count);
		}

		/// <summary>
		/// Add item to inventory.
		/// </summary>
		/// <param name="interactableObject"></param>
		public void AddItem(InteractableObject interactableObject)
		{
			activeObject?.gameObject.SetActive(false);
			activeObject = interactableObject;

			Inventory inventory = new Inventory(interactableObject);
			inventoryList.Add(inventory);
			CurrentActiveIndex = inventoryList.Count - 1;

			// Add item to inventory ui.
			ItemType itemType = interactableObject.ItemTypeSet.ItemType;
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
			CurrentActiveIndex = CurrentActiveIndex.WithinCount(inventoryList.Count);

			if (CurrentActiveIndex >= inventoryList.Count - 1) return;

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

			// Do nothing if it's the same object.
			var newActiveObject = inventoryList[CurrentActiveIndex];
			if (activeObject == newActiveObject.InteractableObject) return;

			// Set to new active gameobject.
			activeObject?.gameObject.SetActive(false);
			activeObject = newActiveObject.InteractableObject;

			HoldItemInHand();
		}

		/// <summary>
		/// Put it near the player's view.
		/// </summary>
		void HoldItemInHand()
		{
			Transform activeTrans = activeObject.transform;

			activeTrans.SetParent(FPSHoldItemInHandView);
			activeTrans.localPosition = initialPosition;
			activeTrans.localRotation = Quaternion.identity;
			activeTrans.localScale = initialScale;

			activeObject.gameObject.SetActive(true);
			FPS_ShowItem(true);
		}

		/// <summary>
		/// Show or hide the active object.
		/// </summary>
		/// <param name="isFlag"></param>
		void FPS_ShowItem(bool isFlag)
		{
			if (!activeObject) return;

			if (isFlag)
			{
				AnimateActiveItem(Vector3.zero);

				autoHideItemCR?.StopCoroutine();
				autoHideItemCR = CoroutineHelper.WaitFor(autoHideItemDuration, () => FPS_ShowItem(false));
			}
			else
			{
				AnimateActiveItem(initialPosition);
			}
		}

		/// <summary>
		/// Animation of it moving to the hand/away from hand.
		/// </summary>
		/// <param name="toPosition"></param>
		void AnimateActiveItem(Vector3 toPosition)
		{
			Transform activeTrans = activeObject.transform;

			comeIntoViewCR?.StopCoroutine();
			comeIntoViewCR = CoroutineHelper.LerpFromTo(activeTrans, activeTrans.localPosition, toPosition, 0.3f);
		}

		void OnApplicationQuit()
		{
			inputControllerInfo.OnEnableEvent -= FPS_ShowItem;
		}
	}
}