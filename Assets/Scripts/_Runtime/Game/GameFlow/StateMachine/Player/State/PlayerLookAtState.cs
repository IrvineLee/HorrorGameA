using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Character.Player;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerLookAtState : PlayerBaseState
	{
		CinemachineVirtualCamera vCam;
		PlayerController playerController;

		LookAtInfo lookAtInfo;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			playerController = StageManager.Instance.PlayerController;
			playerController.FPSController.enabled = false;
			playerController.PlayerAnimatorController.ResetAnimationBlend(0.25f);

			if (playerFSM.LookAtInfo == null) return;

			lookAtInfo = playerFSM.LookAtInfo;

			vCam = GetComponentInChildren<CinemachineVirtualCamera>();
			vCam.Priority = 20;
			vCam.LookAt = lookAtInfo.LookAt;

			if (!lookAtInfo.IsPersist) return;
			playerController.HandleVCamPersistantLook(vCam, lookAtInfo).Forget();
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			playerController.FPSController.enabled = true;

			vCam.transform.localRotation = Quaternion.identity;
			vCam.LookAt = null;
			vCam.Priority = 0;
		}
	}
}