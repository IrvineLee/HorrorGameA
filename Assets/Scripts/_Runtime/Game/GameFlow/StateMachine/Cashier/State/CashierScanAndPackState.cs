using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;


namespace Personal.FSM.Cashier
{
	public class CashierScanAndPackState : StateBase
	{
		int currentCount, maxCount;

		public override async UniTask OnEnter()
		{
			Debug.Log("CashierScanAndPackState");

			maxCount = transform.childCount;
			await UniTask.WaitUntil(() => currentCount == maxCount);
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