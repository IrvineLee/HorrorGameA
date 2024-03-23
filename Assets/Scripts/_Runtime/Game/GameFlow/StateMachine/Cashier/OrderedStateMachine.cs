using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public class OrderedStateMachine : ActorStateMachine
	{
		protected List<StateBase> orderedStateList = new List<StateBase>();

		/// <summary>
		/// Set the target info and ordered state depending on current scenario.
		/// </summary>
		/// <param name="orderedStateList"></param>
		public override async UniTask Begin(InteractionAssign interactionAssign, StateMachineBase initiatorFSM = null)
		{
			if (NavMeshAgent) NavMeshAgent.enabled = true;

			InitiatorStateMachine = initiatorFSM;
			orderedStateList = interactionAssign.OrderedStateList;

			await PlayOrderedState();
			interactionAssign.DestroyInteraction();
		}

		protected async UniTask PlayOrderedState()
		{
			foreach (var state in orderedStateList)
			{
				await SetState(state);
			}

			await PlayEndState();
		}
	}
}