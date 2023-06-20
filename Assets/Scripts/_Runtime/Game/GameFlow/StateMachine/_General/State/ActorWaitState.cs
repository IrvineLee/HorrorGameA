using UnityEngine;

using Cysharp.Threading.Tasks;
using System;
using Helper;

namespace Personal.FSM.Character
{
	[Serializable]
	public class ActorWaitState : ActorStateBase
	{
		[SerializeField] float waitDuration = 1f;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			actorStateMachine.DialogueSystemTrigger.OnUse(actorStateMachine.transform);

			RunActorAnimation();
			await UniTask.Delay(waitDuration.SecondsToMilliseconds());
			return;
		}
	}
}