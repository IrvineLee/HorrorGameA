using System;
using UnityEngine;

using Personal.FSM;
using Cysharp.Threading.Tasks;
using Personal.InputProcessing;

namespace Personal.Manager
{
	public class InteractableObjectForPlayer : InteractableObject
	{
		public override async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast = default)
		{
			if (interactType == InteractType.Pickupable)
			{
				//// TODO: Item enter into inventory.
				//Debug.Log(hit.transform.name);
			}
			else if (interactType == InteractType.StateChange)
			{
				InputManager.Instance.EnableActionMap(actionMapType);

				await orderedStateMachine.Initialize(null, interactionAssign);

				doLast?.Invoke();
				InputManager.Instance.EnableActionMap(ActionMapType.Player);
			}
		}
	}
}

