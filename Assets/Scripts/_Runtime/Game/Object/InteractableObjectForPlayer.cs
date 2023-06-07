using System;

using Personal.FSM;
using Personal.Manager;
using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;

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
				var ifsmHandler = stateMachineBase.GetComponentInChildren<IFSMHandler>();
				await HandleEventStateChange(ifsmHandler);
			}
			else if (interactType == InteractType.Dialogue)
			{
				var ifsmHandler = stateMachineBase.GetComponentInChildren<IFSMHandler>();
				await HandleDialogue(ifsmHandler);
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
		/// Check whether it's the correct item type before using it.
		/// </summary>
		void HandleUseActiveItem()
		{
			var activeObject = StageManager.Instance.PlayerController.Inventory.ActiveObject;

			if (!activeObject) return;
			if (!itemTypeCompare.HasFlag(activeObject.ItemTypeSet.ItemType)) return;

			activeObject.ParentTrans.GetComponentInChildren<IItem>().PlaceAt(placeAt.position);
			StageManager.Instance.PlayerController.Inventory.UseActiveItem();
		}

		/// <summary>
		/// Handle the orderedStateMachine.
		/// </summary>
		/// <returns></returns>
		async UniTask HandleEventStateChange(IFSMHandler ifSMHandler)
		{
			ifSMHandler?.OnBegin();
			InputManager.Instance.EnableActionMap(actionMapType);

			await orderedStateMachine.Initialize(null, interactionAssign);

			InputManager.Instance.SetToDefaultActionMap();
			ifSMHandler?.OnExit();
		}

		/// <summary>
		/// Handle only dialogue talking with interactables.
		/// </summary>
		/// <param name="ifSMHandler"></param>
		/// <returns></returns>
		async UniTask HandleDialogue(IFSMHandler ifSMHandler)
		{
			dialogueSystemTrigger.OnUse(transform);

			ifSMHandler?.OnBegin();
			headLookAt.SetLookAtPlayer(true);

			await UniTask.WaitUntil(() => !DialogueManager.Instance.isConversationActive);

			headLookAt.SetLookAtPlayer(false);
			ifSMHandler?.OnExit();
		}
	}
}

