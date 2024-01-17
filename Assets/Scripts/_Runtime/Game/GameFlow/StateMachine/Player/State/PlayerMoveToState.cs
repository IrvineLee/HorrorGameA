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
		public bool IsCompleted { get; private set; }

		public override async UniTask OnEnter()
		{
			PlayerController playerController = StageManager.Instance.PlayerController;
			float targetPitch = playerController.FPSController.CinemachineTargetPitch;

			Action<float> callbackMethod = (result) => playerController.FPSController.UpdateTargetPitch(result);
			CoroutineHelper.LerpWithinSeconds(targetPitch, 0, 1f, callbackMethod, () => playerController.FPSController.UpdateTargetPitch(0));

			await base.OnEnter();
			IsCompleted = true;
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			IsCompleted = false;
		}

		public void SetTarget(PlayerMoveToInfo playerMoveToInfo)
		{
			moveToTarget = playerMoveToInfo.MoveToTarget;
			turnTowardsTarget = playerMoveToInfo.TurnTowardsTarget;
			updateTurnTowardsSpeed = playerMoveToInfo.UpdateTurnTowardsSpeed;
		}

		Transform turnTowardsTarget;

		protected override Transform GetTarget()
		{
			return moveToTarget;
		}

		protected override Transform GetTurnTowardsTarget()
		{
			return turnTowardsTarget;
		}
	}
}