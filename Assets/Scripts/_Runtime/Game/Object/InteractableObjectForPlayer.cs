using System;
using UnityEngine;

using Personal.FSM;
using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.Object
{
	public class InteractableObjectForPlayer : InteractableObject
	{
		public override async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast = default)
		{
			if (interactType == InteractType.Pickupable)
			{
				HandlePickupable();
			}
			else if (interactType == InteractType.UseActiveItem)
			{
				HandleUseActiveItem();
			}
			else if (interactType == InteractType.Event_StateChange)
			{
				await HandleEventStateChange();
				InputManager.Instance.SetToDefaultActionMap();
			}

			doLast?.Invoke();
		}

		/// <summary>
		/// Add item into inventory.
		/// </summary>
		void HandlePickupable()
		{
			StageManager.Instance.PlayerController.Inventory.AddItem(this);

			currentCollider.enabled = false;
			meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			enabled = false;
		}

		/// <summary>
		/// Handle the orderedStateMachine.
		/// </summary>
		/// <returns></returns>
		async UniTask HandleEventStateChange()
		{
			InputManager.Instance.EnableActionMap(actionMapType);

			await orderedStateMachine.Initialize(null, interactionAssign);
		}

		/// <summary>
		/// Check whether it's the correct item type before using it.
		/// </summary>
		void HandleUseActiveItem()
		{
			var activeObject = StageManager.Instance.PlayerController.Inventory.ActiveObject;

			if (activeObject.CurrentItemTypeSet.ItemType != itemTypeCompare) return;

			activeObject.GetComponentInChildren<IItem>().PlaceAt(placeAt.position);
			StageManager.Instance.PlayerController.Inventory.UseActiveItem();
		}
	}
}

