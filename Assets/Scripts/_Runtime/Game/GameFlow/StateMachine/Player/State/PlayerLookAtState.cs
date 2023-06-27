using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Character.Player;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerLookAtState : PlayerBaseState
	{
		protected CinemachineVirtualCamera vCam;
		protected FPSController fpsController;

		public override UniTask OnEnter()
		{
			base.OnEnter();

			fpsController = StageManager.Instance.PlayerController.FPSController;

			fpsController.enabled = false;
			fpsController.ResetAnimationBlend();

			if (playerFSM.LookAtTarget)
			{
				vCam = GetComponentInChildren<CinemachineVirtualCamera>();
				vCam.LookAt = playerFSM.LookAtTarget;
				vCam.Priority = 15;
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
			playerFSM.SetLookAtTarget(null);

			return base.OnExit();
		}
	}
}