using UnityEngine;

using Helper;
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

		CoroutineRun lookAtCR = new();

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
			await HandlePersistantLook();
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			pc.FPSController.enabled = true;

			if (playerFSM.LookAtInfo == null) return;

			vCam.LookAt = null;
			vCam.Priority = 0;
		}

		async UniTask HandlePersistantLook()
		{
			parent = vCam.transform.parent;
			vCam.transform.SetParent(null);

			lookAtCR?.StopCoroutine();

			// Make sure the vCam is out of it's parent.
			await UniTask.NextFrame();

			if (!lookAtInfo.IsInstant) RotateByAnimation();

			await UniTask.WaitUntil(() => !StageManager.Instance.CameraHandler.CinemachineBrain.IsBlending, cancellationToken: this.GetCancellationTokenOnDestroy());

			// Rotate the player's transform to look at target, on the horizontal axis.
			Vector3 lookAtPos = lookAtInfo.LookAt.position;
			lookAtPos.y = 0;

			pc.transform.LookAt(lookAtPos);
			vCam.transform.SetParent(parent);

			pc.FPSController.UpdateTargetPitch(vCam.transform.eulerAngles.x);
		}

		void RotateByAnimation()
		{
			var cinemachineBrain = StageManager.Instance.CameraHandler.CinemachineBrain;
			float duration = cinemachineBrain.m_DefaultBlend.BlendTime;

			var direction = pc.transform.position.GetNormalizedDirectionTo(lookAtInfo.LookAt.position);
			direction.y = 0;

			var endRotation = Quaternion.LookRotation(direction);
			lookAtCR = CoroutineHelper.QuaternionLerpWithinSeconds(pc.transform, pc.transform.rotation, endRotation, duration, space: Space.World);
		}
	}
}