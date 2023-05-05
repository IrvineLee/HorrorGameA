using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System;
using PixelCrushers.DialogueSystem;

namespace Personal.FSM.General
{
	[Serializable]
	public class ActorTalkState : StateBase
	{
		ActorStateMachine actorStateMachine;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			actorStateMachine = (ActorStateMachine)stateMachine;
			actorStateMachine.DialogueSystemTrigger.enabled = true;

			await UniTask.WaitUntil(() => !DialogueManager.Instance.isConversationActive);

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
			actorStateMachine.DialogueSystemTrigger.enabled = false;
			await UniTask.DelayFrame(0);
		}
	}
}