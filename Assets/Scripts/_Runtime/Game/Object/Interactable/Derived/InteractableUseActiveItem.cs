﻿using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Item;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class InteractableUseActiveItem : InteractableObject
	{
		[SerializeField] protected ItemType itemTypeCompare = ItemType.Item_1;
		[SerializeField] protected Transform placeAt = null;

		protected override UniTask HandleInteraction()
		{
			HandleUseActiveItem();
			return UniTask.CompletedTask;
		}

		/// <summary>
		/// Check whether it's the correct item type before using it.
		/// </summary>
		void HandleUseActiveItem()
		{
			var pickupable = StageManager.Instance.PlayerController.Inventory.ActiveObject?.PickupableObject;

			if (!pickupable) return;
			if (!itemTypeCompare.HasFlag(pickupable.ItemTypeSet.ItemType)) return;

			pickupable.ParentTrans.GetComponentInChildren<IItem>().PlaceAt(placeAt.position, placeAt);
			StageManager.Instance.PlayerController.Inventory.UseActiveItem(false);
		}
	}
}

