﻿using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Item;
using Personal.Manager;

namespace Personal.Object
{
	public class InteractableUseActiveItem : InteractableObject
	{
		[SerializeField] protected ItemType itemTypeCompare = ItemType.Item_1;
		[SerializeField] protected Transform placeAt = null;

		protected override async UniTask HandleInteraction(ActorStateMachine actorStateMachine)
		{
			await base.HandleInteraction(actorStateMachine);
			HandleUseActiveItem();
		}

		/// <summary>
		/// Check whether it's the correct item type before using it.
		/// </summary>
		void HandleUseActiveItem()
		{
			var activeObject = StageManager.Instance.PlayerController.Inventory.ActiveObject;

			if (!activeObject) return;
			if (!itemTypeCompare.HasFlag(activeObject.ItemTypeSet.ItemType)) return;

			activeObject.ParentTrans.GetComponentInChildren<IItem>().PlaceAt(placeAt.position, placeAt);
			StageManager.Instance.PlayerController.Inventory.UseActiveItem();
		}
	}
}
