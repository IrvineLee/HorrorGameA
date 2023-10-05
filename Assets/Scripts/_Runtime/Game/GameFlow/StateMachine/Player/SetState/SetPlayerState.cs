using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class SetPlayerState : ActorStateBase
	{
		[SerializeField] PlayerStateType playerState = PlayerStateType.Idle;

		public override UniTask OnEnter()
		{
			base.OnEnter();

			PlayerStateMachine playerFSM = StageManager.Instance.PlayerController.FSM;

			Type type = playerFSM.GetStateType(playerState);
			playerFSM.SwitchToState(type).Forget();

			return UniTask.CompletedTask;
		}
	}
}