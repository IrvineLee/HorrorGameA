using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Personal.FSM;

namespace Personal.GameFlow.CashierSystem
{
	public class CashierSystem : StateMachine<CashierState>
	{
		async void OnEnable()
		{
			await state.OnEnter();
		}

		public async void OnUpdate()
		{
			await state.OnUpdate();
		}

		public async void OnExit()
		{
			await state.OnExit();
		}
	}
}