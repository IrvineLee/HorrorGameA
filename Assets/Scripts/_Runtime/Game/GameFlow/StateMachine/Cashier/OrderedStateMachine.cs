using UnityEngine;
using UnityEngine.AI;

using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public class OrderedStateMachine : ActorStateMachine
	{
		/// <summary>
		/// Set the target info and ordered state depending on current scenario.
		/// </summary>
		/// <param name="orderedStateList"></param>
		public override async UniTask Begin(StateMachineBase initiatorFSM, TargetInfo targetInfo, InteractionAssign interactionAssign)
		{
			await base.Begin(initiatorFSM, targetInfo, interactionAssign);

			if (NavMeshAgent) NavMeshAgent.enabled = true;
			if (targetInfo != null) TargetInfo = targetInfo;

			InitiatorStateMachine = initiatorFSM;
			orderedStateList = interactionAssign.OrderedStateList;

			await PlayOrderedState();
		}
	}
}