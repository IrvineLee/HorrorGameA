using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;


namespace Personal.FSM.Cashier
{
	public class CashierScanAndPackState : StateBase
	{
		public override async UniTask OnEnter()
		{
			Debug.Log("Begin cashier state");
			await UniTask.Delay(3000);

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