using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Personal.GameState;
using UnityEngine.AI;

namespace Personal.FSM.Cashier
{
	public class CashierStateMachine : ActorStateMachine
	{
		List<StateBase> orderedStateList = new List<StateBase>();

		async void PlayOrderedState()
		{
			foreach (var state in orderedStateList)
			{
				state.SetFSM(this);

				await SetState(state);
				await state.OnExit();
			}
		}

		async void Update()
		{
			if (state == null) return;

			await state.OnUpdate();
		}

		/// <summary>
		/// Set the target info and ordered state depending on current scenario.
		/// </summary>
		/// <param name="orderedStateList"></param>
		public void Initialize(TargetInfo targetInfo, List<StateBase> orderedStateList)
		{
			NavMeshAgent.enabled = true;
			TargetInfo = targetInfo;

			this.orderedStateList = orderedStateList;
			PlayOrderedState();
		}
	}
}