using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System;

namespace Personal.FSM.Cashier
{
	[Serializable]
	public class ActorTalkState : StateBase
	{
		[SerializeField] int test = 1;

		//[SerializeField] Dialogue dialogue = null;

		public ActorTalkState(CashierStateMachine cashierStateMachine) : base(cashierStateMachine) { }

		public override async UniTask OnEnter()
		{
			Debug.Log("Begin talking state...Press mouse 0 to continue");
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));

			return;
		}

		public override async UniTask OnUpdate()
		{
			await UniTask.DelayFrame(0);
		}

		public override async UniTask OnExit()
		{
			Debug.Log("Talking ended...");
			await UniTask.DelayFrame(0);
		}
	}
}