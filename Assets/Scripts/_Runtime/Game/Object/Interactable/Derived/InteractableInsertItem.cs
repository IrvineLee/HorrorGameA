using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Item;
using Personal.Character.Player;
using Personal.Save;
using Helper;

namespace Personal.InteractiveObject
{
	public class InteractableInsertItem : InteractableObject, IDataPersistence
	{
		[Serializable]
		class ItemInfo
		{
			[SerializeField] ItemType itemType = ItemType._10000_Item_1;

			[Tooltip("This is the item that should be enabled in scene upon inserting the object.")]
			[SerializeField] Transform activateObjectTrans = null;

			public ItemType ItemType { get => itemType; }
			public Transform ActivateObjectTrans { get => activateObjectTrans; }
			public bool IsInteractionEnded { get; private set; }

			public ItemInfo(ItemType itemType, Transform activateObjectTrans)
			{
				this.itemType = itemType;
				this.activateObjectTrans = activateObjectTrans;
			}

			public void SetInteractionEnded() { IsInteractionEnded = true; }
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

		protected override UniTask HandleInteraction()
		{
			var activeObject = playerInventory.ActiveObject;
			if (activeObject == null) return UniTask.CompletedTask;

			ItemType activeItemType = activeObject.ItemType;
			foreach (var itemInfo in itemInfoList)
			{
				if (itemInfo.ItemType != activeItemType) continue;

				// Use the active item.
				playerInventory.UseActiveItem();

				// Enable the active gameobject and remove it from list.
				itemInfo.ActivateObjectTrans.gameObject.SetActive(true);
				itemInfo.SetInteractionEnded();

				break;
			}

			foreach (var itemInfo in itemInfoList)
			{
				if (!itemInfo.IsInteractionEnded) return UniTask.CompletedTask;
			}

			isInteractionEnded = true;
			gameObject.SetActive(false);

			return UniTask.CompletedTask;
		}

		void IDataPersistence.SaveData(SaveObject data)
		{
			List<bool> clearItemList = itemInfoList.Select((x) => x.IsInteractionEnded).ToList();
			Completed completed = new Completed(clearItemList);

			if (completed.IsCompletedList.Contains(true))
			{
				data.InsertItemDictionary.AddOrUpdateValue(id, completed);
			}

			if (!isInteractionEnded) return;
			data.PickupableDictionary.AddOrUpdateValue(id, isInteractionEnded);
		}

		void IDataPersistence.LoadData(SaveObject data)
		{
			if (!data.PickupableDictionary.TryGetValue(id, out bool value)) return;
			gameObject.SetActive(!value);

			if (!data.InsertItemDictionary.TryGetValue(id, out valueCompleted)) return;

			for (int i = 0; i < valueCompleted.IsCompletedList.Count; i++)
			{
				if (!valueCompleted.IsCompletedList[i]) continue;

				ItemInfo currentItem = itemInfoList[i];

				currentItem.ActivateObjectTrans.gameObject.SetActive(true);
				currentItem.SetInteractionEnded();
			}
		}
	}
}
