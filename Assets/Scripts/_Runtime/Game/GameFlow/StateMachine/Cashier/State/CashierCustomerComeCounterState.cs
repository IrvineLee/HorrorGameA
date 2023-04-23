using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;


namespace Personal.FSM.Cashier
{
	public class CashierCustomerComeCounterState : StateBase
	{
		public CashierCustomerComeCounterState(CashierStateMachine cashierStateMachine) : base(cashierStateMachine) { }

		public override async UniTask OnEnter()
		{
			Debug.Log(stateMachine);

			Debug.Log("CustomerComeCounter");
			await UniTask.Delay(2000);

			//stateMachine.SetState(new )
			return;
		}

		public override async UniTask OnUpdate()
		{
			await UniTask.DelayFrame(0);
		}

		public override async UniTask OnExit()
		{
			await UniTask.DelayFrame(0);
		}
	}
}