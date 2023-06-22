using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;

namespace Personal.FSM.Character
{
	public class PlayerLookAtState : PlayerBaseState
	{
		protected CinemachineVirtualCamera vCam;

		public override UniTask OnEnter()
		{
			base.OnEnter();

			vCam = GetComponentInChildren<CinemachineVirtualCamera>();
			vCam.LookAt = playerFSM.LookAtTarget;
			vCam.Priority = 15;

			return UniTask.CompletedTask;
		}

		public override UniTask OnExit()
		{
			vCam.LookAt = null;
			vCam.Priority = 0;

			playerFSM.SetLookAtTarget(null);
			return base.OnExit();
		}
	}
}