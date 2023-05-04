using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System;
using PixelCrushers.DialogueSystem;

namespace Personal.FSM.Cashier
{
	[Serializable]
	public class ActorTalkState : StateBase
	{
		ActorStateMachine actorStateMachine;

		public override async UniTask OnEnter()
		{
			Debug.Log("Begin talking state...");

			actorStateMachine = (ActorStateMachine)stateMachine;
			actorStateMachine.DialogueSystemTrigger.enabled = true;

			await UniTask.WaitUntil(() => !DialogueManager.Instance.isConversationActive);

			return;
		}

		public override async UniTask OnUpdate()
		{
			await UniTask.DelayFrame(0);
		}

		public override async UniTask OnExit()
		{
			Debug.Log("Talking ended...");

			actorStateMachine.DialogueSystemTrigger.enabled = false;
			await UniTask.DelayFrame(0);
		}
	}
}