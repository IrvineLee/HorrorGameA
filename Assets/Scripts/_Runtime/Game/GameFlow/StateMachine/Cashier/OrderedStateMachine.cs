using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Personal.GameState;

namespace Personal.FSM
{
	public class OrderedStateMachine : ActorStateMachine
	{
		public TargetInfo TargetInfo { get; protected set; }

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
			TargetInfo = null;
		}

		public void SetTargetInfo(TargetInfo targetInfo)
		{
			TargetInfo = targetInfo;
		}

		protected async UniTask PlayOrderedState()
		{
			foreach (var state in orderedStateList)
			{
				await SetState(state);
			}
		}
	}
}