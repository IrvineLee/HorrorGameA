using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using Personal.Item;
using Personal.Character.Player;
using Personal.Save;

namespace Personal.InteractiveObject
{
	public class InteractableUseMultipleItem : InteractableObject, IDataPersistence
	{
		[Serializable]
		class ItemInfo
		{
			[SerializeField] ItemType itemType = ItemType._None;

			[Tooltip("This is the item that should be enabled in scene upon inserting the object.")]
			[SerializeField] Transform activateObjectTrans = null;

			public ItemType ItemType { get => itemType; }
			public Transform ActivateObjectTrans { get => activateObjectTrans; }
			public bool IsCompleted { get; private set; }

			public void SetCompleted() { IsCompleted = true; }
		}

		[SerializeField] List<ItemInfo> itemInfoList = new List<ItemInfo>();

		PlayerInventory playerInventory;
		Completed valueCompleted;

		protected override void Initialize()
		{
			base.Initialize();
			playerInventory = StageManager.Instance.PlayerController?.Inventory;

			// Use the loaded data if have.
			if (valueCompleted != null) return;

			foreach (var itemInfo in itemInfoList)
			{
				if (itemInfo.ActivateObjectTrans) itemInfo.ActivateObjectTrans.gameObject.SetActive(false);
			}
		}

		protected override async UniTask HandleInteraction()
		{
			var activeObject = playerInventory.ActiveObject;
			if (activeObject == null) return;

			ItemType activeItemType = activeObject.ItemType;
			foreach (var itemInfo in itemInfoList)
			{
				if (itemInfo.ItemType != activeItemType) continue;

				// Use the active item.
				playerInventory.UseActiveItem();

				// Enable the active gameobject and remove it from list.
				itemInfo.ActivateObjectTrans.gameObject.SetActive(true);
				itemInfo.SetCompleted();

				break;
			}

			foreach (var itemInfo in itemInfoList)
			{
				if (!itemInfo.IsCompleted) return;
			}

			gameObject.SetActive(false);
			await base.HandleInteraction();
		}

		void IDataPersistence.SaveData(SaveObject data)
		{
			List<bool> clearItemList = itemInfoList.Select((x) => x.IsCompleted).ToList();
			Completed completed = new Completed(clearItemList);

			if (completed.IsCompletedList.Contains(true))
			{
				data.SceneObjectSavedData.InsertItemDictionary.AddOrUpdateValue(guid, completed);
			}

			data.SceneObjectSavedData.PickupableDictionary.AddOrUpdateValue(guid, interactableState);
		}

		void IDataPersistence.LoadData(SaveObject data)
		{
			if (!data.SceneObjectSavedData.PickupableDictionary.TryGetValue(guid, out interactableState)) return;
			gameObject.SetActive(interactableState != InteractableState.EndNonInteractable);

			if (!data.SceneObjectSavedData.InsertItemDictionary.TryGetValue(guid, out valueCompleted)) return;

			for (int i = 0; i < valueCompleted.IsCompletedList.Count; i++)
			{
				if (!valueCompleted.IsCompletedList[i]) continue;

				ItemInfo currentItem = itemInfoList[i];

				currentItem.ActivateObjectTrans.gameObject.SetActive(true);
				currentItem.SetCompleted();
			}
		}
	}
}
