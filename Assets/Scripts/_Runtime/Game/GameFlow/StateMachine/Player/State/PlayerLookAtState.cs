using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Character.Player;
using Personal.Manager;
using Personal.Character.Animation;

namespace Personal.FSM.Character
{
	public class PlayerLookAtState : PlayerBaseState
	{
		protected CinemachineVirtualCamera vCam;
		protected FPSController fpsController;
		protected PlayerAnimatorController playerAnimatorController;

		public override UniTask OnEnter()
		{
			base.OnEnter();

			PlayerController pc = StageManager.Instance.PlayerController;
			fpsController = pc.FPSController;
			playerAnimatorController = pc.PlayerAnimatorController;

			fpsController.enabled = false;
			playerAnimatorController.ResetAnimationBlend();

			if (playerFSM.LookAtTarget)
			{
				vCam = GetComponentInChildren<CinemachineVirtualCamera>();
				vCam.LookAt = playerFSM.LookAtTarget;
				vCam.Priority = 20;
			}

			return UniTask.CompletedTask;
		}

		public override UniTask OnExit()
		{
			if (playerFSM.LookAtTarget)
			{
				vCam.LookAt = null;
				vCam.Priority = 0;
			}

			fpsController.enabled = true;
			return base.OnExit();
		}
	}
}