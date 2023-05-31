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
				StageManager.Instance.PlayerController.Inventory.AddItem(this);
				currentCollider.enabled = false;
				meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

				await UniTask.Yield();
			}
			else if (interactType == InteractType.StateChange)
			{
				InputManager.Instance.EnableActionMap(actionMapType);

				await orderedStateMachine.Initialize(null, interactionAssign);
			}

			doLast?.Invoke();
			InputManager.Instance.SetToDefaultActionMap();
		}
	}
}

