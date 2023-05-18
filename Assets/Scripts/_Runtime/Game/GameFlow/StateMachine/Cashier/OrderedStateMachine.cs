using System.Collections.Generic;
using UnityEngine.AI;

using Personal.GameState;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Personal.FSM
{
	public class OrderedStateMachine : ActorStateMachine
	{
		/// <summary>
		/// Set the target info and ordered state depending on current scenario.
		/// </summary>
		/// <param name="orderedStateList"></param>
		public async UniTask Initialize(TargetInfo targetInfo, InteractionAssign interactionAssign)
		{
			// Wait for awake before initializing.
			await UniTask.DelayFrame(1);

			if (NavMeshAgent) NavMeshAgent.enabled = true;
			if (targetInfo != null) TargetInfo = targetInfo;

			orderedStateList = interactionAssign.OrderedStateList;
			await PlayOrderedState();
		}
	}
}