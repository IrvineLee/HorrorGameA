using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System;

namespace Personal.FSM.Cashier
{
	[Serializable]
	public class TalkState : StateBase
	{
		[SerializeField] int test = 1;

		//[SerializeField] Dialogue dialogue = null;

		public TalkState(CashierStateMachine cashierStateMachine) : base(cashierStateMachine) { Debug.Log("ASDSAd"); }

		public override async UniTask OnEnter()
		{
			Debug.Log(stateMachine.gameObject.name);

			Debug.Log("Begin talking state...Press mouse 0 to continue");
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));

			Debug.Log("Talking ended...");
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