using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System;
using PixelCrushers.DialogueSystem;

namespace Personal.FSM.Character
{
	[Serializable]
	public class ActorTalkState : ActorStateBase
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			actorStateMachine.DialogueSystemTrigger.OnUse(actorStateMachine.transform);

			RunActorAnimation();
			await UniTask.WaitUntil(() => DialogueManager.Instance && !DialogueManager.Instance.isConversationActive);
		}
	}
}