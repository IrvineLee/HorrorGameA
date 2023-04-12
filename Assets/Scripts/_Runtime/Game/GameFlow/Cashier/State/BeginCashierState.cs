using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Personal.FSM;
using Cysharp.Threading.Tasks;

namespace Personal.GameFlow.CashierSystem
{
	public class BeginCashierState : State
	{
		public override async UniTask OnEnter()
		{
			Debug.Log("Begin cashier state");
			await UniTask.DelayFrame(0);
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