using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Character.Player;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerMoveToState : ActorMoveState
	{
		Transform lookAtTarget;

		public override async UniTask OnEnter()
		{
			PlayerController playerController = StageManager.Instance.PlayerController;
			float targetPitch = playerController.FPSController.CinemachineTargetPitch;

			Action<float> callbackMethod = (result) => playerController.FPSController.UpdateTargetPitch(result);
			CoroutineHelper.LerpWithinSeconds(targetPitch, 0, 1f, callbackMethod, () => playerController.FPSController.UpdateTargetPitch(0));

			await base.OnEnter();
		}

		public void SetTarget(PlayerMoveToInfo playerMoveToInfo)
		{
			moveToTarget = playerMoveToInfo.MoveToTarget;
			lookAtTarget = playerMoveToInfo.LookAtTarget;
			updateTurnTowardsSpeed = playerMoveToInfo.UpdateTurnTowardsSpeed;
		}

		protected override Transform GetTarget()
		{
			return moveToTarget;
		}

		protected override Transform GetLookAtTarget()
		{
			return lookAtTarget;
		}
	}
}