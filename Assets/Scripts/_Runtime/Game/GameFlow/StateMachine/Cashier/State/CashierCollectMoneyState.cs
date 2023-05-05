using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;


namespace Personal.FSM.Cashier
{
	public class CashierCollectMoneyState : StateBase
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			Debug.Log("Begin cashier state");
			await UniTask.DelayFrame(0);

			//stateMachine.SetState(new )
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