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
		protected PlayerController pc;
		protected PlayerAnimatorController playerAnimatorController;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			pc = StageManager.Instance.PlayerController;
			pc.FPSController.enabled = false;
			pc.PlayerAnimatorController.ResetAnimationBlend(0.25f);

			playerFSM.SetLookAtTarget(pc.FSM.LookAtTarget);

			if (playerFSM.LookAtTarget)
			{
				vCam = GetComponentInChildren<CinemachineVirtualCamera>();
				vCam.LookAt = playerFSM.LookAtTarget;
				vCam.Priority = 20;
			}
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			pc.FPSController.enabled = true;

			if (playerFSM.LookAtTarget)
			{
				vCam.LookAt = null;
				vCam.Priority = 0;
			}
		}
	}
}