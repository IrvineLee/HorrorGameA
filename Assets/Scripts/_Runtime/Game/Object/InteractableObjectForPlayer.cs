using System;
using UnityEngine;

using Personal.GameState;
using Personal.FSM;
using Cysharp.Threading.Tasks;
using Personal.FSM.Character;

namespace Personal.Manager
{
	public class InteractableObjectForPlayer : InteractableObject
	{
		PlayerStateMachine playerFSM;

		public override async UniTask HandleInteraction(StateMachineBase stateMachineBase, Action doLast = default)
		{
			playerFSM = (PlayerStateMachine)stateMachineBase;

			if (interactType == InteractType.Pickupable)
			{

			}
			else if (interactType == InteractType.StateChange)
			{
				await orderedStateMachine.Initialize(null, interactionAssign);
				doLast?.Invoke();
			}
			//// TODO: Item enter into inventory.
			//Debug.Log(hit.transform.name);
		}
	}
}

