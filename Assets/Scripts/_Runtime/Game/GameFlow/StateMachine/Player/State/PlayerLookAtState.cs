using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Cinemachine;
using Personal.Character.Player;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerLookAtState : PlayerBaseState
	{
		public bool IsStateEnded { get; private set; }

		CinemachineVirtualCamera vCam;
		PlayerController playerController;

		LookAtInfo lookAtInfo;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			IsStateEnded = false;

			playerController = StageManager.Instance.PlayerController;
			playerController.PlayerAnimatorController.ResetAnimationBlend(0.25f);

			if (playerFSM.LookAtInfo == null) return;

			lookAtInfo = playerFSM.LookAtInfo;

			vCam = GetComponentInChildren<CinemachineVirtualCamera>();
			vCam.Priority = 20;
			vCam.LookAt = lookAtInfo.LookAt;

			await UniTask.NextFrame();

			List<UniTask> uniTaskList = new();
			if (lookAtInfo.IsPersist)
			{
				var bodyRotateTask = playerController.HandleVCamPersistantLook(vCam, lookAtInfo);
				uniTaskList.Add(bodyRotateTask);
			}

			uniTaskList.Add(UniTask.WaitUntil(() => !StageManager.Instance.CameraHandler.CinemachineBrain.IsBlending));

			await UniTask.WhenAll(uniTaskList);
			IsStateEnded = true;
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			vCam.LookAt = null;
			vCam.Priority = 0;
		}
	}
}