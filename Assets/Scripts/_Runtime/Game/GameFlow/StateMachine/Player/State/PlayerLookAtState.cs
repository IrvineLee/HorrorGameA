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

		protected LookAtInfo lookAtInfo;
		protected Transform parent;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			pc = StageManager.Instance.PlayerController;
			pc.FPSController.enabled = false;
			pc.PlayerAnimatorController.ResetAnimationBlend(0.25f);

			if (playerFSM.LookAtInfo == null) return;

			lookAtInfo = playerFSM.LookAtInfo;

			vCam = GetComponentInChildren<CinemachineVirtualCamera>();
			vCam.Priority = 20;
			vCam.LookAt = lookAtInfo.LookAt;

			if (!lookAtInfo.IsPersist) return;
			pc.HandlePersistantLook(vCam, lookAtInfo).Forget();
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			pc.FPSController.enabled = true;

			vCam.LookAt = null;
			vCam.Priority = 0;
		}
	}
}