using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Cashier
{
	public class CashierStateMachine : StateMachine
	{
		void OnEnable()
		{
			SetState(new CashierTalkState(this));
		}

		async void Update()
		{
			if (state == null) return;

			await state.OnUpdate();
		}

		async void OnDisable()
		{
			if (state == null) return;

			await state.OnExit();
			state = null;
		}
	}
}