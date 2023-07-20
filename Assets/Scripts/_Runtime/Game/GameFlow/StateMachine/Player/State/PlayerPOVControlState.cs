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

		public override UniTask OnEnter()
		{
			base.OnEnter();

			fpsController = StageManager.Instance.PlayerController.FPSController;
			fpsController.enabled = true;

			if (playerFSM.LookAtTarget)
			{
				vCam = GetComponentInChildren<CinemachineVirtualCamera>();
				vCam.LookAt = playerFSM.LookAtTarget;
				vCam.Priority = 30;
			}

			return UniTask.CompletedTask;
		}

		public override UniTask OnExit()
		{
			vCam.LookAt = null;
			vCam.Priority = 0;

			return base.OnExit();
		}
	}
}