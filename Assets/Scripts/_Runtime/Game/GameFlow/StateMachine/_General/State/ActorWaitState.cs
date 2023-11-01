using UnityEngine;

using Cysharp.Threading.Tasks;
using System;
using Helper;

namespace Personal.FSM.Character
{
	public class ActorWaitState : ActorStateBase
	{
		[SerializeField] float waitDuration = 1f;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			RunActorAnimation();
			await UniTask.Delay(waitDuration.SecondsToMilliseconds());

			return;
		}
	}
}