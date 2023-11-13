using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Character.Player;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerPOVControlState : PlayerBaseState
	{
		protected CinemachineVirtualCamera vCam;
		protected FPSController fpsController;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			fpsController = StageManager.Instance.PlayerController.FPSController;
			fpsController.enabled = true;

			if (playerFSM.LookAtTarget)
			{
				vCam = GetComponentInChildren<CinemachineVirtualCamera>();
				vCam.LookAt = playerFSM.LookAtTarget;
				vCam.Priority = 30;
			}
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			if (!vCam) return;

			vCam.LookAt = null;
			vCam.Priority = 0;
		}
	}
}