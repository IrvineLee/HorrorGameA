using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.FSM.Cashier
{
	public class CashierStateMachine : StateMachineBase
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
		/// Set the ordered state depending on current scenario.
		/// </summary>
		/// <param name="orderedStateList"></param>
		public void SetAndPlayOrderedStateList(List<StateBase> orderedStateList)
		{
			this.orderedStateList = orderedStateList;
			PlayOrderedState();
		}
	}
}