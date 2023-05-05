using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM.Cashier
{
	public class CashierScanAndPackState : StateBase
	{
		CashierStateMachine cashierStateMachine;
		int currentCount, maxCount;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			Debug.Log("CashierScanAndPackState");

			cashierStateMachine = (CashierStateMachine)stateMachine;

			maxCount = cashierStateMachine.transform.childCount;
			await UniTask.WaitUntil(() => currentCount == maxCount);

			return;
		}

		public override async UniTask OnUpdate()
		{
			await base.OnUpdate();

			await UniTask.DelayFrame(0);
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
			await UniTask.DelayFrame(0);
		}
	}
}