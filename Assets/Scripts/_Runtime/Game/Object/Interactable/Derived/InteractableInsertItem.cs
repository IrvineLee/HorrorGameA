using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Item;
using Personal.InteractiveObject;
using Personal.Character.Player;

namespace Puzzle.EightSlide
{
	public class InteractableInsertItem : InteractableObject
	{
		[Serializable]
		class ItemInfo
		{
			[SerializeField] ItemType itemType = ItemType.Item_1;

			[Tooltip("This is the item that should be enabled in scene upon inserting the object.")]
			[SerializeField] Transform activateObjectTrans = null;

			public ItemType ItemType { get => itemType; }
			public Transform ActivateObjectTrans { get => activateObjectTrans; }

			public ItemInfo(ItemType itemType, Transform activateObjectTrans)
			{
				this.itemType = itemType;
				this.activateObjectTrans = activateObjectTrans;
			}
		}

		[SerializeField] List<ItemInfo> itemInfoList = new List<ItemInfo>();

		PlayerInventory playerInventory;

		protected override void Initialize()
		{
			base.Initialize();

			playerInventory = StageManager.Instance.PlayerController?.Inventory;

			foreach (var itemInfo in itemInfoList)
			{
				itemInfo.ActivateObjectTrans.gameObject.SetActive(false);
			}
		}

		protected override UniTask HandleInteraction()
		{
			var pickupable = playerInventory.ActiveObject?.PickupableObject;
			if (!pickupable) return UniTask.CompletedTask;

			ItemType activeItemType = pickupable.ItemTypeSet.ItemType;
			foreach (var itemInfo in itemInfoList)
			{
				if (itemInfo.ItemType != activeItemType) continue;

				// Use the active item.
				playerInventory.UseActiveItem(true);

				// Enable the active gameobject and remove it from list.
				itemInfo.ActivateObjectTrans.gameObject.SetActive(true);
				itemInfoList.Remove(itemInfo);

				break;
			}

			if (itemInfoList.Count <= 0) gameObject.SetActive(false);
			return UniTask.CompletedTask;
		}
	}
}
